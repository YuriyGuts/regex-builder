using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeConcatenation : RegexNode
    {
        private IList<RegexNode> childNodes;

        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public IList<RegexNode> ChildNodes
        {
            get { return childNodes; }
        }

        public RegexNodeConcatenation()
        {
            childNodes = new List<RegexNode>();
        }

        public RegexNodeConcatenation(IList<RegexNode> childNodes)
        {
            if (childNodes == null)
            {
                throw new ArgumentNullException("childNodes", "Child node collection cannot be null.");
            }
            this.childNodes = childNodes;
        }

        public RegexNodeConcatenation(params RegexNode[] childNodes)
        {
            this.childNodes = new List<RegexNode>(childNodes);
        }

        public override string ToRegexPattern()
        {
            StringBuilder resultBuilder = new StringBuilder();
            foreach (RegexNode node in ChildNodes)
            {
                resultBuilder.Append(node.ToRegexPattern());
            }

            string result;
            if (HasQuantifier)
            {
                result = string.Format(CultureInfo.InvariantCulture, "(?:{0}){1}", resultBuilder, Quantifier.ToRegexPattern());
            }
            else
            {
                result = resultBuilder.ToString();
            }

            return result;
        }
    }
}
