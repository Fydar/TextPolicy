using System;
using System.IO;
using System.Text;
using TextConstraints.Tree;

namespace TextConstraints.REPL
{
	internal class Program
	{
		private const string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 !\"£$% ^&*() - _ = +[{]}#~'@;:/?.>,<\\|";

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
			var tree = WordTree.ReadFromBytes(File.ReadAllBytes("output.bin"));

			var charsetRule = new CharsetTextRule(charset);

			var policy = new TextPolicyBuilder("password_policy")
				.UseTextRule(new LengthTextRule(4, 32))
				.UseTextRule(charsetRule)
				.UseTextRule(new NotSingleWordTextRule(tree))
				.UseTextRule(new RequireCharactersTextRule(1, char.IsUpper))
				.UseTextRule(new RequireCharactersTextRule(1, char.IsNumber))
				.UseTextRule(new RequireCharactersTextRule(1, c => char.IsPunctuation(c) || char.IsSymbol(c)))
				.UseTextRule(new RequireCharactersTextRule(1, char.IsLower))
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

				case RequireCharactersTextRule requirement:

					var sb = new StringBuilder();
					sb.Append(requirement.MinAmount);
					sb.Append(" of ");

					int samples = 0;
					foreach (var character in charset)
					{
						var result = requirement.Evaluate(new char[] { character });
						if (result.Status == RuleStatus.Passed)
						{
							sb.Append(character);
							samples++;
						}
						if (samples > 5)
						{
							break;
						}
					}

					ruleName = sb.ToString();
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
