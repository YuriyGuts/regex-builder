using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeGroupReference : RegexNode
    {
        public int? GroupIndex { get; set; }
        public string GroupName { get; set; }

        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public RegexNodeGroupReference(int? groupIndex)
        {
            GroupIndex = groupIndex;
        }

        public RegexNodeGroupReference(string groupName)
        {
            GroupName = groupName;
        }

        public override string ToRegexPattern()
        {
            string result;
            if (GroupIndex.HasValue)
            {
                result = "\\" + GroupIndex;
            }
            else
            {
                result = string.Format(CultureInfo.InvariantCulture, "\\k<{0}>", GroupName);
            }

            if (HasQuantifier)
            {
                result += Quantifier.ToRegexPattern();
            }

            return result;
        }
    }
}
