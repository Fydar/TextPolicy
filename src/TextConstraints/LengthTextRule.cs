using System;

namespace TextConstraints
{
	/// <summary>
	/// Ensures the length of the text is within a specified range.
	/// </summary>
	public class LengthTextRule : ITextRule
	{
		/// <summary>
		/// The <b>inclusive</b> minimum length of the text.
		/// </summary>
		public int MinLength { get; }

		/// <summary>
		/// The <b>inclusive</b> maximum length of the text.
		/// </summary>
		public int MaxLength { get; }

		/// <summary>
		/// Constructs a new instance of the <see cref="LengthTextRule"/> class.
		/// </summary>
		/// <param name="minLength">The <b>inclusive</b> minimum length of the text.</param>
		/// <param name="maxLength">The <b>inclusive</b> maximum length of the text.</param>
		public LengthTextRule(int minLength, int maxLength)
		{
			MinLength = minLength;
			MaxLength = maxLength;
		}

		/// <inheritdoc/>
		public TextRuleResult Evaluate(ReadOnlySpan<char> text)
		{
			if (text.Length > MaxLength)
			{
				return TextRuleResult.Failed();
			}

			if (text.Length < MinLength)
			{
				return TextRuleResult.Failed();
			}

			return TextRuleResult.Passed();
		}
	}
}
