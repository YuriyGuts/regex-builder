using System;
using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeConditionalMatch : RegexNode
    {
        private RegexNode trueMatchExpression;
        private RegexNode falseMatchExpression;

        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public string ConditionGroupName { get; set; }
        public RegexNode ConditionExpression { get; set; }

        public RegexNode TrueMatchExpression
        {
            get { return trueMatchExpression; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "TrueMatchExpression cannot be null");
                }
                trueMatchExpression = value;
            }
        }

        public RegexNode FalseMatchExpression
        {
            get { return falseMatchExpression; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "FalseMatchExpression cannot be null");
                }
                falseMatchExpression = value;
            }
        }

        public RegexNodeConditionalMatch(RegexNode conditionExpression, RegexNode trueMatchExpression, RegexNode falseMatchExpression)
        {
            ConditionExpression = conditionExpression;
            TrueMatchExpression = trueMatchExpression;
            FalseMatchExpression = falseMatchExpression;
        }

        public RegexNodeConditionalMatch(string conditionGroupName, RegexNode trueMatchExpression, RegexNode falseMatchExpression)
        {
            ConditionGroupName = conditionGroupName;
            TrueMatchExpression = trueMatchExpression;
            FalseMatchExpression = falseMatchExpression;
        }

        public override string ToRegexPattern()
        {
            string result = string.Format
            (
                CultureInfo.InvariantCulture,
                "(?({0}){1}|{2})",
                string.IsNullOrEmpty(ConditionGroupName) ? ConditionExpression.ToRegexPattern() : ConditionGroupName,
                TrueMatchExpression.ToRegexPattern(),
                FalseMatchExpression.ToRegexPattern()
            );

            if (HasQuantifier)
            {
                result += Quantifier.ToRegexPattern();
            }

            return result;
        }
    }
}
