namespace TextConstraints
{
	/// <summary>
	/// An enum describing the result of an <see cref="ITextRule"/> evaluation.
	/// </summary>
	public enum RuleStatus
	{
		/// <summary>
		/// Indicates that the text successfully passed the <see cref="ITextRule"/>.
		/// </summary>
		Passed,

		/// <summary>
		/// Indicates that the text failed to passed the <see cref="ITextRule"/>.
		/// </summary>
		Failed
	}
}
