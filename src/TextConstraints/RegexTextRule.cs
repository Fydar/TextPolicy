using System;
using System.Text.RegularExpressions;

namespace TextConstraints
{
	/// <summary>
	/// Controls the evaluation of a text via a Regex expression.
	/// </summary>
	public class RegexTextRule : ITextRule
	{
		/// <summary>
		/// The <see cref="Regex"/> expression used to evaluate the text.
		/// </summary>
		public Regex Evaluator { get; }

		/// <summary>
		/// If <c>true</c>, evaluations of text should have matches in order to pass this <see cref="ITextRule"/>; otherwise <c>false</c>.
		/// </summary>
		public bool ShouldHaveMatches { get; }

		/// <summary>
		/// Constructs a new instance of the <see cref="RegexTextRule"/> class.
		/// </summary>
		/// <param name="pattern">A regular expression used to evaluate the text.</param>
		/// <param name="shouldHaveMatches">If <c>true</c>, evaluations of text should have matches in order to pass this <see cref="ITextRule"/>; otherwise <c>false</c>.</param>
		public RegexTextRule(string pattern, bool shouldHaveMatches)
		{
			try
			{
				Evaluator = new Regex(pattern);
			}
			catch (ArgumentException exception)
			{
				throw new ArgumentException($"The supplied regex is invalid.", nameof(pattern), exception);
			}

			ShouldHaveMatches = shouldHaveMatches;
		}

		/// <inheritdoc/>
		public TextRuleResult Evaluate(ReadOnlySpan<char> text)
		{
			var matches = Evaluator.Matches(text.ToString());

			if (matches.Count > 0 == ShouldHaveMatches)
			{
				return TextRuleResult.Failed();
			}

			return TextRuleResult.Passed();
		}
	}
}
