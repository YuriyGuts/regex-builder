using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YuriyGuts.RegexBuilder.Tests
{
    [TestClass]
    public class RegexBuilderTests
    {
        [TestMethod]
        public void TestAsciiCharacterRendering()
        {
            RegexNode node1 = RegexBuilder.AsciiCharacter(0x30);
            Assert.AreEqual(@"\x30", node1.ToRegexPattern());

            RegexNode node2 = RegexBuilder.AsciiCharacter(0x7F, RegexQuantifier.Custom(1, 4, true));
            Assert.AreEqual(@"(?:\x7f){1,4}?", node2.ToRegexPattern());

            RegexNode node3 = RegexBuilder.AsciiCharacter(0x0B, RegexQuantifier.Exactly(5));
            Assert.AreEqual(@"(?:\x0b){5}", node3.ToRegexPattern());
        }

        [TestMethod]
        public void TestUnicodeCharacterRendering()
        {
            RegexNode node1 = RegexBuilder.UnicodeCharacter(0x1234);
            Assert.AreEqual(@"\u1234", node1.ToRegexPattern());

            RegexNode node2 = RegexBuilder.UnicodeCharacter(0x7F03, RegexQuantifier.Custom(1, 4, true));
            Assert.AreEqual(@"(?:\u7f03){1,4}?", node2.ToRegexPattern());

            RegexNode node3 = RegexBuilder.UnicodeCharacter(0x0BA5, RegexQuantifier.Exactly(5));
            Assert.AreEqual(@"(?:\u0ba5){5}", node3.ToRegexPattern());
        }

        [TestMethod]
        public void TestMetaCharacterRendering()
        {
            RegexNode node1 = RegexBuilder.MetaCharacter(RegexMetaChars.NonWordBoundary);
            Assert.AreEqual(@"\B", node1.ToRegexPattern());

            RegexNode node2 = RegexBuilder.MetaCharacter(RegexMetaChars.Digit, RegexQuantifier.Custom(1, 4, true));
            Assert.AreEqual(@"\d{1,4}?", node2.ToRegexPattern());

            RegexNode node3 = RegexBuilder.MetaCharacter(RegexMetaChars.WhiteSpace, RegexQuantifier.Exactly(5));
            Assert.AreEqual(@"\s{5}", node3.ToRegexPattern());
        }

        [TestMethod]
        public void TestBuildMethod1()
        {
            RegexNode literal = RegexBuilder.Literal("abc");
            RegexNode characterRange = RegexBuilder.CharacterRange('a', 'f', false, RegexQuantifier.None);
            RegexNode character = RegexBuilder.MetaCharacter(RegexMetaChars.LineEnd);

            Regex regex = RegexBuilder.Build(literal, characterRange, character);
            Assert.AreEqual("abc[a-f]$", regex.ToString());
        }

        [TestMethod]
        public void TestBuildMethod2()
        {
            RegexNode literal = RegexBuilder.Literal("abc");
            RegexNode characterRange = RegexBuilder.CharacterRange('a', 'f', false, RegexQuantifier.None);
            RegexNode character = RegexBuilder.MetaCharacter(RegexMetaChars.LineEnd);
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation(literal, characterRange, character);

            Regex regex = RegexBuilder.Build(concatenation);
            Assert.AreEqual("abc[a-f]$", regex.ToString());
        }

        [TestMethod]
        public void TestBuildMethod3()
        {
            RegexNode literal = RegexBuilder.Literal("abc");
            RegexNode characterRange = RegexBuilder.CharacterRange('a', 'f', false, RegexQuantifier.None);
            RegexNode character = RegexBuilder.MetaCharacter(RegexMetaChars.LineEnd);

            Regex regex = RegexBuilder.Build(RegexOptions.Compiled, literal, characterRange, character);
            Assert.IsTrue((regex.Options & RegexOptions.Compiled) == RegexOptions.Compiled);
            Assert.AreEqual("abc[a-f]$", regex.ToString());
        }

        [TestMethod]
        public void TestBuildMethod4()
        {
            RegexNode literal = RegexBuilder.Literal("abc");
            RegexNode characterRange = RegexBuilder.CharacterRange('a', 'f', false, RegexQuantifier.None);
            RegexNode character = RegexBuilder.MetaCharacter(RegexMetaChars.LineEnd);
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation(literal, characterRange, character);

            Regex regex = RegexBuilder.Build(RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline, concatenation);
            Assert.IsTrue((regex.Options & RegexOptions.IgnorePatternWhitespace) == RegexOptions.IgnorePatternWhitespace);
            Assert.IsTrue((regex.Options & RegexOptions.Singleline) == RegexOptions.Singleline);
            Assert.AreEqual("abc[a-f]$", regex.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBuildMethodValidation1()
        {
            RegexBuilder.Build((RegexNode)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBuildMethodValidation2()
        {
            RegexBuilder.Build((RegexNode[])null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBuildMethodValidation3()
        {
            RegexBuilder.Build(RegexOptions.None, (RegexNode)null);
        }
    }
}
