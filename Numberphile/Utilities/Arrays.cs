using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Utilities {

    /// <summary>
    /// Defines array utilities.
    /// </summary>
    public class Arrays {

        /// <summary>
        /// Transforms a <see cref="BigInteger"/> into an array if its digits, e.g.
        /// <para/>
        /// 123456789 => [1,2,3,4,5,6,7,8,9]
        /// </summary>
        /// <param name="num">Defines the value to be split.</param>
        /// <returns></returns>
        public static Int64[] GetArray(Int64 num) {

            var mod = 10;
            var digits = new List<Int64>();
            while (num != 0) {

                // we know this has to be between 0-9.
                var modResult = (num % mod);
                
                digits.Insert(0, modResult);
                num = num / 10;
            }

            return digits.ToArray();
        }

    }
}
