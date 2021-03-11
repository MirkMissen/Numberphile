using System;
using System.Numerics;
using Xunit;

namespace XUnitTest.MultiplicativePersistence {
    public class MultiplicativePersistenceTests {

        [Theory]
        [InlineData(277777788888899, 11)]
        public void ShouldCalculateCorrectPersistence(Int64 number, int expectedResult) {

            var algo = new global::Algorithms.MultiplicativePersistence();
            
            var result = algo.CalculatePersistence(number);

            Assert.Equal(result, expectedResult);
        }
        
    }
}
