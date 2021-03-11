using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace UnitTests.MultiplicatePersistence {
    class MultiplicatePersistenceTests {

        [Theory]
        [InlineData(277777788888899, 11)]
        public void ShouldCalculateCorrectPersistence(int number, int expectedResult) {

            var algo = new MultiplicativePersistence.MultiplicatePersistence();

            var bigInt = new BigInteger(number);

            var result = algo.CalculatePersistence(bigInt);

            Assert.Equal(result, expectedResult);
        }


    }
}
