namespace TextConstraints
{
	public struct TextPolicyRuleResult
	{
		public ITextRule Rule { get; }
		public RuleStatus Status { get; }

		internal TextPolicyRuleResult(
			ITextRule rule,
			RuleStatus status)
		{
			Rule = rule;
			Status = status;
		}
	}
}
