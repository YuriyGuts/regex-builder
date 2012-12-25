using System;
using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeEscapingLiteral : RegexNodeLiteral
    {
        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        private string GetEscapedValue()
        {
            return RegexStringEscaper.Escape(Value, true);
        }

        public RegexNodeEscapingLiteral(string value) : base(value)
        {
        }

        public override string ToRegexPattern()
        {
            string result = GetEscapedValue();
            if (HasQuantifier)
            {
                bool shouldNotRenderGroup = (result.Length == 1) || (result.Length == 2 && result.StartsWith("\\", StringComparison.Ordinal));
                result = string.Format(CultureInfo.InvariantCulture, shouldNotRenderGroup ? "{0}{1}" : "(?:{0}){1}", result, Quantifier.ToRegexPattern());
            }
            return result;
        }
    }
}
