using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TextConstraints.Generator.Services.DictionaryService
{
	public class OxfordDictionaryService : IDictionaryService
	{
		private static readonly char[] trimEndCharacters = new char[] { ' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '\'' };

		public async IAsyncEnumerable<string> GetWordsAsync(
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var sourceDictionaryFile = new FileInfo("dictionary.txt");

			using var reader = sourceDictionaryFile.OpenText();

			var allWords = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

			while (!reader.EndOfStream)
			{
				string line = await reader.ReadLineAsync();

				// Ignore blank lines.
				if (string.IsNullOrEmpty(line))
				{
					continue;
				}

				// Determine whether this line contains a word.
				int wordSeperatorIndex = line.IndexOf("  ");
				if (wordSeperatorIndex == -1)
				{
					continue;
				}

				// Isolate the word from the line.
				string word = line.Substring(0, wordSeperatorIndex);

				// Remove numbers from the end of the word.
				word = word.Trim(trimEndCharacters);

				if (string.IsNullOrEmpty(word))
				{
					// The word "Intestate" has two descriptions but the word is not repeated. Ignore lines like this.
					continue;
				}

				if (!allWords.Contains(word))
				{
					allWords.Add(word);
				}
			}

			foreach (string word in allWords)
			{
				yield return word;
			}
		}
	}
}
