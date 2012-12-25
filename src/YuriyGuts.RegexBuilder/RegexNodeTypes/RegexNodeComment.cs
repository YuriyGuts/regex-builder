using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeComment : RegexNode
    {
        protected override bool AllowQuantifier
        {
            get { return false; }
        }

        public string CommentText { get; set; }

        public RegexNodeComment(string commentText)
        {
            CommentText = commentText;
        }

        public override string ToRegexPattern()
        {
            string result = string.Format(CultureInfo.InvariantCulture, "(?#{0})", CommentText);
            return result;
        }
    }
}
