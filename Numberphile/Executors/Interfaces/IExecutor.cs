using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Executors.EventArguments;

namespace Executors.Interfaces {

    /// <summary>
    /// Defines a contract for starting an execuor.
    /// </summary>
    /// <typeparam name="Result"></typeparam>
    public interface IExecutor<TInput, TResult> {

        /// <summary>
        /// For executors that output multiple results or endless running simulations.
        /// </summary>
        event EventHandler<ResultEventArgument<TInput, TResult>> OnResult;

        /// <summary>
        /// Defines the collection of all results found.
        /// </summary>
        Dictionary<TResult, TInput> Results { get; }

        /// <summary>
        /// Executes until a final result has been found.
        /// </summary>
        /// <returns></returns>
        Task Execute();

    }
}
