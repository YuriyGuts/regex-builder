using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeInlineOption : RegexNode
    {
        private RegexOptions options;
        private RegexNode innerExpression;

        protected override bool AllowQuantifier
        {
            get { return false; }
        }

        public RegexOptions Options
        {
            get { return options; }
            set
            {
                string invalidOptionString = null;
                if (value == RegexOptions.None)
                {
                    invalidOptionString = "None";
                }
                if ((value & RegexOptions.Compiled) == RegexOptions.Compiled)
                {
                    invalidOptionString = "Compiled";
                }
                if ((value & RegexOptions.RightToLeft) == RegexOptions.RightToLeft)
                {
                    invalidOptionString = "RightToLeft";
                }
                if ((value & RegexOptions.ECMAScript) == RegexOptions.ECMAScript)
                {
                    invalidOptionString = "ECMAScript";
                }
                if ((value & RegexOptions.CultureInvariant) == RegexOptions.CultureInvariant)
                {
                    invalidOptionString = "CultureInvariant";
                }
                
                if (invalidOptionString != null)
                {
                    throw new ArgumentException(invalidOptionString + " option is not available in inline mode");
                }
                options = value;
            }
        }

        public RegexNode InnerExpression
        {
            get { return innerExpression; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "InnerExpression cannot be null");
                }
                innerExpression = value;
            }
        }

        public RegexNodeInlineOption(RegexOptions options, RegexNode innerExpression)
        {
            Options = options;
            InnerExpression = innerExpression;
        }

        public override string ToRegexPattern()
        {
            string result = string.Format
                (
                    CultureInfo.InvariantCulture,
                    "(?{0}{1}{2}{3}{4}:{5})",
                    ((Options & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase) ? "i" : null,
                    ((Options & RegexOptions.Multiline) == RegexOptions.Multiline) ? "m" : null,
                    ((Options & RegexOptions.Singleline) == RegexOptions.Singleline) ? "s" : null,
                    ((Options & RegexOptions.ExplicitCapture) == RegexOptions.ExplicitCapture) ? "n" : null,
                    ((Options & RegexOptions.IgnorePatternWhitespace) == RegexOptions.IgnorePatternWhitespace) ? "x" : null,
                    InnerExpression.ToRegexPattern()
                );
            return result;
        }
    }
}
