using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YuriyGuts.RegexBuilder.Tests
{
    [TestClass]
    public class RegexQuantifierRenderingTests
    {
        [TestMethod]
        public void TestZeroOrMoreRendering()
        {
            RegexQuantifier quantifier1 = RegexQuantifier.ZeroOrMore;
            Assert.AreEqual("*", quantifier1.ToRegexPattern());
            quantifier1.IsLazy = true;
            Assert.AreEqual("*?", quantifier1.ToRegexPattern());

            RegexQuantifier quantifier2 = RegexQuantifier.AtLeast(0);
            Assert.AreEqual("*", quantifier2.ToRegexPattern());
            quantifier2.IsLazy = true;
            Assert.AreEqual("*?", quantifier2.ToRegexPattern());

            RegexQuantifier quantifier3 = RegexQuantifier.Custom(0, null, false);
            Assert.AreEqual("*", quantifier3.ToRegexPattern());
            quantifier3.IsLazy = true;
            Assert.AreEqual("*?", quantifier3.ToRegexPattern());
        }

        [TestMethod]
        public void TestOneOrMoreRendering()
        {
            RegexQuantifier quantifier1 = RegexQuantifier.OneOrMore;
            Assert.AreEqual("+", quantifier1.ToRegexPattern());
            quantifier1.IsLazy = true;
            Assert.AreEqual("+?", quantifier1.ToRegexPattern());

            RegexQuantifier quantifier2 = RegexQuantifier.AtLeast(1);
            Assert.AreEqual("+", quantifier2.ToRegexPattern());
            quantifier2.IsLazy = true;
            Assert.AreEqual("+?", quantifier2.ToRegexPattern());

            RegexQuantifier quantifier3 = RegexQuantifier.Custom(1, null, false);
            Assert.AreEqual("+", quantifier3.ToRegexPattern());
            quantifier3.IsLazy = true;
            Assert.AreEqual("+?", quantifier3.ToRegexPattern());
        }

        [TestMethod]
        public void TestZeroOrOneRendering()
        {
            RegexQuantifier quantifier1 = RegexQuantifier.ZeroOrOne;
            Assert.AreEqual("?", quantifier1.ToRegexPattern());
            quantifier1.IsLazy = true;
            Assert.AreEqual("??", quantifier1.ToRegexPattern());

            RegexQuantifier quantifier2 = RegexQuantifier.Custom(0, 1, false);
            Assert.AreEqual("?", quantifier2.ToRegexPattern());
            quantifier2.IsLazy = true;
            Assert.AreEqual("??", quantifier2.ToRegexPattern());
        }

        [TestMethod]
        public void TestExactlyRendering()
        {
            RegexQuantifier quantifier1 = RegexQuantifier.Exactly(5, false);
            Assert.AreEqual("{5}", quantifier1.ToRegexPattern());
            quantifier1.IsLazy = true;
            Assert.AreEqual("{5}?", quantifier1.ToRegexPattern());

            RegexQuantifier quantifier2 = RegexQuantifier.Custom(5, 5, false);
            Assert.AreEqual("{5}", quantifier2.ToRegexPattern());
            quantifier2.IsLazy = true;
            Assert.AreEqual("{5}?", quantifier2.ToRegexPattern());
        }

        [TestMethod]
        public void TestAtLeastRendering()
        {
            RegexQuantifier quantifier1 = RegexQuantifier.AtLeast(5);
            Assert.AreEqual("{5,}", quantifier1.ToRegexPattern());
            quantifier1.IsLazy = true;
            Assert.AreEqual("{5,}?", quantifier1.ToRegexPattern());

            RegexQuantifier quantifier2 = RegexQuantifier.Custom(5, null, false);
            Assert.AreEqual("{5,}", quantifier2.ToRegexPattern());
            quantifier2.IsLazy = true;
            Assert.AreEqual("{5,}?", quantifier2.ToRegexPattern());
        }

        [TestMethod]
        public void TestCustomRendering()
        {
            RegexQuantifier quantifier1 = RegexQuantifier.Custom(1, 2, false);
            Assert.AreEqual("{1,2}", quantifier1.ToRegexPattern());
            quantifier1.IsLazy = true;
            Assert.AreEqual("{1,2}?", quantifier1.ToRegexPattern());

            RegexQuantifier quantifier2 = RegexQuantifier.Custom(101, 152, false);
            Assert.AreEqual("{101,152}", quantifier2.ToRegexPattern());
            quantifier2.IsLazy = true;
            Assert.AreEqual("{101,152}?", quantifier2.ToRegexPattern());
        }
    }
}
