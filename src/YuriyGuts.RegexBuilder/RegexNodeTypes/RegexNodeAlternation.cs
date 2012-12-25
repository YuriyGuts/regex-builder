using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeAlternation : RegexNode
    {
        private RegexNode[] expressions;

        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public IEnumerable<RegexNode> Expressions
        {
            get { return expressions; }
        }

        public RegexNodeAlternation(RegexNode expression1, RegexNode expression2)
        {
            if (expression1 == null || expression2 == null)
            {
                throw new ArgumentNullException(string.Empty, "Expressions cannot be null");
            }
            expressions = new[] { expression1, expression2 };
        }

        public RegexNodeAlternation(params RegexNode[] expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException("expressions", "Expression list cannot be null.");
            }
            for (int i = 0; i < expressions.Length; i++)
            {
                if (expressions[i] == null)
                {
                    throw new ArgumentNullException("expressions", "All alternating expressions must be not null.");
                }
            }
            this.expressions = expressions;
        }

        public override string ToRegexPattern()
        {
            string result;

            if (expressions.Length == 2)
            {
                result = string.Format(CultureInfo.InvariantCulture, "(?:{0}|{1})", expressions[0].ToRegexPattern(), expressions[1].ToRegexPattern());
            }
            else
            {
                StringBuilder resultBuilder = new StringBuilder();
                resultBuilder.Append("(?:");
                for (int i = 0; i < expressions.Length; i++)
                {
                    resultBuilder.Append(expressions[i].ToRegexPattern());
                    if (i < expressions.Length - 1)
                    {
                        resultBuilder.Append("|");
                    }
                }
                resultBuilder.Append(")");
                result = resultBuilder.ToString();
            }

            if (HasQuantifier)
            {
                result += Quantifier.ToRegexPattern();
            }

            return result;
        }
    }
}
