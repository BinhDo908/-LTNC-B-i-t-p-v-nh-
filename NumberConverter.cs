using ChangeBaseNumberSystem.Models;
using System;
using System.Numerics;
using System.Text;

namespace B4_NumberConverter
{
    public class NumberConverter
    {
        public bool TryConvert(string input, BaseSystem from, BaseSystem to,
                               out string? output, out string? error)
        {
            output = null;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Emty String.";
                return false;
            }

            string cleaned = Clean(input, from);

            if (!TryParseBase(cleaned, (int)from, out BigInteger value, out string? parseErr))
            {
                error = parseErr;
                return false;
            }

            output = ToBaseString(value, (int)to);
            return true;
        }

        private string Clean(string s, BaseSystem from)
        {
            string t = s.Trim();
            bool isNeg = t.StartsWith("-");
            if (isNeg) t = t.Substring(1).TrimStart();

            if (from == BaseSystem.Binary && t.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
                t = t.Substring(2);
            else if (from == BaseSystem.Hexadecimal && t.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                t = t.Substring(2);

            t = t.Replace(" ", string.Empty).Replace("_", string.Empty);
            return isNeg ? "-" + t : t;
        }

        private bool TryParseBase(string input, int fromBase, out BigInteger value, out string? error)
        {
            value = BigInteger.Zero;
            error = null;

            bool isNeg = input.StartsWith("-");
            string core = isNeg ? input.Substring(1) : input;

            BigInteger acc = BigInteger.Zero;
            foreach (char ch in core)
            {
                int digit = DigitValue(ch);
                if (digit < 0 || digit >= fromBase)
                {
                    error = $"Digit '{ch}' invalid {fromBase}.";
                    return false;
                }
                acc = acc * fromBase + digit;
            }

            value = isNeg ? BigInteger.Negate(acc) : acc;
            return true;
        }

        private int DigitValue(char ch)
        {
            if (ch >= '0' && ch <= '9') return ch - '0';
            if (ch >= 'A' && ch <= 'F') return 10 + (ch - 'A');
            if (ch >= 'a' && ch <= 'f') return 10 + (ch - 'a');
            return -1;
        }

        private string ToBaseString(BigInteger value, int toBase)
        {
            if (value.IsZero) return "0";

            bool isNeg = value.Sign < 0;
            if (isNeg) value = BigInteger.Negate(value);

            StringBuilder sb = new StringBuilder();
            BigInteger b = new BigInteger(toBase);

            while (value > 0)
            {
                BigInteger rem;
                value = BigInteger.DivRem(value, b, out rem);
                sb.Append(DigitChar((int)rem));
            }

            char[] arr = sb.ToString().ToCharArray();
            Array.Reverse(arr);
            string core = new string(arr);

            return isNeg ? "-" + core : core;
        }

        private char DigitChar(int v)
        {
            if (v >= 0 && v <= 9) return (char)('0' + v);
            return (char)('A' + (v - 10));
        }
    }
}
