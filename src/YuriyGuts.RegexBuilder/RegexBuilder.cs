using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YuriyGuts.RegexBuilder
{
    public static class RegexBuilder
    {
        #region Build methods

        /// <summary>
        /// Generates a Regex object from a single RegexNode.
        /// </summary>
        /// <param name="rootNode">Root node.</param>
        /// <returns>A new instance of Regex which corresponds to the specified RegexNode.</returns>
        public static Regex Build(RegexNode rootNode)
        {
            if (rootNode == null)
            {
                throw new ArgumentNullException("rootNode");
            }
            return new Regex(rootNode.ToRegexPattern());
        }

        /// <summary>
        /// Generates a Regex object from a single RegexNode and applies a combination of RegexOptions to it.
        /// </summary>
        /// <param name="regexOptions">Combination of RegexOption flags to be applied to the Regex.</param>
        /// <param name="rootNode">Root node.</param>
        /// <returns>A new instance of Regex which corresponds to the specified RegexNode.</returns>
        public static Regex Build(RegexOptions regexOptions, RegexNode rootNode)
        {
            if (rootNode == null)
            {
                throw new ArgumentNullException("rootNode");
            }
            return new Regex(rootNode.ToRegexPattern(), regexOptions);
        }

        /// <summary>
        /// Generates a Regex object from a list of RegexNodes.
        /// </summary>
        /// <param name="regexNodes">Top-level nodes for the Regex.</param>
        /// <returns>A new instance of Regex which corresponds to the specified RegexNode list.</returns>
        public static Regex Build(params RegexNode[] regexNodes)
        {
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation(regexNodes);
            return Build(concatenation);
        }

        /// <summary>
        /// Generates a Regex object from a list of RegexNode and applies a combination of RegexOptions to it.
        /// </summary>
        /// <param name="regexOptions">Combination of RegexOption flags to be applied to the Regex.</param>
        /// <param name="regexNodes">Top-level nodes for the Regex.</param>
        /// <returns>A new instance of Regex which corresponds to the specified RegexNode list.</returns>
        public static Regex Build(RegexOptions regexOptions, params RegexNode[] regexNodes)
        {
            RegexNodeConcatenation concatenation = new RegexNodeConcatenation(regexNodes);
            return Build(regexOptions, concatenation);
        }

        #endregion Build methods

        #region Static factory methods

        /// <summary>
        /// Generates a simple string literal with automatic character escaping.
        /// </summary>
        /// <param name="value">Node text. Special characters will be automatically escaped.</param>
        /// <returns>An instance of RegexNode containing the specified text.</returns>
        public static RegexNodeEscapingLiteral Literal(string value)
        {
            return new RegexNodeEscapingLiteral(value);
        }

        /// <summary>
        /// Generates a simple string literal with automatic character escaping.
        /// </summary>
        /// <param name="value">Node text. Special characters will be automatically escaped.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the specified text.</returns>
        public static RegexNodeEscapingLiteral Literal(string value, RegexQuantifier quantifier)
        {
            return new RegexNodeEscapingLiteral(value) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a simple string literal "as is", without character escaping. This method can be used to render
        /// explicit preformatted patterns of the Regex or some rare constructions not supported by RegexBuilder.
        /// </summary>
        /// <param name="value">Node text.</param>
        /// <returns>An instance of RegexNode containing the specified text.</returns>
        public static RegexNodeLiteral NonEscapedLiteral(string value)
        {
            return new RegexNodeLiteral(value);
        }

        /// <summary>
        /// Generates a simple string literal "as is", without character escaping. This method can be used to render
        /// explicit preformatted patterns of the Regex or some rare constructions not supported by RegexBuilder.
        /// </summary>
        /// <param name="value">Node text.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the specified text.</returns>
        public static RegexNodeLiteral NonEscapedLiteral(string value, RegexQuantifier quantifier)
        {
            return new RegexNodeLiteral(value) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a Regex metacharacter, such as \d, \w or \s. Many metacharacter constants are available in RegexMetaChars class.
        /// </summary>
        /// <param name="characterPattern">Metacharacter pattern.</param>
        /// <returns>An instance of RegexNode containing the specified metacharacter.</returns>
        public static RegexNodeLiteral MetaCharacter(string characterPattern)
        {
            return new RegexNodeLiteral(characterPattern);
        }

        /// <summary>
        /// Generates a Regex metacharacter, such as \d, \w or \s. Many metacharacter constants are available in RegexMetaChars class.
        /// </summary>
        /// <param name="characterPattern">Metacharacter pattern.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the specified metacharacter.</returns>
        public static RegexNodeLiteral MetaCharacter(string characterPattern, RegexQuantifier quantifier)
        {
            return new RegexNodeLiteral(characterPattern) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates an ASCII character pattern ("\xNN") with the specified character code.
        /// </summary>
        /// <param name="code">ASCII character code.</param>
        /// <returns>An instance of RegexNode containing the specified ASCII character.</returns>
        public static RegexNodeLiteral AsciiCharacter(byte code)
        {
            return new RegexNodeLiteral(string.Format(CultureInfo.InvariantCulture, "\\x{0:x2}", code & 0xFF));
        }

        /// <summary>
        /// Generates an ASCII character pattern ("\xNN") with the specified character code.
        /// </summary>
        /// <param name="code">ASCII character code.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the specified ASCII character.</returns>
        public static RegexNodeLiteral AsciiCharacter(byte code, RegexQuantifier quantifier)
        {
            return new RegexNodeLiteral(string.Format(CultureInfo.InvariantCulture, "\\x{0:x2}", code & 0xFF)) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a Unicode character pattern ("\uNNNN") with the specified character code.
        /// </summary>
        /// <param name="code">Unicode character code.</param>
        /// <returns>An instance of RegexNode containing the specified Unicode character.</returns>
        public static RegexNodeLiteral UnicodeCharacter(int code)
        {
            return new RegexNodeLiteral(string.Format(CultureInfo.InvariantCulture, "\\u{0:x4}", code));
        }

        /// <summary>
        /// Generates a Unicode character pattern ("\uNNNN") with the specified character code.
        /// </summary>
        /// <param name="code">Unicode character code.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the specified Unicode character.</returns>
        public static RegexNodeLiteral UnicodeCharacter(int code, RegexQuantifier quantifier)
        {
            return new RegexNodeLiteral(string.Format(CultureInfo.InvariantCulture, "\\u{0:x4}", code)) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates an inline Regex comment ("(?#text)").
        /// </summary>
        /// <param name="commentText">Comment text.</param>
        /// <returns>An instance of RegexNode containing the specified comment.</returns>
        public static RegexNodeComment Comment(string commentText)
        {
            return new RegexNodeComment(commentText);
        }

        /// <summary>
        /// Generates a backreference to the group with the specified index ("\N").
        /// </summary>
        /// <param name="groupIndex">Group ordinal number.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the group reference.</returns>
        public static RegexNodeGroupReference GroupBackReference(int groupIndex, RegexQuantifier quantifier)
        {
            return new RegexNodeGroupReference(groupIndex) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a backreference to a named group ("\k&lt;GroupName&gt;").
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <returns>An instance of RegexNode containing the group reference.</returns>
        public static RegexNodeGroupReference GroupBackReference(string groupName)
        {
            return new RegexNodeGroupReference(groupName);
        }

        /// <summary>
        /// Generates a backreference to a named group ("\k&lt;GroupName&gt;").
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the group reference.</returns>
        public static RegexNodeGroupReference GroupBackReference(string groupName, RegexQuantifier quantifier)
        {
            return new RegexNodeGroupReference(groupName) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a character set expression ("[abc]") using a preformatted character string.
        /// </summary>
        /// <param name="characters">Character set description. Special characters will be automatically escaped.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the character set.</returns>
        public static RegexNodeCharacterSet CharacterSet(string characters, RegexQuantifier quantifier)
        {
            return new RegexNodeCharacterSet(characters, false) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a character set expression ("[abc]") from the specified character array.
        /// </summary>
        /// <param name="characters">An array of allowed characters.</param>
        /// <param name="useCharacterCodes">True - encode every character with "\uNNNN" pattern. False - use every character explicitly.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns></returns>
        public static RegexNodeCharacterSet CharacterSet(char[] characters, bool useCharacterCodes, RegexQuantifier quantifier)
        {
            return new RegexNodeCharacterSet(characters, false) { Quantifier = quantifier, UseCharacterCodes = useCharacterCodes };
        }

        /// <summary>
        /// Generates a negative character set expression ("[^abc]") using a preformatted character string.
        /// </summary>
        /// <param name="characters">Character set description. Special characters will be automatically escaped.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the character set.</returns>
        public static RegexNodeCharacterSet NegativeCharacterSet(string characters, RegexQuantifier quantifier)
        {
            return new RegexNodeCharacterSet(characters, true) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a negative character set expression ("[^abc]") from the specified character array.
        /// </summary>
        /// <param name="characters">An array of allowed characters.</param>
        /// <param name="useCharacterCodes">True - encode every character with "\uNNNN" pattern. False - use every character explicitly.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns></returns>
        public static RegexNodeCharacterSet NegativeCharacterSet(char[] characters, bool useCharacterCodes, RegexQuantifier quantifier)
        {
            return new RegexNodeCharacterSet(characters, true) { Quantifier = quantifier, UseCharacterCodes = useCharacterCodes };
        }

        /// <summary>
        /// Generates a character range expression ("[a-z]") with the specified start/end characters.
        /// </summary>
        /// <param name="rangeStart">First character in the range.</param>
        /// <param name="rangeEnd">Last character in the range.</param>
        /// <param name="useCharacterCodes">True - encode every character with "\uNNNN" pattern. False - use every character explicitly.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the character range.</returns>
        public static RegexNodeCharacterRange CharacterRange(char rangeStart, char rangeEnd, bool useCharacterCodes, RegexQuantifier quantifier)
        {
            return new RegexNodeCharacterRange(rangeStart, rangeEnd, false) { Quantifier = quantifier, UseCharacterCodes = useCharacterCodes };
        }

        /// <summary>
        /// Generates a negative character range expression ("[^a-z]") with the specified start/end characters.
        /// </summary>
        /// <param name="rangeStart">First character in the range.</param>
        /// <param name="rangeEnd">Last character in the range.</param>
        /// <param name="useCharacterCodes">True - encode every character with "\uNNNN" pattern. False - use every character explicitly.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the character range.</returns>
        public static RegexNodeCharacterRange NegativeCharacterRange(char rangeStart, char rangeEnd, bool useCharacterCodes, RegexQuantifier quantifier)
        {
            return new RegexNodeCharacterRange(rangeStart, rangeEnd, true) { Quantifier = quantifier, UseCharacterCodes = useCharacterCodes };
        }

        /// <summary>
        /// Generates a zero-width positive lookahead assertion ("match(?=lookahead)").
        /// </summary>
        /// <param name="lookupExpression">Lookahead expression.</param>
        /// <param name="matchExpression">Match expression.</param>
        /// <returns>An instance of RegexNode containing the positive lookahead assertion.</returns>
        public static RegexNodeLookAround PositiveLookAhead(RegexNode lookupExpression, RegexNode matchExpression)
        {
            return new RegexNodeLookAround(RegexLookAround.PositiveLookAhead, lookupExpression, matchExpression);
        }

        /// <summary>
        /// Generates a zero-width positive lookahead assertion ("match(?=lookahead)").
        /// </summary>
        /// <param name="lookupExpression">Lookahead expression.</param>
        /// <param name="matchExpression">Match expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the positive lookahead assertion.</returns>
        public static RegexNodeLookAround PositiveLookAhead(RegexNode lookupExpression, RegexNode matchExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeLookAround(RegexLookAround.PositiveLookAhead, lookupExpression, matchExpression) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a zero-width positive lookbehind assertion ("(?&lt;=lookbehind)match").
        /// </summary>
        /// <param name="lookupExpression">Lookbehind expression.</param>
        /// <param name="matchExpression">Match expression.</param>
        /// <returns>An instance of RegexNode containing the positive lookbehind assertion.</returns>
        public static RegexNodeLookAround PositiveLookBehind(RegexNode lookupExpression, RegexNode matchExpression)
        {
            return new RegexNodeLookAround(RegexLookAround.PositiveLookBehind, lookupExpression, matchExpression);
        }

        /// <summary>
        /// Generates a zero-width positive lookbehind assertion ("(?&lt;=lookbehind)match").
        /// </summary>
        /// <param name="lookupExpression">Lookbehind expression.</param>
        /// <param name="matchExpression">Match expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the positive lookbehind assertion.</returns>
        public static RegexNodeLookAround PositiveLookBehind(RegexNode lookupExpression, RegexNode matchExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeLookAround(RegexLookAround.PositiveLookBehind, lookupExpression, matchExpression) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a zero-width negative lookahead assertion ("match(?!lookahead)").
        /// </summary>
        /// <param name="lookupExpression">Lookahead expression.</param>
        /// <param name="matchExpression">Match expression.</param>
        /// <returns>An instance of RegexNode containing the negative lookahead assertion.</returns>
        public static RegexNodeLookAround NegativeLookAhead(RegexNode lookupExpression, RegexNode matchExpression)
        {
            return new RegexNodeLookAround(RegexLookAround.NegativeLookAhead, lookupExpression, matchExpression);
        }

        /// <summary>
        /// Generates a zero-width negative lookahead assertion ("match(?!lookahead)").
        /// </summary>
        /// <param name="lookupExpression">Lookahead expression.</param>
        /// <param name="matchExpression">Match expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the negative lookahead assertion.</returns>
        public static RegexNodeLookAround NegativeLookAhead(RegexNode lookupExpression, RegexNode matchExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeLookAround(RegexLookAround.NegativeLookAhead, lookupExpression, matchExpression) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a zero-width negative lookbehind assertion ("(?&lt;!lookbehind)match").
        /// </summary>
        /// <param name="lookupExpression">Lookbehind expression.</param>
        /// <param name="matchExpression">Match expression.</param>
        /// <returns>An instance of RegexNode containing the negative lookbehind assertion.</returns>
        public static RegexNodeLookAround NegativeLookBehind(RegexNode lookupExpression, RegexNode matchExpression)
        {
            return new RegexNodeLookAround(RegexLookAround.NegativeLookBehind, lookupExpression, matchExpression);
        }

        /// <summary>
        /// Generates a zero-width negative lookbehind assertion ("(?&lt;!lookbehind)match").
        /// </summary>
        /// <param name="lookupExpression">Lookbehind expression.</param>
        /// <param name="matchExpression">Match expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the negative lookbehind assertion.</returns>
        public static RegexNodeLookAround NegativeLookBehind(RegexNode lookupExpression, RegexNode matchExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeLookAround(RegexLookAround.NegativeLookBehind, lookupExpression, matchExpression) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates an unnamed capturing group with the specified subexpression.
        /// </summary>
        /// <param name="matchExpression">Inner expression.</param>
        /// <returns>An instance of RegexNode containing the unnamed capturing group.</returns>
        public static RegexNodeGroup Group(RegexNode matchExpression)
        {
            return new RegexNodeGroup(matchExpression);
        }

        /// <summary>
        /// Generates an unnamed capturing group with the specified subexpression.
        /// </summary>
        /// <param name="matchExpression">Inner expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the unnamed capturing group.</returns>
        public static RegexNodeGroup Group(RegexNode matchExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeGroup(matchExpression) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a named capturing group with the specified subexpression.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="matchExpression">Inner expression.</param>
        /// <returns>An instance of RegexNode containing the named capturing group.</returns>
        public static RegexNodeGroup Group(string groupName, RegexNode matchExpression)
        {
            return new RegexNodeGroup(matchExpression, groupName);
        }

        /// <summary>
        /// Generates a named capturing group with the specified subexpression.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="matchExpression">Inner expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the named capturing group.</returns>
        public static RegexNodeGroup Group(string groupName, RegexNode matchExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeGroup(matchExpression, groupName) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a non-capturing group with the specified subexpression.
        /// </summary>
        /// <param name="matchExpression">Inner expression.</param>
        /// <returns>An instance of RegexNode containing the non-capturing group.</returns>
        public static RegexNodeGroup NonCapturingGroup(RegexNode matchExpression)
        {
            return new RegexNodeGroup(matchExpression, false);
        }

        /// <summary>
        /// Generates a non-capturing group with the specified subexpression.
        /// </summary>
        /// <param name="matchExpression">Inner expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the non-capturing group.</returns>
        public static RegexNodeGroup NonCapturingGroup(RegexNode matchExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeGroup(matchExpression, false) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a subexpression with disabled backtracking ("(?&gt;expression)").
        /// </summary>
        /// <param name="innerExpression">Inner expression.</param>
        /// <returns>An instance of RegexNode containing the expression with suppressed backtracking.</returns>
        public static RegexNodeBacktrackingSuppression BacktrackingSuppression(RegexNode innerExpression)
        {
            return new RegexNodeBacktrackingSuppression(innerExpression);
        }

        /// <summary>
        /// Generates a subexpression with disabled backtracking ("(?&gt;expression)").
        /// </summary>
        /// <param name="innerExpression">Inner expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the expression with suppressed backtracking.</returns>
        public static RegexNodeBacktrackingSuppression BacktrackingSuppression(RegexNode innerExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeBacktrackingSuppression(innerExpression) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates an alternation expression with two options ("a|b").
        /// </summary>
        /// <param name="expression1">First option.</param>
        /// <param name="expression2">Second option.</param>
        /// <returns>An instance of RegexNode containing the alternation expression.</returns>
        public static RegexNodeAlternation Alternate(RegexNode expression1, RegexNode expression2)
        {
            return new RegexNodeAlternation(expression1, expression2);
        }

        /// <summary>
        /// Generates an alternation expression with two options ("a|b").
        /// </summary>
        /// <param name="expression1">First option.</param>
        /// <param name="expression2">Second option.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the alternation expression.</returns>
        public static RegexNodeAlternation Alternate(RegexNode expression1, RegexNode expression2, RegexQuantifier quantifier)
        {
            return new RegexNodeAlternation(expression1, expression2) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates an alternation expression with two or more options ("a|b|c|...").
        /// </summary>
        /// <param name="expressions">Array of option expressions.</param>
        /// <returns>An instance of RegexNode containing the alternation expression.</returns>
        public static RegexNodeAlternation Alternate(RegexNode[] expressions)
        {
            return new RegexNodeAlternation(expressions);
        }

        /// <summary>
        /// Generates an alternation expression with two or more options ("a|b|c|...").
        /// </summary>
        /// <param name="expressions">Array of option expressions.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the alternation expression.</returns>
        public static RegexNodeAlternation Alternate(RegexNode[] expressions, RegexQuantifier quantifier)
        {
            return new RegexNodeAlternation(expressions) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a conditional match expression ("(?(condition)|(true)|(false))").
        /// </summary>
        /// <param name="conditionExpression">Condition expression.</param>
        /// <param name="trueMatchExpression">True match expression.</param>
        /// <param name="falseMatchExpression">False match expression.</param>
        /// <returns>An instance of RegexNode containing the conditional match expression.</returns>
        public static RegexNodeConditionalMatch ConditionalMatch(RegexNode conditionExpression, RegexNode trueMatchExpression, RegexNode falseMatchExpression)
        {
            return new RegexNodeConditionalMatch(conditionExpression, trueMatchExpression, falseMatchExpression);
        }

        /// <summary>
        /// Generates a conditional match expression ("(?(condition)|(true)|(false))").
        /// </summary>
        /// <param name="conditionExpression">Condition expression.</param>
        /// <param name="trueMatchExpression">True match expression.</param>
        /// <param name="falseMatchExpression">False match expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the conditional match expression.</returns>
        public static RegexNodeConditionalMatch ConditionalMatch(RegexNode conditionExpression, RegexNode trueMatchExpression, RegexNode falseMatchExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeConditionalMatch(conditionExpression, trueMatchExpression, falseMatchExpression) { Quantifier = quantifier };
        }

        /// <summary>
        /// Generates a conditional match expression which uses a named group for condition evaluation ("(?(GroupName)|(true)|(false))").
        /// </summary>
        /// <param name="conditionGroupName">The name of the group to be used as a condition.</param>
        /// <param name="trueMatchExpression">True match expression.</param>
        /// <param name="falseMatchExpression">False match expression.</param>
        /// <returns>An instance of RegexNode containing the conditional match expression.</returns>
        public static RegexNodeConditionalMatch ConditionalMatch(string conditionGroupName, RegexNode trueMatchExpression, RegexNode falseMatchExpression)
        {
            return new RegexNodeConditionalMatch(conditionGroupName, trueMatchExpression, falseMatchExpression);
        }

        /// <summary>
        /// Generates a conditional match expression which uses a named group for condition evaluation ("(?(GroupName)|(true)|(false))").
        /// </summary>
        /// <param name="conditionGroupName">The name of the group to be used as a condition.</param>
        /// <param name="trueMatchExpression">True match expression.</param>
        /// <param name="falseMatchExpression">False match expression.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode containing the conditional match expression.</returns>
        public static RegexNodeConditionalMatch ConditionalMatch(string conditionGroupName, RegexNode trueMatchExpression, RegexNode falseMatchExpression, RegexQuantifier quantifier)
        {
            return new RegexNodeConditionalMatch(conditionGroupName, trueMatchExpression, falseMatchExpression) { Quantifier = quantifier };
        }

        /// <summary>
        /// Concatenates two nodes.
        /// </summary>
        /// <param name="node1">First node.</param>
        /// <param name="node2">Second node.</param>
        /// <returns>An instance of RegexNode representing the concatenation of child nodes.</returns>
        public static RegexNodeConcatenation Concatenate(RegexNode node1, RegexNode node2)
        {
            return Concatenate(new[] { node1, node2 });
        }

        /// <summary>
        /// Concatenates two nodes.
        /// </summary>
        /// <param name="node1">First node.</param>
        /// <param name="node2">Second node.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode representing the concatenation of child nodes.</returns>
        public static RegexNodeConcatenation Concatenate(RegexNode node1, RegexNode node2, RegexQuantifier quantifier)
        {
            return Concatenate(new[] { node1, node2 }, quantifier);
        }

        /// <summary>
        /// Concatenates three nodes.
        /// </summary>
        /// <param name="node1">First node.</param>
        /// <param name="node2">Second node.</param>
        /// <param name="node3">Third node.</param>
        /// <returns>An instance of RegexNode representing the concatenation of child nodes.</returns>
        public static RegexNodeConcatenation Concatenate(RegexNode node1, RegexNode node2, RegexNode node3)
        {
            return Concatenate(new[] { node1, node2, node3 });
        }

        /// <summary>
        /// Concatenates three nodes.
        /// </summary>
        /// <param name="node1">First node.</param>
        /// <param name="node2">Second node.</param>
        /// <param name="node3">Third node.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode representing the concatenation of child nodes.</returns>
        public static RegexNodeConcatenation Concatenate(RegexNode node1, RegexNode node2, RegexNode node3, RegexQuantifier quantifier)
        {
            return Concatenate(new[] { node1, node2, node3 }, quantifier);
        }

        /// <summary>
        /// Concatenates four nodes.
        /// </summary>
        /// <param name="node1">First node.</param>
        /// <param name="node2">Second node.</param>
        /// <param name="node3">Third node.</param>
        /// <param name="node4">Fourth node.</param>
        /// <returns>An instance of RegexNode representing the concatenation of child nodes.</returns>
        public static RegexNodeConcatenation Concatenate(RegexNode node1, RegexNode node2, RegexNode node3, RegexNode node4)
        {
            return Concatenate(new[] { node1, node2, node3, node4 });
        }

        /// <summary>
        /// Concatenates four nodes.
        /// </summary>
        /// <param name="node1">First node.</param>
        /// <param name="node2">Second node.</param>
        /// <param name="node3">Third node.</param>
        /// <param name="node4">Fourth node.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode representing the concatenation of child nodes.</returns>
        public static RegexNodeConcatenation Concatenate(RegexNode node1, RegexNode node2, RegexNode node3, RegexNode node4, RegexQuantifier quantifier)
        {
            return Concatenate(new[] { node1, node2, node3, node4 }, quantifier);
        }

        /// <summary>
        /// Concatenates an array of nodes.
        /// </summary>
        /// <param name="expressions">Nodes to concatenate.</param>
        /// <returns>An instance of RegexNode representing the concatenation of child nodes.</returns>
        public static RegexNodeConcatenation Concatenate(RegexNode[] expressions)
        {
            return new RegexNodeConcatenation(expressions);
        }

        /// <summary>
        /// Concatenates an array of nodes.
        /// </summary>
        /// <param name="expressions">Nodes to concatenate.</param>
        /// <param name="quantifier">Node quantifier.</param>
        /// <returns>An instance of RegexNode representing the concatenation of child nodes.</returns>
        public static RegexNodeConcatenation Concatenate(RegexNode[] expressions, RegexQuantifier quantifier)
        {
            return new RegexNodeConcatenation(expressions) { Quantifier = quantifier };
        }

        #endregion Static factory methods
    }
}
