using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TextConstraints.Generator.Services.DictionaryService;
using TextConstraints.Tree;

namespace TextConstraints.Generator
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("");
			Console.WriteLine(" ╔══ [TextConstraints.Generator] ═══════════════════════════════════╗");
			Console.WriteLine(" ║ Console application for generating optimized word tree files.    ║");
			Console.WriteLine(" ╚══════════════════════════════════════════════════════════════════╝");
			Console.WriteLine("");

			Console.ResetColor();

			var dictionary = new OxfordDictionaryService();

			var filteredWords = ModifyStringsForPasswordsAsync(dictionary.GetWordsAsync());
			var toLowerWords = ToLowerStringsAsync(dictionary.GetWordsAsync());

			await GenerateWordTree(filteredWords, "output.bin");
			await GenerateWordTree(toLowerWords, "with-spaces.bin");
		}

		private static async Task GenerateWordTree(IAsyncEnumerable<string> words, string outputPath)
		{
			var tree = await WordTreeFactory.CreateFromWords(words);

			int wordsCount = 0;
			foreach (string word in tree.Words)
			{
				wordsCount++;
			}

			Console.WriteLine($"Created word tree of {wordsCount:###,##0} words.");

			var outputFile = new FileInfo(outputPath);
			if (outputFile.Exists)
			{
				outputFile.Delete();
			}
			using (var writer = outputFile.OpenWrite())
			{
				tree.WriteToStream(writer);
			}

			Console.WriteLine($"Saved to '{outputFile.FullName}'.");
		}

		private static async IAsyncEnumerable<string> ToLowerStringsAsync(IAsyncEnumerable<string> source)
		{
			int sizeThreshold = 2;

			await foreach (string word in source)
			{
				// Ignore words that are too short to be valid passwords.
				// Are these even words?
				if (word.Length < sizeThreshold)
				{
					continue;
				}

				// Normalise all words to lowercase.
				string modifiedWord = word.ToLower();

				yield return modifiedWord;
			}
		}

		private static async IAsyncEnumerable<string> ModifyStringsForPasswordsAsync(IAsyncEnumerable<string> source)
		{
			int sizeThreshold = 2;

			await foreach (string word in source)
			{
				// Ignore words that are too short to be valid passwords.
				// Are these even words?
				if (word.Length < sizeThreshold)
				{
					continue;
				}

				// Normalise all words to lowercase.
				string modifiedWord = word.ToLower();

				// Remove spaces from words.
				modifiedWord = modifiedWord.Replace(" ", "");

				yield return modifiedWord;
			}
		}
	}
}
