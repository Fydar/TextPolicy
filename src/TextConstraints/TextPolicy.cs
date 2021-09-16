using System;
using System.Collections.Generic;

namespace TextConstraints
{
	/// <summary>
	/// A text policy constructed from a series of <see cref="ITextRule"/>s.
	/// </summary>
	public class TextPolicy
	{
		/// <summary>
		/// A unique identifier for this <see cref="TextPolicy"/>.
		/// </summary>
		public string PolicyIdentifier { get; }

		/// <summary>
		/// A collection of <see cref="ITextRule"/>s that make up this <see cref="TextPolicy"/>
		/// </summary>
		public IReadOnlyList<ITextRule> Rules { get; }

		internal TextPolicy(
			string policyIdentifier,
			IReadOnlyList<ITextRule> rules)
		{
			PolicyIdentifier = policyIdentifier;
			Rules = rules;
		}

		/// <summary>
		/// Evaluates this policy against a string.
		/// </summary>
		/// <param name="text">The text to evaluate against this <see cref="TextPolicy"/>.</param>
		/// <returns>A <see cref="TextPolicyResult"/> that describes the result of the evaluation.</returns>
		public TextPolicyResult Evaluate(ReadOnlySpan<char> text)
		{
			var results = new TextPolicyRuleResult[Rules.Count];
			for (int i = 0; i < Rules.Count; i++)
			{
				var rule = Rules[i];
				var ruleResult = rule.Evaluate(text);

				results[i] = new TextPolicyRuleResult(rule, ruleResult.Status);
			}

			return new TextPolicyResult(this, results);
		}
	}
}
