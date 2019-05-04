using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class BigInteger
    {
        private bool sign;

        private readonly List<byte> number = new List<byte>();


        public BigInteger(string s)
        {
            var output = new BigInteger(new List<byte> { 0 });

            bool flag = false;

            int start = 0;

            if (s[0] == '-')
            {
                flag = true;
                start++;
            }

            for (int i = start; i < s.Length; i++)
            {
                output *= 10;
                output += new BigInteger(new List<byte> { (byte)(s[i] - '0') });
            }

            number = output.number;

            sign = flag;
        }

        public BigInteger(List<byte> bytes)
        {
            number.AddRange(bytes);
        }

        public BigInteger(List<byte> bytes, bool sign)
        {
            number.AddRange(bytes);
            this.sign = sign;
        }

        public BigInteger()
        {
        }


        public static BigInteger Addition(BigInteger first, BigInteger second)
        {
            var sum = new BigInteger(first.number);
            var isCarry = false;
            for (int i = 0; i < second.number.Count; i++)
            {
                var add = sum.number[i] + second.number[i] + (isCarry ? 1 : 0);
                isCarry = (add >> 8) > 0;
                sum.number[i] = (byte)add;
            }
            var carryIndex = second.number.Count;
            while (isCarry)
            {
                if (sum.number.Count > carryIndex)
                {
                    sum.number[carryIndex] += 1;
                    if (sum.number[carryIndex] != 0) isCarry = false;
                    carryIndex++;
                }
                else
                {
                    sum.number.Add(1);
                    isCarry = false;
                }
            }
            return sum;
        }
        public static BigInteger operator +(BigInteger first, BigInteger second)
        {
            if (Compare(first, second) == -1)
            {
                var temp = second;
                second = first;
                first = temp;
            }
            var result = first.sign == second.sign ? Addition(first, second) : Subtraction(first, second);
            result.sign = first.sign;
            return result;
        }


        public static BigInteger Subtraction(BigInteger first, BigInteger second)
        {
            var sum = new BigInteger(first.number);
            var isCarry = false;
            for (int i = 0; i < second.number.Count; i++)
            {
                var isCarryNext = sum.number[i] < second.number[i];
                sum.number[i] -= (byte)(second.number[i] + (isCarry ? 1 : 0));
                isCarry = isCarryNext;
            }
            var carryIndex = second.number.Count;
            while (isCarry)
            {
                if (sum.number[carryIndex] != 0)
                    isCarry = false;
                sum.number[carryIndex] -= 1;
                carryIndex++;
            }
            var lastIndex = sum.number.Count;
            while (lastIndex != 0 && sum.number[--lastIndex] == 0) ;
            sum.number.RemoveRange(lastIndex + 1, sum.number.Count - lastIndex - 1);

            return sum;
        }

        public static BigInteger operator -(BigInteger first, BigInteger second)
        {
            if (first.Equals(second))
                second = new BigInteger(second.number) { sign = !first.sign };
            else
                second.sign = !second.sign;

            var result = first + second;

            second.sign = !second.sign;

            return result;
        }


        public static BigInteger operator *(BigInteger number, byte b)
        {
            var result = new BigInteger(number.number);

            byte carry = 0;

            for (int i = 0; i < result.number.Count; i++)
            {
                int intermediate = result.number[i] * b;
                var addCarry = ((intermediate & 255) + carry) >> 8;
                result.number[i] = (byte)((intermediate & 255) + carry);
                carry = (byte)((intermediate >> 8) + addCarry);
            }

            if (carry > 0)
                result.number.Add(carry);

            return result;
        }

        public static BigInteger operator *(BigInteger first, BigInteger second)
        {
            var result = new BigInteger();
            if (Compare(first, second) == -1)
            {
                var temp = second;
                second = first;
                first = temp;
            }

            for (int i = 0; i < second.number.Count; i++)
                MultiplicationAdd(result, first * second.number[i], i);
            var lastIndex = result.number.Count;
            while (lastIndex != 0 && result.number[--lastIndex] == 0) ;
            result.number.RemoveRange(lastIndex + 1, result.number.Count - lastIndex - 1);
            result.sign = first.sign != second.sign;
            return result;
        }

        private static void MultiplicationAdd(BigInteger result, BigInteger longNumber, int index)
        {

            var isCarry = false;
            for (int i = 0; i < longNumber.number.Count; i++)
            {
                if (result.number.Count == i + index) result.number.Add(0);
                var add = result.number[i + index] + longNumber.number[i] + (isCarry ? 1 : 0);

                isCarry = (add >> 8) > 0;
                result.number[i + index] = (byte)add;

            }
            var carryIndex = longNumber.number.Count + index;
            while (isCarry)
            {
                if (result.number.Count > carryIndex)
                {
                    result.number[carryIndex] += 1;
                    if (result.number[carryIndex] != 0) isCarry = false;
                    carryIndex++;
                }
                else
                {
                    result.number.Add(1);
                    isCarry = false;
                }
            }

        }


        public static Tuple<BigInteger, BigInteger> Divide(BigInteger dividend, BigInteger divider)
        {
            var result = new BigInteger();
            var residue = new BigInteger(dividend.number, dividend.sign);

            if (Compare(dividend, divider) == -1)
            {
                result.number.Add(0);
                return new Tuple<BigInteger, BigInteger>(result, residue);
            }
            var discharge = residue.number.Count - divider.number.Count;

            for (int i = 0; i < discharge; i++) divider.number.Insert(0, 0);

            for (int i = 0; i < discharge; i++)
            {
                if (Compare(residue, divider) == -1)
                    result.number.Insert(0, 0);
                else
                {
                    var cont = GetDivider(residue, divider);
                    result.number.Insert(0, cont.Item1);
                    residue = cont.Item2;

                }
                divider.number.RemoveAt(0);
            }
            if (Compare(residue, divider) == -1)
                result.number.Insert(0, 0);
            else
            {
                var cont = GetDivider(residue, divider);
                result.number.Insert(0, cont.Item1);
                residue = cont.Item2;

            }


            var lastIndex = result.number.Count;
            while (lastIndex != 0 && result.number[--lastIndex] == 0) ;
            result.number.RemoveRange(lastIndex + 1, result.number.Count - lastIndex - 1);
            result.sign = dividend.sign != divider.sign;
            return new Tuple<BigInteger, BigInteger>(result, residue);
        }

        private static Tuple<Byte, BigInteger> GetDivider(BigInteger residue, BigInteger second)
        {
            var down = 0;
            var up = 256;
            while (down + 1 < up)
            {
                var newValue = second * (byte)((down + up) / 2);

                if (Compare(residue, newValue) == -1)
                {
                    up = (down + up) / 2;
                }
                else
                {
                    down = (down + up) / 2;
                }
            }

            var residueOut = Subtraction(residue, second * (byte)down);
            residueOut.sign = residue.sign;

            return new Tuple<Byte, BigInteger>((byte)down, residueOut);
        }

        public static BigInteger operator /(BigInteger first, BigInteger second)
        {
            return Divide(first, second).Item1;
        }
        public static BigInteger operator %(BigInteger first, BigInteger second)
        {
            return Divide(first, second).Item2;
        }

        public static bool operator >(BigInteger first, BigInteger second)
        {
            if (first.sign)
            {
                if (second.sign)
                    return Compare(first, second) == -1;

                return false;
            }

            if (second.sign)
                return true;

            return Compare(first, second) == 1;
        }

        public static bool operator <(BigInteger first, BigInteger second)
        {
            return second > first;
        }

        public static bool operator <=(BigInteger first, BigInteger second)
        {
            return (first == second) || (first < second);
        }

        public static bool operator >=(BigInteger first, BigInteger second)
        {
            return (first == second) || (first > second);
        }

        public static bool operator ==(BigInteger first, BigInteger second)
        {
            if (first.sign != second.sign) return false;

            return Compare(first, second) == 0;
        }

        public static bool operator !=(BigInteger first, BigInteger second)
        {
            return !(first == second);
        }

        public static int Compare(BigInteger first, BigInteger second)
        {
            if (first.number.Count < second.number.Count)
                return -1;

            if (first.number.Count > second.number.Count)
                return 1;

            for (int i = first.number.Count - 1; i >= 0; i--)
            {
                if (first.number[i] < second.number[i])
                    return -1;

                if (first.number[i] > second.number[i])
                    return 1;
            }
            return 0;
        }


        public override string ToString()
        {
            var builder = new StringBuilder();
            var start = 0;
            if (sign)
            {
                builder.Append('-');
                start++;
            }
            var forOut = new BigInteger(number);
            while (forOut.number.Count > 1 || forOut.number[0] > 9)
            {
                var div = forOut / 10;
                var mod = forOut % 10;
                forOut = div;
                builder.Insert(start, (char)('0' + mod.number[0]));
            }
            builder.Insert(start, (char)('0' + forOut.number[0]));
            return builder.ToString().TrimStart(' ');
        }


        public static implicit operator BigInteger(int value)
        {
            return new BigInteger(value.ToString());
        }
        public static implicit operator BigInteger(uint value)
        {
            return new BigInteger(value.ToString());
        }

        public static implicit operator BigInteger(long value)
        {
            return new BigInteger(value.ToString());
        }
        public static implicit operator BigInteger(ulong value)
        {
            return new BigInteger(value.ToString());
        }

        public static implicit operator BigInteger(string value)
        {
            return new BigInteger(value);
        }

        public static BigInteger Factorial(BigInteger number)
        {
            BigInteger result = 1;
            for (int i = 2; i <= number; ++i)
                result *= i;
            return result;
        }
    }
}
