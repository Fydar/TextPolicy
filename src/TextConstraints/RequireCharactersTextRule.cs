using System;

namespace TextConstraints
{
	/// <summary>
	/// Ensures the text has an amount of characters that matcha predicate.
	/// </summary>
	public class RequireCharactersTextRule : ITextRule
	{
		/// <summary>
		/// The amount of characters that must match the predicate.
		/// </summary>
		public int MinAmount { get; }

		/// <summary>
		/// A predicate used to determine whether the character meets the condition of this rule.
		/// </summary>
		public Func<char, bool> Predicate { get; }

		/// <summary>
		/// Constructs a new instance of the <see cref="RequireCharactersTextRule"/> class.
		/// </summary>
		/// <param name="minAmount">The amount of characters that must match the predicate.</param>
		/// <param name="predicate">A predicate used to determine whether the character meets the condition of this rule.</param>
		public RequireCharactersTextRule(int minAmount, Func<char, bool> predicate)
		{
			MinAmount = minAmount;
			Predicate = predicate;
		}

		/// <inheritdoc/>
		public TextRuleResult Evaluate(ReadOnlySpan<char> text)
		{
			int matches = 0;

			foreach (var character in text)
			{
				if (Predicate.Invoke(character))
				{
					matches++;
				}
			}

			if (matches < MinAmount)
			{
				return TextRuleResult.Failed();
			}

			return TextRuleResult.Passed();
		}
	}
}
