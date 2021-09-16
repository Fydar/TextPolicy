using System;

namespace TextConstraints
{
	/// <summary>
	/// A rule for evaluating text.
	/// </summary>
	public interface ITextRule
	{
		/// <summary>
		/// Evaluates this rule against some text.
		/// </summary>
		/// <param name="text">The text to evaluate against this <see cref="ITextRule"/>.</param>
		/// <returns>A <see cref="TextRuleResult"/> that describes the result of the evaluation.</returns>
		TextRuleResult Evaluate(ReadOnlySpan<char> text);
	}
}
