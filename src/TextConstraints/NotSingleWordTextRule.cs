using System;
using System.Text;
using TextConstraints.Tree;

namespace TextConstraints
{
	/// <summary>
	/// Ensures evaluated text does not consist fully of a single word (ignoring non-letter characters).
	/// </summary>
	public class NotSingleWordTextRule : ITextRule
	{
		/// <summary>
		/// A tree used to lookup invalid words.
		/// </summary>
		public WordTree Tree { get; }

		/// <summary>
		/// Constructs a new instance of the <see cref="NotSingleWordTextRule"/> class.
		/// </summary>
		/// <param name="tree">A tree used to lookup invalid words.</param>
		public NotSingleWordTextRule(WordTree tree)
		{
			Tree = tree;
		}

		/// <inheritdoc/>
		public TextRuleResult Evaluate(ReadOnlySpan<char> text)
		{
			string searchText = RemoveNonLettersAndLowercase(text);

			if (string.IsNullOrEmpty(searchText))
			{
				return TextRuleResult.Passed();
			}

			// Is the only word in the password in the dictionary
			if (Tree.Contains(searchText))
			{
				return TextRuleResult.Failed();
			}

			return TextRuleResult.Passed();
		}

		private static string RemoveNonLettersAndLowercase(ReadOnlySpan<char> text)
		{
			var sb = new StringBuilder();
			foreach (char c in text)
			{
				if (char.IsLetter(c))
				{
					sb.Append(char.ToLower(c));
				}
			}
			return sb.ToString();
		}
	}
}
