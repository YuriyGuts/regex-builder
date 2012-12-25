using System;
using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public enum RegexLookAround
    {
        PositiveLookAhead,
        PositiveLookBehind,
        NegativeLookAhead,
        NegativeLookBehind,
    }

    public class RegexNodeLookAround : RegexNode
    {
        private RegexNode lookAroundExpression;
        private RegexNode matchExpression;

        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public RegexLookAround LookAroundType { get; set; }

        public RegexNode LookAroundExpression
        {
            get { return lookAroundExpression; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "LookAroundExpression cannot be null");
                }
                lookAroundExpression = value;
            }
        }

        public RegexNode MatchExpression
        {
            get { return matchExpression; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "MatchExpression cannot be null");
                }
                matchExpression = value;
            }
        }

        public RegexNodeLookAround(RegexLookAround lookAroundType, RegexNode lookAroundExpression, RegexNode matchExpression)
        {
            LookAroundType = lookAroundType;
            LookAroundExpression = lookAroundExpression;
            MatchExpression = matchExpression;
        }

        public override string ToRegexPattern()
        {
            string format = string.Empty;
            switch (LookAroundType)
            {
                case RegexLookAround.PositiveLookAhead:
                    format = "(?:{1}(?={0}))";
                    break;
                case RegexLookAround.PositiveLookBehind:
                    format = "(?:(?<={0}){1})";
                    break;
                case RegexLookAround.NegativeLookAhead:
                    format = "(?:{1}(?!{0}))";
                    break;
                case RegexLookAround.NegativeLookBehind:
                    format = "(?:(?<!{0}){1})";
                    break;
            }

            string result = string.Format(CultureInfo.InvariantCulture, format, LookAroundExpression.ToRegexPattern(), MatchExpression.ToRegexPattern());
            if (HasQuantifier)
            {
                result += Quantifier.ToRegexPattern();
            }

            return result;
        }
    }
}
