using System;
using System.Text;

namespace YuriyGuts.RegexBuilder
{
    internal static class ExtensionMethods
    {
        public static void ReplaceMany(this StringBuilder builder, string[] oldValues, string[] newValues)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (oldValues == null || newValues == null)
            {
                throw new ArgumentNullException
                (
                    oldValues == null ? "oldValues" : "newValues",
                    "Search and replacement arrays should both be not-null."
                );
            }

            if (oldValues.Length != newValues.Length)
            {
                throw new ArgumentException("Search and replacement arrays should have equal lengths.");
            }

            for (int i = 0; i < oldValues.Length; i++)
            {
                builder.Replace(oldValues[i], newValues[i]);
            }
        }
    }
}
