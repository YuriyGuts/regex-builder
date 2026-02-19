using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YuriyGuts.RegexBuilder.Tests
{
    [TestClass]
    public class ExtensionMethodTests
    {
        [TestMethod]
        public void TestReplaceMany1()
        {
            StringBuilder stringBuilder = new StringBuilder("abc");
            stringBuilder.ReplaceMany(new[] { "a", "c" }, new[] { "x", "cz" });
            Assert.AreEqual("xbcz", stringBuilder.ToString());
        }

        [TestMethod]
        public void TestReplaceMany2()
        {
            StringBuilder stringBuilder = new StringBuilder("abc");
            stringBuilder.ReplaceMany(new[] { "x", "y" }, new[] { "X", "Y" });
            Assert.AreEqual("abc", stringBuilder.ToString());
        }

        [TestMethod]
        public void TestReplaceManyWithNullExtensionObject()
        {
            StringBuilder builder = null;
            Assert.ThrowsExactly<ArgumentNullException>(() => builder.ReplaceMany(new[] { "a" }, new[] { "b" }));
        }

        [TestMethod]
        public void TestReplaceManyWithNullArguments1()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new StringBuilder("abc").ReplaceMany(null, null));
        }

        [TestMethod]
        public void TestReplaceManyWithNullArguments2()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new StringBuilder("abc").ReplaceMany(new[] { "a", "b" }, null));
        }

        [TestMethod]
        public void TestReplaceManyWithNullArguments3()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new StringBuilder("abc").ReplaceMany(null, new[] { "a", "b" }));
        }

        [TestMethod]
        public void TestReplaceManyWithDifferentArgumentLength()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new StringBuilder("abc").ReplaceMany(new[] { "a", "b" }, new[] { "x" }));
        }
    }
}
