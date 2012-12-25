using System;
using System.Text.RegularExpressions;

namespace YuriyGuts.RegexBuilder.Tests
{
    internal class Program
    {
        public static void Main()
        {
            Regex helloWorldRegex = RegexBuilder.Build
            (
                RegexBuilder.Literal("Hello"),
                RegexBuilder.MetaCharacter(RegexMetaChars.WhiteSpace, RegexQuantifier.OneOrMore),
                RegexBuilder.Group
                (
                    "Target",
                    RegexBuilder.Alternate
                    (
                        RegexBuilder.Literal("world"),
                        RegexBuilder.Literal("universe")
                    )
                ),
                RegexBuilder.Literal("!", RegexQuantifier.ZeroOrOne)
            );

            MatchAndPrintResults(helloWorldRegex, "Hello world!");
            MatchAndPrintResults(helloWorldRegex, "Hello universe!");
            Console.ReadKey(true);
        }

        private static void MatchAndPrintResults(Regex regex, string input)
        {
            Console.WriteLine("Regex: " + regex);
            Console.WriteLine("Input: " + input);

            MatchCollection matches = regex.Matches(input);
            Console.WriteLine("Match Count: " + matches.Count);

            int matchCounter = 0;
            foreach (Match match in matches)
            {
                Console.WriteLine(string.Empty.PadRight(50, '='));
                Console.WriteLine("  Match {0}:", ++matchCounter);
                Console.WriteLine("  Value: " + match.Value);
                
                Console.WriteLine(string.Empty.PadRight(50, '-'));
                Console.WriteLine("    Groups:");
                Console.WriteLine(string.Empty.PadRight(50, '-'));
                
                foreach (string name in regex.GetGroupNames())
                {
                    Console.WriteLine("    \"{0}\": {1}", name, match.Groups[name].Value);
                }
                Console.WriteLine(string.Empty.PadRight(50, '='));
            }

            Console.WriteLine();
        }
    }
}
