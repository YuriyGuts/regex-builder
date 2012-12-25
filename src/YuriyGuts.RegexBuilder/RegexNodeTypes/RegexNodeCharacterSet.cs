using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace YuriyGuts.RegexBuilder
{
    public class RegexNodeCharacterSet : RegexNode
    {
        private char[] characters;
        private string characterListExpression;

        protected override bool AllowQuantifier
        {
            get { return true; }
        }

        public IEnumerable<char> Characters
        {
            get { return characters; }            
        }

        public string CharacterListExpression
        {
            get { return characterListExpression; }
            set { InitializeCharactersFromString(value); }
        }

        public bool IsNegative { get; set; }
        public bool UseCharacterCodes { get; set; }

        public RegexNodeCharacterSet(char[] characters, bool isNegative)
        {
            if (characters == null)
            {
                throw new ArgumentNullException("characters", "Character array cannot be null");
            }
            this.characters = characters;
            characterListExpression = RegexStringEscaper.Escape(new string(characters), false);
            IsNegative = isNegative;
        }

        public RegexNodeCharacterSet(string characterListExpression, bool isNegative)
        {
            InitializeCharactersFromString(characterListExpression);
            IsNegative = isNegative;
        }

        private void InitializeCharactersFromString(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "CharacterListExpression cannot be null");
            }
            characterListExpression = RegexStringEscaper.Escape(value, false);
            characters = characterListExpression.ToCharArray();
        }        

        public override string ToRegexPattern()
        {
            string characterSetPattern;
            if (UseCharacterCodes)
            {
                StringBuilder patternBuilder = new StringBuilder();
                foreach (char c in characters)
                {
                    patternBuilder.AppendFormat(CultureInfo.InvariantCulture, "\\u{0:x4}", (int)c);
                }
                characterSetPattern = patternBuilder.ToString();
            }
            else
            {
                characterSetPattern = characterListExpression;
            }
            
            string result = string.Format(CultureInfo.InvariantCulture, (IsNegative ? "[^{0}]" : "[{0}]"), characterSetPattern);
            if (HasQuantifier)
            {
                result += Quantifier.ToRegexPattern();
            }

            return result;
        }
    }
}
