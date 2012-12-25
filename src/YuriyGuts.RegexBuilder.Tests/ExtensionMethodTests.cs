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
            Assert.AreEqual(stringBuilder.ToString(), "xbcz");
        }

        [TestMethod]
        public void TestReplaceMany2()
        {
            StringBuilder stringBuilder = new StringBuilder("abc");
            stringBuilder.ReplaceMany(new[] { "x", "y" }, new[] { "X", "Y" });
            Assert.AreEqual(stringBuilder.ToString(), "abc");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceManyWithNullExtensionObject()
        {
            StringBuilder builder = null;
            // ReSharper disable ExpressionIsAlwaysNull
            builder.ReplaceMany(new[] { "a" }, new[] { "b" });
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceManyWithNullArguments1()
        {
            new StringBuilder("abc").ReplaceMany(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceManyWithNullArguments2()
        {
            new StringBuilder("abc").ReplaceMany(new[] { "a", "b" }, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceManyWithNullArguments3()
        {
            new StringBuilder("abc").ReplaceMany(null, new[] { "a", "b" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReplaceManyWithDifferentArgumentLength()
        {
            new StringBuilder("abc").ReplaceMany(new[] { "a", "b" }, new[] { "x" });
        }
    }
}
