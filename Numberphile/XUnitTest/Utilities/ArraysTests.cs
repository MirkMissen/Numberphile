using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTest.Utilities {
    public class ArraysTests {

        private readonly ITestOutputHelper output;

        public ArraysTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Number_TransformedIntoSingleDigitArray() {

            var value = 123456789;

            var output = global::Utilities.Arrays.GetArray(value);
            
            for (int i = 0; i < 9; i++) {
            
                this.output.WriteLine(output[i].ToString());
                
                Assert.Equal(i+1, output[i]);
            }

        }

    }
}
