using System;
using System.IO;
using System.Text;
using TextConstraints.Tree;

namespace TextConstraints.REPL
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("");
			Console.WriteLine(" ╔══ [TextConstraints.REPL] ════════════════════════════════════════╗");
			Console.WriteLine(" ║ Demo console application for testing text constraints.           ║");
			Console.WriteLine(" ║                                                                  ║");
			Console.WriteLine(" ║ Enter text and observe the highlighted colour of the password.   ║");
			Console.WriteLine(" ╚══════════════════════════════════════════════════════════════════╝");
			Console.WriteLine("");

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(" Password: ");

			InteractiveLoop();
		}

		private static void InteractiveLoop()
		{
			string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 !\"£$% ^&*() - _ = +[{]}#~'@;:/?.>,<\\|";
			var tree = WordTree.ReadFromBytes(File.ReadAllBytes("output.bin"));

			var charsetRule = new CharsetTextRule(charset);

			var policy = new TextPolicyBuilder("password_policy")
				.UseTextRule(new LengthTextRule(4, 32))
				.UseTextRule(charsetRule)
				.UseTextRule(new NotSingleWordTextRule(tree))
				.Build();

			var sb = new StringBuilder();

			int startConsoleCursorLeft = Console.CursorLeft;
			int startConsoleCursorTop = Console.CursorTop;

			while (true)
			{
				var input = Console.ReadKey();

				if (input.Key == ConsoleKey.Backspace)
				{
					if (sb.Length > 0)
					{
						sb.Remove(sb.Length - 1, 1);

						Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
						Console.Write(" ");
					}
				}
				else
				{
					var textResult = charsetRule.Evaluate(new char[] { input.KeyChar });
					if (textResult.Status == RuleStatus.Passed)
					{
						sb.Append(input.KeyChar);
					}
				}

				var result = policy.Evaluate(sb.ToString());

				if (result.Status == RuleStatus.Failed)
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Green;
				}

				Console.SetCursorPosition(startConsoleCursorLeft, startConsoleCursorTop);
				Console.Write(sb);

				int returnConsoleCursorLeft = Console.CursorLeft;
				int returnConsoleCursorTop = Console.CursorTop;

				Console.WriteLine();
				Console.WriteLine();

				foreach (var ruleResult in result.RuleResults)
				{
					DrawRuleResult(ruleResult);
				}

				Console.SetCursorPosition(returnConsoleCursorLeft, returnConsoleCursorTop);
			}
		}

		private static void DrawRuleResult(TextPolicyRuleResult ruleResult)
		{
			string ruleName;
			switch (ruleResult.Rule)
			{
				case LengthTextRule:
					ruleName = "Length";
					break;

				case CharsetTextRule:
					ruleName = "Charset";
					break;

				case NotSingleWordTextRule:
					ruleName = "Not a word";
					break;

				default:
					ruleName = "Unknown";
					break;
			}

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write($" {ruleName,-12} ");

			if (ruleResult.Status == RuleStatus.Failed)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Failed");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Passed");
			}
		}
	}
}
