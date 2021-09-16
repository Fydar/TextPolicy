namespace TextConstraints
{
	/// <summary>
	/// A structure representing the result of an <see cref="ITextRule"/> evaluation.
	/// </summary>
	public struct TextRuleResult
	{
		/// <summary>
		/// An enum representing the result of an <see cref="ITextRule"/> evaluation.
		/// </summary>
		public RuleStatus Status { get; }

		internal TextRuleResult(
			RuleStatus status)
		{
			Status = status;
		}

		/// <summary>
		/// Creates a <see cref="TextRuleResult"/> representing a passed evaluation of an <see cref="ITextRule"/>.
		/// </summary>
		/// <returns>A <see cref="TextRuleResult"/> representing a passed evaluation.</returns>
		public static TextRuleResult Passed()
		{
			return new TextRuleResult(RuleStatus.Passed);
		}

		/// <summary>
		/// Creates a <see cref="TextRuleResult"/> representing a failed evaluation of an <see cref="ITextRule"/>.
		/// </summary>
		/// <returns>A <see cref="TextRuleResult"/> representing a failed evaluation.</returns>
		public static TextRuleResult Failed()
		{
			return new TextRuleResult(RuleStatus.Failed);
		}
	}
}
