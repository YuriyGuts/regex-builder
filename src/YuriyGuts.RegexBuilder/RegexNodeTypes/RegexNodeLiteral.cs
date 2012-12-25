using System;
using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeLiteral : RegexNode
    {
        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public string Value { get; set; }

        public RegexNodeLiteral(string value)
        {
            Value = value;
        }

        public override string ToRegexPattern()
        {
            string result = Value;
            if (HasQuantifier)
            {
                bool shouldNotRenderGroup = (Value.Length == 1) || (Value.Length == 2 && Value.StartsWith("\\", StringComparison.Ordinal));
                result = string.Format(CultureInfo.InvariantCulture, shouldNotRenderGroup ? "{0}{1}" : "(?:{0}){1}", result, Quantifier.ToRegexPattern());
            }
            return result;
        }
    }
}
