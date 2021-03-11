using System;
using System.Collections.Generic;
using System.Text;

namespace Executors.EventArguments {
    public class ResultEventArgument<TInput, TResult> : EventArgs {
        
        public readonly TInput Input;

        public readonly TResult Result;

        public ResultEventArgument(TInput input, TResult result) {
            this.Input = input;
            this.Result = result;
        }

        public override string ToString() {
            return $"Input: [{this.Input}], returned: [{this.Result}]";
        }
    }
}
