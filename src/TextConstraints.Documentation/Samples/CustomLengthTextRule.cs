using System;
using TextConstraints;

namespace RPGCore.Documentation.Samples
{
	#region length_rule
	public class CustomLengthTextRule : ITextRule
	{
		public int MinLength { get; }
		public int MaxLength { get; }

		public CustomLengthTextRule(int minLength, int maxLength)
		{
			MinLength = minLength;
			MaxLength = maxLength;
		}

		public TextRuleResult Evaluate(ReadOnlySpan<char> text)
		{
			// Test whether the input is valid here...

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
	#endregion length_rule

	public static class CustomLengthRulePolicy
	{
		public static void Run()
		{
			#region use_in_policy
			var policy = new TextPolicyBuilder("custom_policy")
				.UseTextRule(new CustomLengthTextRule(5, 10))
				.Build();
			#endregion use_in_policy
		}
	}
}
