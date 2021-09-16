using System;
using System.IO;
using TextConstraints;
using TextConstraints.Tree;

namespace RPGCore.Documentation.Samples
{
	public class TextPolicySample
	{
		public static void Run()
		{
			#region policy
			#region create_policy
			var tree = WordTree.ReadFromBytes(File.ReadAllBytes("output.bin"));

			var policy = new TextPolicyBuilder("password_policy")
				.UseTextRule(new LengthTextRule(4, 32))
				.UseTextRule(new CharsetTextRule("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"))
				.UseTextRule(new NotSingleWordTextRule(tree))
				.Build();
			#endregion create_policy

			#region evaluate_policy
			var result = policy.Evaluate("Password!23");

			if (result.Status == RuleStatus.Passed)
			{
				Console.WriteLine("Passed!");
			}
			else
			{
				Console.WriteLine("Failed!");
			}
			#endregion evaluate_policy
			#endregion policy
		}
	}
}
