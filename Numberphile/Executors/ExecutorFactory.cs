using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using Executors.ConcreteExecutors;
using Executors.Interfaces;

namespace Executors {
    public class ExecutorFactory {

        private readonly CancellationTokenSource _cancellationTokenSource;

        public ExecutorFactory() {
            this._cancellationTokenSource = new CancellationTokenSource();
        }

        public IExecutor<BigInteger, int> GetMultiplicativePersistenceExecutor() {
            return new MultiplicativePersistenceExecutor(this._cancellationTokenSource.Token);
        }

        public void StopExecutors() {
            this._cancellationTokenSource.Cancel();
        }

    }
}
