using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithms {
    public class MultiplicativePersistence {

        /// <summary>
        /// Calculates the persistence of any number n.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int CalculatePersistence(Int64 n) {

            if (n < 0) {
                throw new ArgumentException("Only positive integers are allowed.");
            }

            var digits = global::Utilities.Arrays.GetArray(n);

            return PreFilter(digits);
        }

        private int PreFilter(long[] digits) {
            
            if (IsInvalid(digits)) {
                return 0;
            }

            return Execute(digits, 0);
        }

        private int Execute(long[] digits, int counter) {
            
            if (digits.Length <= 1) {
                return counter;
            }

            var n = digits.Aggregate((x1, x2) => x1 * x2);

            return Execute(global::Utilities.Arrays.GetArray(n), counter++);
        }

        private bool IsInvalid(long[] digits) {

            if (digits.Any(x => x == 5)) {
                return true;
            }

            if (GetDigitCount(2, digits) > 1 || GetDigitCount(3, digits) > 1 || GetDigitCount(4, digits) > 1) {
                return true;
            }

            return false;
        }

        private int GetDigitCount(int value, long[] digits) {
            return digits.Count(x => x == value);
        }

    }
}
