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
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestQuantifierSetterForNodesWithQuantifierNotAllowed()
        {
            RegexNode node = new RegexNodeComment("This is a comment");
            node.Quantifier = RegexQuantifier.OneOrMore;
        }

        [TestMethod]
        public void TestRegexNodeAdditionOperator1()
        {
            RegexNodeLiteral literal1 = new RegexNodeLiteral("\\w*");
            RegexNodeLiteral literal2 = new RegexNodeLiteral("\\d+");

            RegexNode sum = literal1 + literal2;
            Assert.IsInstanceOfType(sum, typeof(RegexNodeConcatenation));
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

            Assert.IsInstanceOfType(sum, typeof(RegexNodeConcatenation));
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

            Assert.IsInstanceOfType(sum, typeof(RegexNodeConcatenation));
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

            Assert.IsInstanceOfType(sum, typeof(RegexNodeConcatenation));
            Assert.AreNotSame(concatenation1, sum);
            Assert.AreNotSame(concatenation2, sum);
            Assert.AreEqual(literal1, ((RegexNodeConcatenation)sum).ChildNodes[0]);
            Assert.AreEqual(literal2, ((RegexNodeConcatenation)sum).ChildNodes[1]);
            Assert.AreEqual(literal3, ((RegexNodeConcatenation)sum).ChildNodes[2]);
            Assert.AreEqual(literal4, ((RegexNodeConcatenation)sum).ChildNodes[3]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRegexNodeAdditionOperator5()
        {
            RegexNodeLiteral literal = new RegexNodeLiteral("\\w*");
            RegexNode sum = literal + null;
            Assert.IsNull(sum);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRegexNodeAdditionOperator6()
        {
            RegexNodeLiteral literal = new RegexNodeLiteral("\\w*");
            RegexNode sum = null + literal;
            Assert.IsNull(sum);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRegexNodeAdditionOperator7()
        {
            RegexNode sum = (RegexNode)null + null;
            Assert.IsNull(sum);
        }

        [TestMethod]
        public void TestConcatenationNodeConstruction()
        {
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation();
            Assert.AreEqual(concatenation.ChildNodes.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConcatenationNodeNullChildAssignment1()
        {
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation((List<RegexNode>)null);
            Assert.IsNull(concatenation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConcatenationNodeNullChildAssignment2()
        {
            RegexNodeLiteral literal1 = new RegexNodeLiteral("a");
            RegexNodeLiteral literal2 = new RegexNodeLiteral("b");
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation(literal1, literal2);
            Assert.AreEqual(2, concatenation.ChildNodes.Count);
            
            // ReSharper disable RedundantAssignment
            // -- This line is expected to cause an exception.
            concatenation = new RegexNodeConcatenation(null);
            // ReSharper restore RedundantAssignment
        }

        [TestMethod]
        public void TestAlternationNodeConstruction()
        {
            RegexNodeLiteral literal1 = new RegexNodeLiteral("a");
            RegexNodeLiteral literal2 = new RegexNodeLiteral("b");
            RegexNodeAlternation alternation = new RegexNodeAlternation(literal1, literal2);

            List<RegexNode> expressions = new List<RegexNode>(alternation.Expressions);
            Assert.AreEqual(expressions.Count, 2);
            Assert.AreEqual(expressions[0], literal1);
            Assert.AreEqual(expressions[1], literal2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAlternationNodeNullExpressionListAssignment1()
        {
            RegexNodeAlternation alternation = new RegexNodeAlternation(null);
            Assert.IsNull(alternation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAlternationNodeNullExpressionListAssignment2()
        {
            RegexNodeAlternation alternation = new RegexNodeAlternation(null, null);
            Assert.IsNull(alternation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAlternationNodeNullExpressionListAssignment3()
        {
            RegexNodeAlternation alternation = new RegexNodeAlternation(new RegexNodeLiteral("a"), new RegexNodeLiteral("b"), null);
            Assert.IsNull(alternation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBacktrackingSuppressionNodeNullExpressionAssignment1()
        {
            RegexNodeBacktrackingSuppression alternation = new RegexNodeBacktrackingSuppression(null);
            Assert.IsNull(alternation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBacktrackingSuppressionNodeNullExpressionAssignment2()
        {
            RegexNodeBacktrackingSuppression alternation = new RegexNodeBacktrackingSuppression(new RegexNodeLiteral("a"));
            alternation.InnerExpression = null;
            Assert.IsNull(alternation);
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCharacterSetNodeNullAssignment1()
        {
            RegexNodeCharacterSet characterSet = new RegexNodeCharacterSet((char[])null, false);
            Assert.IsNull(characterSet);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCharacterSetNodeNullAssignment2()
        {
            RegexNodeCharacterSet characterSet = new RegexNodeCharacterSet((string)null, false);
            Assert.IsNull(characterSet);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCharacterSetNodeNullAssignment3()
        {
            RegexNodeCharacterSet characterSet = new RegexNodeCharacterSet(new[] { 'a', 'b', 'c' }, false);
            characterSet.CharacterListExpression = null;
            Assert.IsNull(characterSet);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConditionalMatchNodeNullAssignment1()
        {
            RegexNodeLiteral condition = new RegexNodeLiteral("a");
            RegexNodeLiteral trueMatch = new RegexNodeLiteral("b");
            RegexNodeConditionalMatch conditionalMatch = new RegexNodeConditionalMatch(condition, trueMatch, null);
            Assert.IsNull(conditionalMatch);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConditionalMatchNodeNullAssignment2()
        {
            RegexNodeLiteral condition = new RegexNodeLiteral("a");
            RegexNodeLiteral falseMatch = new RegexNodeLiteral("c");
            RegexNodeConditionalMatch conditionalMatch = new RegexNodeConditionalMatch(condition, null, falseMatch);
            Assert.IsNull(conditionalMatch);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConditionalMatchNodeNullAssignment3()
        {
            RegexNodeLiteral condition = new RegexNodeLiteral("a");
            RegexNodeLiteral trueMatch = new RegexNodeLiteral("b");
            RegexNodeLiteral falseMatch = new RegexNodeLiteral("c");
            RegexNodeConditionalMatch conditionalMatch = new RegexNodeConditionalMatch(condition, trueMatch, falseMatch);
            conditionalMatch.TrueMatchExpression = null;
            conditionalMatch.FalseMatchExpression = null;
            Assert.IsNull(conditionalMatch);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGroupNodeNullAssignment1()
        {
            RegexNodeGroup group = new RegexNodeGroup(null);
            Assert.IsNull(group);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGroupNodeNullAssignment2()
        {
            RegexNodeGroup group = new RegexNodeGroup(new RegexNodeLiteral("abc"));
            group.InnerExpression = null;
            Assert.IsNull(group);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLookAroundNodeNullAssignment1()
        {
            RegexNodeLookAround lookAround = new RegexNodeLookAround(RegexLookAround.PositiveLookAhead, null, null);
            Assert.IsNull(lookAround);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLookAroundNodeNullAssignment2()
        {
            RegexNodeLookAround lookAround = new RegexNodeLookAround(RegexLookAround.PositiveLookAhead, new RegexNodeLiteral("abc"), new RegexNodeLiteral("abc"));
            lookAround.LookAroundExpression = null;
            Assert.IsNull(lookAround);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLookAroundNodeNullAssignment3()
        {
            RegexNodeLookAround lookAround = new RegexNodeLookAround(RegexLookAround.PositiveLookAhead, new RegexNodeLiteral("abc"), new RegexNodeLiteral("abc"));
            lookAround.MatchExpression = null;
            Assert.IsNull(lookAround);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInlineOptionNodeQuantifierAssignment()
        {
            RegexNodeInlineOption option = new RegexNodeInlineOption(RegexOptions.Multiline, new RegexNodeLiteral("abc"));
            option.Quantifier = RegexQuantifier.OneOrMore;
            Assert.IsNull(option);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInlineOptionNodeNullExpressionAssignment()
        {
            RegexNodeInlineOption option = new RegexNodeInlineOption(RegexOptions.Multiline, null);
            Assert.IsNull(option);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInlineOptionNodeInvalidOptionAssignment1()
        {
            RegexNodeInlineOption option = new RegexNodeInlineOption(RegexOptions.None, new RegexNodeLiteral("abc"));
            Assert.IsNull(option);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInlineOptionNodeInvalidOptionAssignment2()
        {
            RegexNodeInlineOption option = new RegexNodeInlineOption(RegexOptions.CultureInvariant, new RegexNodeLiteral("abc"));
            Assert.IsNull(option);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInlineOptionNodeInvalidOptionAssignment3()
        {
            RegexNodeInlineOption option = new RegexNodeInlineOption(RegexOptions.ECMAScript, new RegexNodeLiteral("abc"));
            Assert.IsNull(option);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInlineOptionNodeInvalidOptionAssignment4()
        {
            RegexNodeInlineOption option = new RegexNodeInlineOption(RegexOptions.RightToLeft, new RegexNodeLiteral("abc"));
            Assert.IsNull(option);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInlineOptionNodeInvalidOptionAssignment5()
        {
            RegexNodeInlineOption option = new RegexNodeInlineOption(RegexOptions.Compiled, new RegexNodeLiteral("abc"));
            Assert.IsNull(option);
        }
    }
}
