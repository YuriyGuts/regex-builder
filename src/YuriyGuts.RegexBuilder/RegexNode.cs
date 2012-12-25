using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace YuriyGuts.RegexBuilder
{
    public abstract class RegexNode
    {
        private RegexQuantifier quantifier;

        protected bool HasQuantifier
        {
            get { return quantifier != null; }
        }

        protected abstract bool AllowQuantifier { get; }

        /// <summary>
        /// Gets or sets a RegexQuantifier associated with this node.
        /// </summary>
        public RegexQuantifier Quantifier
        {
            get
            {
                return quantifier;
            }
            set
            {
                if (!AllowQuantifier)
                {
                    throw new InvalidOperationException("This node type does not allow quantifiers.");
                }
                quantifier = value;
            }
        }

        /// <summary>
        /// Converts RegexNode to a Regex pattern string.
        /// </summary>
        public abstract string ToRegexPattern();

        // The default equality operator works as expected, no need to override it in this case.
        [SuppressMessage("Microsoft.Design", "CA1013:OverloadOperatorEqualsOnOverloadingAddAndSubtract", Scope = "Member")]
        public static RegexNode operator +(RegexNode node1, RegexNode node2)
        {
            return Add(node1, node2);
        }

        public static RegexNode Add(RegexNode node1, RegexNode node2)
        {
            if (node1 == null || node2 == null)
            {
                throw new ArgumentException("Both nodes must be not null.");
            }

            RegexNodeConcatenation node1AsConcatenation = node1 as RegexNodeConcatenation;
            RegexNodeConcatenation node2AsConcatenation = node2 as RegexNodeConcatenation;

            if (node1AsConcatenation != null && node2AsConcatenation != null)
            {
                List<RegexNode> newChildNodes = new List<RegexNode>();
                newChildNodes.AddRange(node1AsConcatenation.ChildNodes);
                newChildNodes.AddRange(node2AsConcatenation.ChildNodes);

                RegexNodeConcatenation result = new RegexNodeConcatenation(newChildNodes);
                return result;
            }

            if (node1AsConcatenation != null)
            {
                List<RegexNode> newChildNodes = new List<RegexNode>(node1AsConcatenation.ChildNodes);
                newChildNodes.Add(node2);

                RegexNodeConcatenation result = new RegexNodeConcatenation(newChildNodes);
                return result;
            }

            if (node2AsConcatenation != null)
            {
                List<RegexNode> newChildNodes = new List<RegexNode>();
                newChildNodes.Add(node1);
                newChildNodes.AddRange(node2AsConcatenation.ChildNodes);

                RegexNodeConcatenation result = new RegexNodeConcatenation(newChildNodes);
                return result;
            }

            return new RegexNodeConcatenation(node1, node2);
        }
    }
}
