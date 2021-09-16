using System;
using System.IO;
using System.Threading.Tasks;
using TextConstraints;
using TextConstraints.Tree;

namespace PasswordChecker
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("");
			Console.WriteLine(" ╔══ [PasswordChecker] ═════════════════════════════════════════════╗");
			Console.WriteLine(" ║ A console application that outputs bad passwords.                ║");
			Console.WriteLine(" ╚══════════════════════════════════════════════════════════════════╝");
			Console.WriteLine("");
			Console.ResetColor();


			var tree = WordTree.ReadFromBytes(File.ReadAllBytes("output.bin"));

			var policy = new TextPolicyBuilder("password_policy")
				.UseTextRule(new RequireCharactersTextRule(1, char.IsUpper))
				.UseTextRule(new RequireCharactersTextRule(1, char.IsNumber))
				.UseTextRule(new NotSingleWordTextRule(tree))
				.Build();

			var passwordsFile = new FileInfo("passwords.csv");
			using var reader = passwordsFile.OpenText();

			// Skip the first line...
			await reader.ReadLineAsync();

			while (!reader.EndOfStream)
			{
				string line = await reader.ReadLineAsync();

				var elements = line.Split(',');
				if (elements.Length < 4)
				{
					continue;
				}

				var passwordElement = elements[2].Trim();

				var result = policy.Evaluate(passwordElement);

				if (result.Status == RuleStatus.Failed)
				{
					Console.WriteLine($"The {elements[3]} user '{elements[0]}' password of '{passwordElement}' failed to pass the password policy.");
				}
			}
		}
	}
}
