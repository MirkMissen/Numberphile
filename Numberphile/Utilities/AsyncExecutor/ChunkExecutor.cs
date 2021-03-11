using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities {
    public class ChunkExecutor<TResult, TBoundary> {

        private readonly Func<TBoundary, TResult> _workFunction;

        private readonly Func<TBoundary> _nextChunkBoundary;

        private readonly Action<TResult> _resultSubmit;
        
        private readonly BlockingCollection<InternalTask> _internalTasks = new BlockingCollection<InternalTask>();
        
        public ChunkExecutor(Func<TBoundary, TResult> function, Func<TBoundary> nextChunkBoundary, Action<TResult> resultSubmit) {
            this._workFunction = function;
            this._nextChunkBoundary = nextChunkBoundary;
            this._resultSubmit = resultSubmit;
        }

        public async Task StartRunners(int count, CancellationToken token) {
            var spawned = new List<Task>();

            for (int i = 0; i < count; i++) {
                spawned.Add(RunExecutor(token));
            }

            spawned.Add(TaskAggregator(token));

            await Task.WhenAll(spawned);
        }

        private async Task RunExecutor(CancellationToken token) {
            await Task.Run(() => {

                while (!token.IsCancellationRequested) {
                    var internalTask = GetNextTask();
                    var result = this._workFunction.Invoke(internalTask.Boundary);
                    internalTask.SetResult(result);
                }
            }, token);
        }

        private InternalTask GetNextTask() {

            lock (this) {
                var boundary = this._nextChunkBoundary.Invoke();
                var t = new InternalTask(boundary);
                this._internalTasks.Add(t);
                return t;
            }
        }

        private async Task TaskAggregator(CancellationToken token) {
            while (!token.IsCancellationRequested) {

                var result = this._internalTasks.Take(token);

                while (result.IsCompleted == false) {
                    await Task.Delay(10000);
                }

                this._resultSubmit(result.Result);
            }
        }

        private class InternalTask {

            public TBoundary Boundary { get; }
            public TResult Result  { get; private set; }
            public bool IsCompleted  { get; private set; }

            public InternalTask(TBoundary boundary) {
                Boundary = boundary;

                IsCompleted = false;
            }

            public void SetResult(TResult result) {
                this.Result = result;
                this.IsCompleted = true;
            }

        }



    }




}
