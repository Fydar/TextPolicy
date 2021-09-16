namespace TextConstraints
{
	public class TextPolicyResult
	{
		public TextPolicy Policy { get; }
		public TextPolicyRuleResult[] RuleResults { get; }

		public RuleStatus Status
		{
			get
			{
				foreach (var result in RuleResults)
				{
					if (result.Status == RuleStatus.Failed)
					{
						return RuleStatus.Failed;
					}
				}
				return RuleStatus.Passed;
			}
		}

		internal TextPolicyResult(
			TextPolicy policy,
			TextPolicyRuleResult[] ruleResults)
		{
			Policy = policy;
			RuleResults = ruleResults;
		}
	}
}
