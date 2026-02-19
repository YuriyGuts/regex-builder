using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YuriyGuts.RegexBuilder.Tests
{
    [TestClass]
    public class RegexNodeTests
    {
        [TestMethod]
        public void TestQuantifierSetterForNodesWithQuantifierAllowed()
        {
            RegexNode node = new RegexNodeLiteral("Value");
            RegexQuantifier quantifier = RegexQuantifier.OneOrMore;
            node.Quantifier = quantifier;
            Assert.AreEqual(quantifier, node.Quantifier);
        }

        [TestMethod]
        public void TestQuantifierSetterForNodesWithQuantifierNotAllowed()
        {
            RegexNode node = new RegexNodeComment("This is a comment");
            Assert.ThrowsExactly<InvalidOperationException>(() => node.Quantifier = RegexQuantifier.OneOrMore);
        }

        [TestMethod]
        public void TestRegexNodeAdditionOperator1()
        {
            RegexNodeLiteral literal1 = new RegexNodeLiteral("\\w*");
            RegexNodeLiteral literal2 = new RegexNodeLiteral("\\d+");

            RegexNode sum = literal1 + literal2;
            Assert.IsInstanceOfType<RegexNodeConcatenation>(sum);
            Assert.AreEqual(literal1, ((RegexNodeConcatenation)sum).ChildNodes[0]);
            Assert.AreEqual(literal2, ((RegexNodeConcatenation)sum).ChildNodes[1]);
        }

        [TestMethod]
        public void TestRegexNodeAdditionOperator2()
        {
            RegexNodeLiteral literal1 = new RegexNodeLiteral("\\w*");
            RegexNodeLiteral literal2 = new RegexNodeLiteral("\\d+");
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation(literal1, literal2);

            RegexNodeLiteral literal3 = new RegexNodeLiteral("\\s?");
            RegexNode sum = concatenation + literal3;

            Assert.IsInstanceOfType<RegexNodeConcatenation>(sum);
            Assert.AreNotSame(concatenation, sum);
            Assert.AreEqual(literal1, ((RegexNodeConcatenation)sum).ChildNodes[0]);
            Assert.AreEqual(literal2, ((RegexNodeConcatenation)sum).ChildNodes[1]);
            Assert.AreEqual(literal3, ((RegexNodeConcatenation)sum).ChildNodes[2]);
        }

        [TestMethod]
        public void TestRegexNodeAdditionOperator3()
        {
            RegexNodeLiteral literal1 = new RegexNodeLiteral("\\w*");
            RegexNodeLiteral literal2 = new RegexNodeLiteral("\\d+");
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation(literal1, literal2);

            RegexNodeLiteral literal3 = new RegexNodeLiteral("\\s?");
            RegexNode sum = literal3 + concatenation;

            Assert.IsInstanceOfType<RegexNodeConcatenation>(sum);
            Assert.AreNotSame(concatenation, sum);
            Assert.AreEqual(literal3, ((RegexNodeConcatenation)sum).ChildNodes[0]);
            Assert.AreEqual(literal1, ((RegexNodeConcatenation)sum).ChildNodes[1]);
            Assert.AreEqual(literal2, ((RegexNodeConcatenation)sum).ChildNodes[2]);
        }

        [TestMethod]
        public void TestRegexNodeAdditionOperator4()
        {
            RegexNodeLiteral literal1 = new RegexNodeLiteral("\\w*");
            RegexNodeLiteral literal2 = new RegexNodeLiteral("\\d+");
            RegexNodeConcatenation concatenation1 = new RegexNodeConcatenation(literal1, literal2);

            RegexNodeLiteral literal3 = new RegexNodeLiteral("\\W*");
            RegexNodeLiteral literal4 = new RegexNodeLiteral("\\t+");
            RegexNodeConcatenation concatenation2 = new RegexNodeConcatenation(literal3, literal4);

            RegexNode sum = concatenation1 + concatenation2;

            Assert.IsInstanceOfType<RegexNodeConcatenation>(sum);
            Assert.AreNotSame(concatenation1, sum);
            Assert.AreNotSame(concatenation2, sum);
            Assert.AreEqual(literal1, ((RegexNodeConcatenation)sum).ChildNodes[0]);
            Assert.AreEqual(literal2, ((RegexNodeConcatenation)sum).ChildNodes[1]);
            Assert.AreEqual(literal3, ((RegexNodeConcatenation)sum).ChildNodes[2]);
            Assert.AreEqual(literal4, ((RegexNodeConcatenation)sum).ChildNodes[3]);
        }

        [TestMethod]
        public void TestRegexNodeAdditionOperator5()
        {
            RegexNodeLiteral literal = new RegexNodeLiteral("\\w*");
            Assert.ThrowsExactly<ArgumentException>(() => { RegexNode sum = literal + null; });
        }

        [TestMethod]
        public void TestRegexNodeAdditionOperator6()
        {
            RegexNodeLiteral literal = new RegexNodeLiteral("\\w*");
            Assert.ThrowsExactly<ArgumentException>(() => { RegexNode sum = null + literal; });
        }

        [TestMethod]
        public void TestRegexNodeAdditionOperator7()
        {
            Assert.ThrowsExactly<ArgumentException>(() => { RegexNode sum = (RegexNode)null + null; });
        }

        [TestMethod]
        public void TestConcatenationNodeConstruction()
        {
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation();
            Assert.AreEqual(0, concatenation.ChildNodes.Count);
        }

        [TestMethod]
        public void TestConcatenationNodeNullChildAssignment1()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeConcatenation((List<RegexNode>)null));
        }

        [TestMethod]
        public void TestConcatenationNodeNullChildAssignment2()
        {
            RegexNodeLiteral literal1 = new RegexNodeLiteral("a");
            RegexNodeLiteral literal2 = new RegexNodeLiteral("b");
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation(literal1, literal2);
            Assert.AreEqual(2, concatenation.ChildNodes.Count);

            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeConcatenation(null));
        }

        [TestMethod]
        public void TestAlternationNodeConstruction()
        {
            RegexNodeLiteral literal1 = new RegexNodeLiteral("a");
            RegexNodeLiteral literal2 = new RegexNodeLiteral("b");
            RegexNodeAlternation alternation = new RegexNodeAlternation(literal1, literal2);

            List<RegexNode> expressions = new List<RegexNode>(alternation.Expressions);
            Assert.AreEqual(2, expressions.Count);
            Assert.AreEqual(expressions[0], literal1);
            Assert.AreEqual(expressions[1], literal2);
        }

        [TestMethod]
        public void TestAlternationNodeNullExpressionListAssignment1()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeAlternation(null));
        }

        [TestMethod]
        public void TestAlternationNodeNullExpressionListAssignment2()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeAlternation(null, null));
        }

        [TestMethod]
        public void TestAlternationNodeNullExpressionListAssignment3()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeAlternation(new RegexNodeLiteral("a"), new RegexNodeLiteral("b"), null));
        }

        [TestMethod]
        public void TestBacktrackingSuppressionNodeNullExpressionAssignment1()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeBacktrackingSuppression(null));
        }

        [TestMethod]
        public void TestBacktrackingSuppressionNodeNullExpressionAssignment2()
        {
            RegexNodeBacktrackingSuppression suppression = new RegexNodeBacktrackingSuppression(new RegexNodeLiteral("a"));
            Assert.ThrowsExactly<ArgumentNullException>(() => suppression.InnerExpression = null);
        }

        [TestMethod]
        public void TestCharacterSetNodeCharacterListExpressionProperty()
        {
            RegexNodeCharacterSet characterSet = new RegexNodeCharacterSet(new[] { 'x', 'y', 'z' }, false);
            characterSet.CharacterListExpression = "abc";
            List<char> characters = new List<char>(characterSet.Characters);
            Assert.AreEqual(3, characters.Count);
            Assert.IsTrue(characters[0] == 'a' && characters[1] == 'b' && characters[2] == 'c');
            Assert.IsTrue(characterSet.CharacterListExpression == "abc");
        }

        [TestMethod]
        public void TestCharacterSetNodeNullAssignment1()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeCharacterSet((char[])null, false));
        }

        [TestMethod]
        public void TestCharacterSetNodeNullAssignment2()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeCharacterSet((string)null, false));
        }

        [TestMethod]
        public void TestCharacterSetNodeNullAssignment3()
        {
            RegexNodeCharacterSet characterSet = new RegexNodeCharacterSet(new[] { 'a', 'b', 'c' }, false);
            Assert.ThrowsExactly<ArgumentNullException>(() => characterSet.CharacterListExpression = null);
        }

        [TestMethod]
        public void TestConditionalMatchNodeNullAssignment1()
        {
            RegexNodeLiteral condition = new RegexNodeLiteral("a");
            RegexNodeLiteral trueMatch = new RegexNodeLiteral("b");
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeConditionalMatch(condition, trueMatch, null));
        }

        [TestMethod]
        public void TestConditionalMatchNodeNullAssignment2()
        {
            RegexNodeLiteral condition = new RegexNodeLiteral("a");
            RegexNodeLiteral falseMatch = new RegexNodeLiteral("c");
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeConditionalMatch(condition, null, falseMatch));
        }

        [TestMethod]
        public void TestConditionalMatchNodeNullAssignment3()
        {
            RegexNodeLiteral condition = new RegexNodeLiteral("a");
            RegexNodeLiteral trueMatch = new RegexNodeLiteral("b");
            RegexNodeLiteral falseMatch = new RegexNodeLiteral("c");
            RegexNodeConditionalMatch conditionalMatch = new RegexNodeConditionalMatch(condition, trueMatch, falseMatch);
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                conditionalMatch.TrueMatchExpression = null;
                conditionalMatch.FalseMatchExpression = null;
            });
        }

        [TestMethod]
        public void TestGroupNodeNullAssignment1()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeGroup(null));
        }

        [TestMethod]
        public void TestGroupNodeNullAssignment2()
        {
            RegexNodeGroup group = new RegexNodeGroup(new RegexNodeLiteral("abc"));
            Assert.ThrowsExactly<ArgumentNullException>(() => group.InnerExpression = null);
        }

        [TestMethod]
        public void TestLookAroundNodeNullAssignment1()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeLookAround(RegexLookAround.PositiveLookAhead, null, null));
        }

        [TestMethod]
        public void TestLookAroundNodeNullAssignment2()
        {
            RegexNodeLookAround lookAround = new RegexNodeLookAround(RegexLookAround.PositiveLookAhead, new RegexNodeLiteral("abc"), new RegexNodeLiteral("abc"));
            Assert.ThrowsExactly<ArgumentNullException>(() => lookAround.LookAroundExpression = null);
        }

        [TestMethod]
        public void TestLookAroundNodeNullAssignment3()
        {
            RegexNodeLookAround lookAround = new RegexNodeLookAround(RegexLookAround.PositiveLookAhead, new RegexNodeLiteral("abc"), new RegexNodeLiteral("abc"));
            Assert.ThrowsExactly<ArgumentNullException>(() => lookAround.MatchExpression = null);
        }

        [TestMethod]
        public void TestInlineOptionNodeQuantifierAssignment()
        {
            RegexNodeInlineOption option = new RegexNodeInlineOption(RegexOptions.Multiline, new RegexNodeLiteral("abc"));
            Assert.ThrowsExactly<InvalidOperationException>(() => option.Quantifier = RegexQuantifier.OneOrMore);
        }

        [TestMethod]
        public void TestInlineOptionNodeNullExpressionAssignment()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new RegexNodeInlineOption(RegexOptions.Multiline, null));
        }

        [TestMethod]
        public void TestInlineOptionNodeInvalidOptionAssignment1()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new RegexNodeInlineOption(RegexOptions.None, new RegexNodeLiteral("abc")));
        }

        [TestMethod]
        public void TestInlineOptionNodeInvalidOptionAssignment2()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new RegexNodeInlineOption(RegexOptions.CultureInvariant, new RegexNodeLiteral("abc")));
        }

        [TestMethod]
        public void TestInlineOptionNodeInvalidOptionAssignment3()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new RegexNodeInlineOption(RegexOptions.ECMAScript, new RegexNodeLiteral("abc")));
        }

        [TestMethod]
        public void TestInlineOptionNodeInvalidOptionAssignment4()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new RegexNodeInlineOption(RegexOptions.RightToLeft, new RegexNodeLiteral("abc")));
        }

        [TestMethod]
        public void TestInlineOptionNodeInvalidOptionAssignment5()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new RegexNodeInlineOption(RegexOptions.Compiled, new RegexNodeLiteral("abc")));
        }
    }
}
