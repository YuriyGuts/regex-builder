using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeCharacterRange : RegexNode
    {
        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public char RangeStart { get; set; }
        public char RangeEnd { get; set; }

        public bool IsNegative { get; set; }
        public bool UseCharacterCodes { get; set; }

        public RegexNodeCharacterRange(char rangeStart, char rangeEnd, bool isNegative)
        {
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
            IsNegative = isNegative;
        }

        public override string ToRegexPattern()
        {
            string rangePattern;
            if (UseCharacterCodes)
            {
                rangePattern = string.Format(CultureInfo.InvariantCulture, "\\u{0:X4}-\\u{1:X4}", (int)RangeStart, (int)RangeEnd);
            }
            else
            {
                rangePattern = RangeStart + "-" + RangeEnd;
            }

            string result = string.Format(CultureInfo.InvariantCulture, (IsNegative ? "[^{0}]" : "[{0}]"), rangePattern);
            if (HasQuantifier)
            {
                result += Quantifier.ToRegexPattern();
            }

            return result;
        }
    }
}
