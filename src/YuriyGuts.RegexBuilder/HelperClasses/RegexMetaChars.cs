namespace YuriyGuts.RegexBuilder
{
    public static class RegexMetaChars
    {
        public const string AnyCharacter = ".";
        public const string LineStart = "^";
        public const string LineEnd = "$";
        public const string StringStart = "\\A";
        public const string WordBoundary = "\\b";
        public const string NonWordBoundary = "\\B";
        public const string Digit = "\\d";
        public const string NonDigit = "\\D";
        public const string Escape = "\\e";
        public const string FormFeed = "\\f";
        public const string ConsecutiveMatch = "\\G";
        public const string NewLine = "\\n";
        public const string CarriageReturn = "\\r";
        public const string WhiteSpace = "\\s";
        public const string NonwhiteSpace = "\\S";
        public const string Tab = "\\t";
        public const string VerticalTab = "\\v";
        public const string WordCharacter = "\\w";
        public const string NonWordCharacter = "\\W";
        public const string StringEnd = "\\Z";
    }
}
