using System;
using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeBacktrackingSuppression : RegexNode
    {
        private RegexNode innerExpression;

        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public RegexNode InnerExpression
        {
            get { return innerExpression; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "InnerExpression cannot be null.");
                }
                innerExpression = value;
            }
        }

        public RegexNodeBacktrackingSuppression(RegexNode innerExpression)
        {
            InnerExpression = innerExpression;
        }

        public override string ToRegexPattern()
        {
            string result = string.Format(CultureInfo.InvariantCulture, "(?>{0})", InnerExpression.ToRegexPattern());
            if (HasQuantifier)
            {
                result += Quantifier.ToRegexPattern();
            }
            return result;
        }
    }
}
