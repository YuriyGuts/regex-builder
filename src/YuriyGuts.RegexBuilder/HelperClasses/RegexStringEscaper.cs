using System.Text;

namespace YuriyGuts.RegexBuilder
{
    public static class RegexStringEscaper
    {
        public static string Escape(string value, bool escapeBackslash)
        {
            StringBuilder resultBuilder = new StringBuilder(value);
            if (escapeBackslash)
            {
                resultBuilder.Replace("\\", "\\\\");
            }

            string[] oldValues =
                { "^",   "$",   ".",   "|",   "?",   "*",   "+",   "(",   ")",   "[",   "]",   "{",   "}"   };
            string[] newValues =
                { "\\^", "\\$", "\\.", "\\|", "\\?", "\\*", "\\+", "\\(", "\\)", "\\[", "\\]", "\\{", "\\}" };

            resultBuilder.ReplaceMany(oldValues, newValues);
            return resultBuilder.ToString();
        }
    }
}
