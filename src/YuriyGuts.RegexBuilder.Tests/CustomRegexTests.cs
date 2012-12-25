using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YuriyGuts.RegexBuilder.Tests
{
    [TestClass]
    public class CustomRegexTests
    {
        [TestMethod]
        public void TestHrefParser()
        {
            const string quotationMark = "\"";

            // Regex value taken from MSDN: http://msdn.microsoft.com/en-us/library/t9e807fx.aspx
            Regex hrefRegex = RegexBuilder.Build
            (
                RegexOptions.IgnoreCase,

                RegexBuilder.Literal("href"),
                RegexBuilder.MetaCharacter(RegexMetaChars.WhiteSpace, RegexQuantifier.ZeroOrMore),
                RegexBuilder.Literal("="),
                RegexBuilder.MetaCharacter(RegexMetaChars.WhiteSpace, RegexQuantifier.ZeroOrMore),
                RegexBuilder.Alternate
                (
                    RegexBuilder.Concatenate
                    (
                        RegexBuilder.NonEscapedLiteral(quotationMark),
                        RegexBuilder.Group
                        (
                            "Target",
                            RegexBuilder.NegativeCharacterSet(quotationMark, RegexQuantifier.ZeroOrMore)
                        ),
                        RegexBuilder.NonEscapedLiteral(quotationMark)
                    ),
                    RegexBuilder.Group
                    (
                        "Target",
                        RegexBuilder.MetaCharacter(RegexMetaChars.NonwhiteSpace, RegexQuantifier.OneOrMore)
                    )
                )
            );

            Match match = hrefRegex.Match("My favorite web sites include:</p>"
                + "<a href=\"http://msdn2.microsoft.com\">"
                + "MSDN Home Page</A></P>"
                + "<a href=\"http://www.microsoft.com\">"
                + "Microsoft Corporation Home Page</a></p>"
                + "<a href=\"http://blogs.msdn.com/bclteam\">"
                + ".NET Base Class Library blog</a></p>)");

            List<string> capturedValues = new List<string>();
            while (match.Success)
            {
                capturedValues.Add(match.Groups["Target"].Value);
                match = match.NextMatch();
            }

            Assert.AreEqual("http://msdn2.microsoft.com", capturedValues[0]);
            Assert.AreEqual("http://www.microsoft.com", capturedValues[1]);
            Assert.AreEqual("http://blogs.msdn.com/bclteam", capturedValues[2]);
        }

        [TestMethod]
        public void TestServerURLParser()
        {
            Regex serverUrlRegex = RegexBuilder.Build
            (
                RegexBuilder.Concatenate
                (
                    RegexBuilder.Group
                    (
                        "Protocol",
                        RegexBuilder.CharacterSet(RegexMetaChars.WordCharacter + '.' + '-', RegexQuantifier.OneOrMore)
                    ),
                    RegexBuilder.Literal("://"),
                    RegexQuantifier.ZeroOrOne
                ),
                RegexBuilder.Group
                (
                    "Host",
                    RegexBuilder.NegativeCharacterSet("/:", RegexQuantifier.OneOrMore)
                ),
                RegexBuilder.Concatenate
                (
                    RegexBuilder.Literal(":", RegexQuantifier.ZeroOrOne),
                    RegexBuilder.Group
                    (
                        "Port",
                        RegexBuilder.MetaCharacter(RegexMetaChars.Digit, RegexQuantifier.OneOrMore)
                    ),
                    RegexQuantifier.ZeroOrOne
                )
            );

            Assert.IsTrue(serverUrlRegex.IsMatch("net.tcp://srv1.mycompany.com.ua:8080/"));
            Assert.IsTrue(serverUrlRegex.IsMatch("http://srv1.mycompany.com.ua/"));
            Assert.IsTrue(serverUrlRegex.IsMatch("srv1.mycompany.com.ua:9876"));

            Match match1 = serverUrlRegex.Match("net.tcp://srv1.mycompany.com.ua:8080/");
            Assert.AreEqual("net.tcp", match1.Groups["Protocol"].Value);
            Assert.AreEqual("srv1.mycompany.com.ua", match1.Groups["Host"].Value);
            Assert.AreEqual("8080", match1.Groups["Port"].Value);

            Match match2 = serverUrlRegex.Match("ftp://filestore-international.company.com/");
            Assert.AreEqual("ftp", match2.Groups["Protocol"].Value);
            Assert.AreEqual("filestore-international.company.com", match2.Groups["Host"].Value);
            Assert.AreEqual(string.Empty, match2.Groups["Port"].Value);

            Match match3 = serverUrlRegex.Match("computer.company-domain.local");
            Assert.AreEqual(string.Empty, match3.Groups["Protocol"].Value);
            Assert.AreEqual("computer.company-domain.local", match3.Groups["Host"].Value);
            Assert.AreEqual(string.Empty, match3.Groups["Port"].Value);
        }

        [TestMethod]
        public void TestEmailValidator()
        {
            const string quotationMark = "\"";
            const string alphaCharacters = "a-zA-Z";
            const string alphaNumericCharacters = "0-9a-zA-Z";
            const string allowedLoginSymbols = "-!#$%&'*+=?^`{}|~";

            // Regex value taken from MSDN: http://msdn.microsoft.com/en-us/library/01escwtf.aspx
            Regex emailRegex = RegexBuilder.Build
            (
                RegexBuilder.MetaCharacter(RegexMetaChars.LineStart),
                // Match everything before @
                RegexBuilder.ConditionalMatch
                (
                    // If the string starts with a quotation mark...
                    RegexBuilder.Literal(quotationMark),
                    // ...then match the quoted text and the @ character...
                    RegexBuilder.Concatenate
                    (
                        RegexBuilder.Literal(quotationMark),
                        RegexBuilder.MetaCharacter(RegexMetaChars.AnyCharacter, RegexQuantifier.OneOrMoreLazy),
                        RegexBuilder.Literal(quotationMark),
                        RegexBuilder.Literal("@")
                    ),
                    // ...otherwise, match a sequence of alphanumeric characters and symbols, followed by @
                    RegexBuilder.Concatenate
                    (
                        RegexBuilder.CharacterSet(alphaNumericCharacters, RegexQuantifier.None),
                        RegexBuilder.Alternate
                        (
                            // The sequence can either contain only one dot...
                            RegexBuilder.NegativeLookAhead
                            (
                                RegexBuilder.Literal("."),
                                RegexBuilder.Literal(".")
                            ),
                            // ...or contain some special symbols (-!#$%&'*+=?^`{}|~)
                            RegexBuilder.CharacterSet(allowedLoginSymbols + RegexMetaChars.WordCharacter, RegexQuantifier.ZeroOrMore),

                            RegexQuantifier.ZeroOrMore
                        ),
                        // Before matching the @ character, make sure that it is preceded by alphanumeric characters.
                        RegexBuilder.PositiveLookBehind
                        (
                            RegexBuilder.CharacterSet(alphaNumericCharacters, RegexQuantifier.None),
                            RegexBuilder.Literal("@")
                        )
                    )
                ),
                // Match everything after @
                RegexBuilder.ConditionalMatch
                (
                    // Domain can be either an IP address enclosed in square brackets...
                    RegexBuilder.Literal("["),
                    // ...(IP address should look like [aaa.bbb.ccc.ddd])...
                    RegexBuilder.Concatenate
                    (
                        RegexBuilder.Literal("["),
                        RegexBuilder.Concatenate
                        (
                            RegexBuilder.MetaCharacter(RegexMetaChars.Digit, RegexQuantifier.Custom(1, 3, false)),
                            RegexBuilder.Literal("."),
                            RegexQuantifier.Exactly(3)
                        ),
                        RegexBuilder.MetaCharacter(RegexMetaChars.Digit, RegexQuantifier.Custom(1, 3, false)),
                        RegexBuilder.Literal("]")
                    ),
                    // ...or it can be a hostname.
                    RegexBuilder.Concatenate
                    (
                        RegexBuilder.Concatenate
                        (
                            RegexBuilder.CharacterSet(alphaNumericCharacters, RegexQuantifier.None),
                            RegexBuilder.CharacterSet("-" + RegexMetaChars.WordCharacter, RegexQuantifier.ZeroOrMore),
                            RegexBuilder.CharacterSet(alphaNumericCharacters, RegexQuantifier.None),
                            RegexBuilder.Literal("."),
                            RegexQuantifier.OneOrMore
                        ),
                        RegexBuilder.CharacterSet(alphaCharacters, RegexQuantifier.Custom(2, 6, false))
                    )
                ),
                RegexBuilder.MetaCharacter(RegexMetaChars.LineEnd)
            );

            Assert.IsTrue(emailRegex.IsMatch("david.jones@proseware.com"));
            Assert.IsTrue(emailRegex.IsMatch("d.j@server1.proseware.com"));
            Assert.IsTrue(emailRegex.IsMatch("jones@ms1.proseware.com"));
            Assert.IsFalse(emailRegex.IsMatch("j.@server1.proseware.com"));
            Assert.IsFalse(emailRegex.IsMatch("j@proseware.com9"));
            Assert.IsTrue(emailRegex.IsMatch("js#internal@proseware.com"));
            Assert.IsTrue(emailRegex.IsMatch("j_9@[129.126.118.1]"));
            Assert.IsFalse(emailRegex.IsMatch("j..s@proseware.com"));
            Assert.IsFalse(emailRegex.IsMatch("js*@proseware.com"));
            Assert.IsFalse(emailRegex.IsMatch("js@proseware..com"));
            Assert.IsFalse(emailRegex.IsMatch("js@proseware.com9"));
            Assert.IsTrue(emailRegex.IsMatch("j.s@server1.proseware.com"));
        }
    }
}
