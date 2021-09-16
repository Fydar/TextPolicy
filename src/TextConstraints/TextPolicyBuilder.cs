using System.Collections.Generic;

namespace TextConstraints
{
	/// <summary>
	/// A builder for constructing instances of the <see cref="TextPolicy"/> class.
	/// </summary>
	public class TextPolicyBuilder
	{
		private readonly List<ITextRule> textRules = new();

		/// <summary>
		/// A unique identifier for the constructed <see cref="TextPolicy"/>.
		/// </summary>
		public string PolicyIdentifier { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="TextPolicyBuilder"/> class.
		/// </summary>
		/// <param name="policyIdentifier">A unique identifier for the constructed <see cref="TextPolicy"/>.</param>
		public TextPolicyBuilder(
			string policyIdentifier)
		{
			PolicyIdentifier = policyIdentifier;
		}

		/// <summary>
		/// Adds a new rule to the constructed <see cref="TextPolicy"/>.
		/// </summary>
		/// <param name="textRule">A new rule to add to the constructed <see cref="TextPolicy"/>.</param>
		/// <returns>The current instance of this <see cref="TextPolicyBuilder"/>.</returns>
		public TextPolicyBuilder UseTextRule(ITextRule textRule)
		{
			textRules.Add(textRule);

			return this;
		}

		/// <summary>
		/// Creates a new <see cref="TextPolicy"/> constructed instance from the current state of this <see cref="TextPolicyBuilder"/>.
		/// </summary>
		/// <returns>A <see cref="TextPolicy"/> constructed from current state of this <see cref="TextPolicyBuilder"/>.</returns>
		public TextPolicy Build()
		{
			return new TextPolicy(PolicyIdentifier, textRules);
		}
	}
}
