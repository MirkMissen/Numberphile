using System;
using System.Numerics;
using System.Threading.Tasks;
using Executors;

namespace ConsoleRunner {
    class Program {
        static async Task Main(string[] args) {

            var factory = new ExecutorFactory();

            var executor = factory.GetMultiplicativePersistenceExecutor();

            executor.OnResult += Executor_SubResults;

            await executor.Execute();
        }

        private static void Executor_SubResults(object sender, Executors.EventArguments.ResultEventArgument<BigInteger, int> e) {
            Console.WriteLine(e);
        }
    }
}
