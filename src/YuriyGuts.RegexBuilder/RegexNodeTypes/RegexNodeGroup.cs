using System;
using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeGroup : RegexNode
    {
        private RegexNode innerExpression;

        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public bool IsCapturing { get; set; }
        public string Name { get; set; }

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

        public RegexNodeGroup(RegexNode innerExpression)
        {
            InnerExpression = innerExpression;
            IsCapturing = true;
        }

        public RegexNodeGroup(RegexNode innerExpression, bool isCapturing)
        {
            InnerExpression = innerExpression;
            IsCapturing = isCapturing;
        }

        public RegexNodeGroup(RegexNode innerExpression, string name)
        {
            InnerExpression = innerExpression;
            IsCapturing = true;
            Name = name;
        }

        public override string ToRegexPattern()
        {
            string result;
            if (!IsCapturing)
            {
                result = string.Format(CultureInfo.InvariantCulture, "(?:{0})", InnerExpression.ToRegexPattern());
            }
            else
            {
                if (string.IsNullOrEmpty(Name))
                {
                    result = string.Format(CultureInfo.InvariantCulture, "({0})", InnerExpression.ToRegexPattern());
                }
                else
                {
                    result = string.Format(CultureInfo.InvariantCulture, "(?<{0}>{1})", Name, InnerExpression.ToRegexPattern());
                }
            }

            if (HasQuantifier)
            {
                result += Quantifier.ToRegexPattern();
            }

            return result;
        }
    }
}
