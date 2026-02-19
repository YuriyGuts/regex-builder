using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YuriyGuts.RegexBuilder.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RegexQuantifierTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestNonePropertyShouldReturnNull()
        {
            RegexQuantifier quantifier = RegexQuantifier.None;
            Assert.IsNull(quantifier);
        }

        [TestMethod]
        public void TestZeroOrOnePropertyShouldReturnProperObject()
        {
            RegexQuantifier greedyQuantifier = RegexQuantifier.ZeroOrOne;
            Assert.AreEqual(0, greedyQuantifier.MinOccurrenceCount);
            Assert.AreEqual(1, greedyQuantifier.MaxOccurrenceCount);
            Assert.IsFalse(greedyQuantifier.IsLazy);

            RegexQuantifier lazyQuantifier = RegexQuantifier.ZeroOrOneLazy;
            Assert.AreEqual(0, lazyQuantifier.MinOccurrenceCount);
            Assert.AreEqual(1, lazyQuantifier.MaxOccurrenceCount);
            Assert.IsTrue(lazyQuantifier.IsLazy);

            Assert.AreNotSame(greedyQuantifier, lazyQuantifier);
        }

        [TestMethod]
        public void TestZeroOrMorePropertyShouldReturnProperObject()
        {
            RegexQuantifier greedyQuantifier = RegexQuantifier.ZeroOrMore;
            Assert.AreEqual(0, greedyQuantifier.MinOccurrenceCount);
            Assert.AreEqual(null, greedyQuantifier.MaxOccurrenceCount);
            Assert.IsFalse(greedyQuantifier.IsLazy);

            RegexQuantifier lazyQuantifier = RegexQuantifier.ZeroOrMoreLazy;
            Assert.AreEqual(0, lazyQuantifier.MinOccurrenceCount);
            Assert.AreEqual(null, lazyQuantifier.MaxOccurrenceCount);
            Assert.IsTrue(lazyQuantifier.IsLazy);

            Assert.AreNotSame(greedyQuantifier, lazyQuantifier);
        }

        [TestMethod]
        public void TestOneOrMorePropertyShouldReturnProperObject()
        {
            RegexQuantifier greedyQuantifier = RegexQuantifier.OneOrMore;
            Assert.AreEqual(1, greedyQuantifier.MinOccurrenceCount);
            Assert.AreEqual(null, greedyQuantifier.MaxOccurrenceCount);
            Assert.IsFalse(greedyQuantifier.IsLazy);

            RegexQuantifier lazyQuantifier = RegexQuantifier.OneOrMoreLazy;
            Assert.AreEqual(1, lazyQuantifier.MinOccurrenceCount);
            Assert.AreEqual(null, lazyQuantifier.MaxOccurrenceCount);
            Assert.IsTrue(lazyQuantifier.IsLazy);

            Assert.AreNotSame(greedyQuantifier, lazyQuantifier);
        }

        [TestMethod]
        public void TestAtLeastMethodShouldReturnProperObject()
        {
            RegexQuantifier greedyQuantifier = RegexQuantifier.AtLeast(5);
            Assert.AreEqual(5, greedyQuantifier.MinOccurrenceCount);
            Assert.AreEqual(null, greedyQuantifier.MaxOccurrenceCount);
            Assert.IsFalse(greedyQuantifier.IsLazy);

            RegexQuantifier lazyQuantifier1 = RegexQuantifier.AtLeast(5, false);
            Assert.AreEqual(5, lazyQuantifier1.MinOccurrenceCount);
            Assert.AreEqual(null, lazyQuantifier1.MaxOccurrenceCount);
            Assert.IsFalse(lazyQuantifier1.IsLazy);

            RegexQuantifier lazyQuantifier2 = RegexQuantifier.AtLeast(5, true);
            Assert.AreEqual(5, lazyQuantifier2.MinOccurrenceCount);
            Assert.AreEqual(null, lazyQuantifier2.MaxOccurrenceCount);
            Assert.IsTrue(lazyQuantifier2.IsLazy);

            Assert.AreNotSame(greedyQuantifier, lazyQuantifier1);
            Assert.AreNotSame(greedyQuantifier, lazyQuantifier2);
            Assert.AreNotSame(lazyQuantifier1, lazyQuantifier2);
        }

        [TestMethod]
        public void TestExactlyMethodShouldReturnProperObject()
        {
            RegexQuantifier greedyQuantifier = RegexQuantifier.Exactly(5);
            Assert.AreEqual(5, greedyQuantifier.MinOccurrenceCount);
            Assert.AreEqual(5, greedyQuantifier.MaxOccurrenceCount);
            Assert.IsFalse(greedyQuantifier.IsLazy);

            RegexQuantifier lazyQuantifier1 = RegexQuantifier.Exactly(3, true);
            Assert.AreEqual(3, lazyQuantifier1.MinOccurrenceCount);
            Assert.AreEqual(3, lazyQuantifier1.MaxOccurrenceCount);
            Assert.IsTrue(lazyQuantifier1.IsLazy);

            RegexQuantifier lazyQuantifier2 = RegexQuantifier.Exactly(2, true);
            Assert.AreEqual(2, lazyQuantifier2.MinOccurrenceCount);
            Assert.AreEqual(2, lazyQuantifier2.MaxOccurrenceCount);
            Assert.IsTrue(lazyQuantifier2.IsLazy);

            Assert.AreNotSame(greedyQuantifier, lazyQuantifier1);
            Assert.AreNotSame(greedyQuantifier, lazyQuantifier2);
            Assert.AreNotSame(lazyQuantifier1, lazyQuantifier2);
        }

        [TestMethod]
        public void TestCustomMethodShouldReturnProperObject()
        {
            RegexQuantifier greedyQuantifier = RegexQuantifier.Custom(1, 9, false);
            Assert.AreEqual(1, greedyQuantifier.MinOccurrenceCount);
            Assert.AreEqual(9, greedyQuantifier.MaxOccurrenceCount);
            Assert.IsFalse(greedyQuantifier.IsLazy);

            RegexQuantifier lazyQuantifier1 = RegexQuantifier.Custom(4, 3, true);
            Assert.AreEqual(4, lazyQuantifier1.MinOccurrenceCount);
            Assert.AreEqual(3, lazyQuantifier1.MaxOccurrenceCount);
            Assert.IsTrue(lazyQuantifier1.IsLazy);

            RegexQuantifier lazyQuantifier2 = RegexQuantifier.Custom(2, null, true);
            Assert.AreEqual(2, lazyQuantifier2.MinOccurrenceCount);
            Assert.AreEqual(null, lazyQuantifier2.MaxOccurrenceCount);
            Assert.IsTrue(lazyQuantifier2.IsLazy);

            Assert.AreNotSame(greedyQuantifier, lazyQuantifier1);
            Assert.AreNotSame(greedyQuantifier, lazyQuantifier2);
            Assert.AreNotSame(lazyQuantifier1, lazyQuantifier2);
        }

        [TestMethod]
        public void TestMinOccurrenceCountPropertyShouldNotAcceptNulls1()
        {
            Assert.ThrowsExactly<ArgumentException>(() => RegexQuantifier.Custom(null, 2, true));
        }

        [TestMethod]
        public void TestMinOccurrenceCountPropertyShouldNotAcceptNulls2()
        {
            RegexQuantifier quantifier = RegexQuantifier.Custom(1, 2, true);
            Assert.ThrowsExactly<ArgumentException>(() => quantifier.MinOccurrenceCount = null);
        }

        [TestMethod]
        public void TestMinMaxOccurrenceCountPropertiesShouldNotAcceptNegativeInts1()
        {
            Assert.ThrowsExactly<ArgumentException>(() => RegexQuantifier.Custom(-1, 5, true));
        }

        [TestMethod]
        public void TestMinMaxOccurrenceCountPropertiesShouldNotAcceptNegativeInts2()
        {
            Assert.ThrowsExactly<ArgumentException>(() => RegexQuantifier.Custom(0, -1, true));
        }

        [TestMethod]
        public void TestMinMaxOccurrenceCountPropertiesShouldNotAcceptNegativeInts3()
        {
            RegexQuantifier quantifier = RegexQuantifier.Custom(1, 2, true);
            Assert.ThrowsExactly<ArgumentException>(() => quantifier.MinOccurrenceCount = -1);
        }

        [TestMethod]
        public void TestMinMaxOccurrenceCountPropertiesShouldNotAcceptNegativeInts4()
        {
            RegexQuantifier quantifier = RegexQuantifier.Custom(1, 2, true);
            Assert.ThrowsExactly<ArgumentException>(() => quantifier.MaxOccurrenceCount = -1);
        }
    }
}
