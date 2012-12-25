using System;
using System.Globalization;

namespace YuriyGuts.RegexBuilder
{
    public class RegexQuantifier
    {
        private int? minOccurrenceCount;
        private int? maxOccurrenceCount;

        /// <summary>
        /// Minimum occurrence count.
        /// </summary>
        public int? MinOccurrenceCount
        {
            get { return minOccurrenceCount; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("MinOccurrenceCount cannot be null.");
                }
                if (value < 0)
                {
                    throw new ArgumentException("MinOccurrenceCount cannot be negative.");
                }
                minOccurrenceCount = value;
            }
        }

        /// <summary>
        /// Maximum occurrence count. NULL = unlimited.
        /// </summary>
        public int? MaxOccurrenceCount
        {
            get { return maxOccurrenceCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("MaxOccurrenceCount cannot be negative.");
                }
                maxOccurrenceCount = value;
            }
        }

        /// <summary>
        /// Specifies whether the quantified expression should be matched as few times as possible.
        /// </summary>
        public bool IsLazy { get; set; }

        /// <summary>
        /// NULL quantifier.
        /// </summary>
        public static RegexQuantifier None
        {
            get { return null; }
        }

        /// <summary>
        /// The "*" quantifier.
        /// </summary>
        public static RegexQuantifier ZeroOrMore
        {
            get { return new RegexQuantifier(0, null, false); }
        }

        /// <summary>
        /// The "*?" quantifier.
        /// </summary>
        public static RegexQuantifier ZeroOrMoreLazy
        {
            get { return new RegexQuantifier(0, null, true); }
        }

        /// <summary>
        /// The "+" quantifier.
        /// </summary>
        public static RegexQuantifier OneOrMore
        {
            get { return new RegexQuantifier(1, null, false); }
        }

        /// <summary>
        /// The "+?" quantifier.
        /// </summary>
        public static RegexQuantifier OneOrMoreLazy
        {
            get { return new RegexQuantifier(1, null, true); }
        }

        /// <summary>
        /// The "?" quantifier.
        /// </summary>
        public static RegexQuantifier ZeroOrOne
        {
            get { return new RegexQuantifier(0, 1, false); }
        }

        /// <summary>
        /// The "??" quantifier.
        /// </summary>
        public static RegexQuantifier ZeroOrOneLazy
        {
            get { return new RegexQuantifier(0, 1, true); }
        }

        /// <summary>
        /// The "{n,}" quantifier.
        /// <param name="minOccurrenceCount">Minimum occurrence count.</param>
        /// </summary>
        /// <returns>An instance of RegexQuantifier with the specified options.</returns>
        public static RegexQuantifier AtLeast(int minOccurrenceCount)
        {
            return new RegexQuantifier(minOccurrenceCount, null, false);
        }

        /// <summary>
        /// The "{n,}" or "{n,}?" quantifier.
        /// <param name="minOccurrenceCount">Minimum occurrence count.</param>
        /// <param name="isLazy">True - use lazy quantifier. False - use greedy quantifier.</param>
        /// </summary>
        /// <returns>An instance of RegexQuantifier with the specified options.</returns>
        public static RegexQuantifier AtLeast(int minOccurrenceCount, bool isLazy)
        {
            return new RegexQuantifier(minOccurrenceCount, null, isLazy);
        }

        /// <summary>
        /// The "{n}" or "{n}?" quantifier.
        /// <param name="occurrenceCount">Exact occurrence count.</param>
        /// </summary>
        /// <returns>An instance of RegexQuantifier with the specified options.</returns>
        public static RegexQuantifier Exactly(int occurrenceCount)
        {
            return new RegexQuantifier(occurrenceCount, occurrenceCount, false);
        }

        /// <summary>
        /// The "{n}" or "{n}?" quantifier.
        /// <param name="occurrenceCount">Exact occurrence count.</param>
        /// <param name="isLazy">True - use lazy quantifier. False - use greedy quantifier.</param>
        /// </summary>
        /// <returns>An instance of RegexQuantifier with the specified options.</returns>
        public static RegexQuantifier Exactly(int occurrenceCount, bool isLazy)
        {
            return new RegexQuantifier(occurrenceCount, occurrenceCount, isLazy);
        }

        /// <summary>
        /// Custom "{n,m}" or "{n,m}?" quantifier.
        /// </summary>
        /// <param name="minOccurrenceCount">Minimum occurrence count.</param>
        /// <param name="maxOccurrenceCount">Maximum occurrence count.</param>
        /// <param name="isLazy">True - use lazy quantifier. False - use greedy quantifier.</param>
        /// <returns>An instance of RegexQuantifier with the specified options.</returns>
        public static RegexQuantifier Custom(int? minOccurrenceCount, int? maxOccurrenceCount, bool isLazy)
        {
            return new RegexQuantifier(minOccurrenceCount, maxOccurrenceCount, isLazy);
        }

        /// <summary>
        /// Initializes a new instance of RegexQuantifier.
        /// </summary>
        public RegexQuantifier()
        {
            IsLazy = false;
        }

        /// <summary>
        /// Initializes a new instance of RegexQuantifier.
        /// </summary>
        /// <param name="minOccurrenceCount">Minimum occurrence count.</param>
        /// <param name="maxOccurrenceCount">Maximum occurrence count.</param>
        public RegexQuantifier(int? minOccurrenceCount, int? maxOccurrenceCount)
            : this()
        {
            MinOccurrenceCount = minOccurrenceCount;
            MaxOccurrenceCount = maxOccurrenceCount;
        }

        /// <summary>
        /// Initializes a new instance of RegexQuantifier.
        /// </summary>
        /// <param name="minOccurrenceCount">Minimum occurrence count.</param>
        /// <param name="maxOccurrenceCount">Maximum occurrence count.</param>
        /// <param name="isLazy">True - use lazy quantifier. False - use greedy quantifier.</param>
        public RegexQuantifier(int? minOccurrenceCount, int? maxOccurrenceCount, bool isLazy)
            : this(minOccurrenceCount, maxOccurrenceCount)
        {
            IsLazy = isLazy;
        }

        /// <summary>
        /// Converts RegexQuantifier to a Regex pattern string.
        /// </summary>
        /// <returns></returns>
        public virtual string ToRegexPattern()
        {
            string result;

            if (MinOccurrenceCount == 0 && MaxOccurrenceCount == 1)
            {
                result = "?";
            }
            else if (MinOccurrenceCount == 0 && MaxOccurrenceCount == null)
            {
                result = "*";
            }
            else if (MinOccurrenceCount == 1 && MaxOccurrenceCount == null)
            {
                result = "+";
            }
            else if (MinOccurrenceCount == MaxOccurrenceCount)
            {
                result = string.Format(CultureInfo.InvariantCulture, "{{{0}}}", MinOccurrenceCount);
            }
            else if (MaxOccurrenceCount == null)
            {
                result = string.Format(CultureInfo.InvariantCulture, "{{{0},}}", MinOccurrenceCount);
            }
            else
            {
                result = string.Format(CultureInfo.InvariantCulture, "{{{0},{1}}}", MinOccurrenceCount, MaxOccurrenceCount);
            }

            if (IsLazy)
            {
                result += "?";
            }

            return result;
        }
    }
}
