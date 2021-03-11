using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Executors.EventArguments;
using Executors.Interfaces;
using Utilities;
using MultiplicativePersistence = Algorithms.MultiplicativePersistence;

namespace Executors.ConcreteExecutors {
    class MultiplicativePersistenceExecutor : IExecutor<BigInteger, int> {

        private const int ChunkSize = 100000000;

        private const int Executors = 7;
        
        public event EventHandler<ResultEventArgument<BigInteger, int>> OnResult;

        private readonly CancellationToken _cancellationToken;

        private readonly MultiplicativePersistence _algorithm;

        private DateTime _startTime;

        public Dictionary<int, BigInteger> Results { get; }

        public MultiplicativePersistenceExecutor(CancellationToken token) {
            this._cancellationToken = token;
            this._algorithm = new MultiplicativePersistence();

            this.Results = new Dictionary<int, BigInteger>();
        }


        public async Task Execute() {
            await QuickExecute();
        }

        public async Task QuickExecute() {

            this._startTime = DateTime.Now;

            var bp = new BoundaryProvider(ChunkSize);
            
            var exe = new ChunkExecutor<ProcessingResult, ChunkBoundary>(
                ProcessChunk, bp.GetNextBonBoundary, HandleResult);

            await exe.StartRunners(Executors, this._cancellationToken);

        }

        private void HandleResult(ProcessingResult processingResult) {

            foreach (var entry in processingResult.results) {

                var key = entry.Key;

                // we are only looking for the first; as the first is the lowest input value.
                if (this.Results.ContainsKey(key)) continue;

                this.Results.Add(entry.Key, entry.Value);
                this.OnResult?.Invoke(this, new ResultEventArgument<BigInteger, int>(entry.Value, entry.Key));
            }

        }

        private ProcessingResult ProcessChunk(ChunkBoundary boundary) {

            //if (boundary.BondaryCount % 1 == 0) {

                var totalChecks = boundary.BondaryCount * ChunkSize;
                var totalSeconds = (DateTime.Now - this._startTime).TotalSeconds + 1;

                var checkPrsec = (totalChecks / totalSeconds);

                var checksPrSecStr = FormatInteger((int) (checkPrsec));

                Console.WriteLine($"Finished boundary: {FormatInteger(boundary.End)} => {checksPrSecStr}/s");


                var remaningTo10 = (3778888999 - totalChecks) / checkPrsec;
                var remaningTo11 = (277777788888899 - totalChecks) / checkPrsec;

                var minutesTo10 = remaningTo10 / 60;
                var daysTo11 = remaningTo11 / 60 / 60 / 24;
                
                Console.WriteLine($"Time until '3778888999': {(int)minutesTo10} minutes.");
                Console.WriteLine($"Time until '277777788888899': {(int)daysTo11} days.");
            //}

            var resultSet = new ProcessingResult();

            for (Int64 i = boundary.Start; i < boundary.End; i++) {
                var result = _algorithm.CalculatePersistence(i);

                // we are only looking for the first; as the first is the lowest input value.
                if (resultSet.results.ContainsKey(result)) continue;
                resultSet.results.Add(result, i);
            }

            return resultSet;
        }

        private string FormatInteger(Int64 n) {

            // Gets a NumberFormatInfo associated with the en-US culture.
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            nfi.CurrencyDecimalSeparator = ",";
            nfi.CurrencyGroupSeparator = ".";
            nfi.CurrencySymbol = "";
            nfi.NumberDecimalDigits = 0;
            return  n.ToString("C", nfi);
        }

        private class ChunkBoundary {
            public Int64 Start { get; }
            public Int64 End { get; }

            public Int64 BondaryCount { get; }

            public ChunkBoundary(Int64 start, Int64 end, Int64 bondaryCount) {
                Start = start;
                End = end;
                BondaryCount = bondaryCount;
            }
        }

        private class ProcessingResult {

            public readonly Dictionary<int, Int64> results = new Dictionary<int, Int64>();

        }

        private class BoundaryProvider {

            private readonly Int64 _chunkSize;

            private Int64 Count;

            public BoundaryProvider(Int64 chunkSize) {
                this._chunkSize = chunkSize;
                Count = 0;
            }

            public ChunkBoundary GetNextBonBoundary() {
                var b = Count * _chunkSize;

                Int64 low = b + 1;
                Int64 top = b + _chunkSize;

                var boundary = new ChunkBoundary(low, top, Count);

                Count++;

                return boundary;
            }
        }


    }
}
