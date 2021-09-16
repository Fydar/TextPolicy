using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextConstraints.Tree.Internal;

namespace TextConstraints.Tree
{
	/// <summary>
	/// A library of mechanisms used to construct <see cref="WordTree"/> data structures.
	/// </summary>
	public static class WordTreeFactory
	{
		/// <summary>
		/// Create a <see cref="WordTree"/> from a collection of <paramref name="words"/>.
		/// </summary>
		/// <param name="words">All words to be included in the word tree.</param>
		/// <returns>A <see cref="WordTree"/> containing all <paramref name="words"/> enumerated.</returns>
		public static async Task<WordTree> CreateFromWords(IAsyncEnumerable<string> words)
		{
			// Create an over-allocating tree of the dictionary.
			var context = new Stack<WordTreeBuilderNode>();
			var rootNode = new WordTreeBuilderNode(' ');
			context.Push(rootNode);

			await foreach (string word in words)
			{
				while (context.Count - 1 > word.Length)
				{
					context.Pop();
				}

				if (context.Count - 1 > 0)
				{
					int matched = 0;
					foreach (var toMatch in context.Reverse().Skip(1))
					{
						if (word[matched] == toMatch.NodeCharacter)
						{
							matched++;
						}
						else
						{
							break;
						}
					}

					int toPop = context.Count - 1 - matched;

					for (int i = 0; i < toPop; i++)
					{
						context.Pop();
					}
				}

				while (context.Count - 1 < word.Length)
				{
					var newNode = new WordTreeBuilderNode(word[context.Count - 1]);
					var addTo = context.Peek();
					addTo.Nodes.Add(newNode);
					context.Push(newNode);

					if (context.Count - 1 == word.Length)
					{
						newNode.IsFullWord = true;
					}
				}
			}

			var optimizedCollection = new List<WordTreeNode>(65536);
			BuildCollectionRecursive(optimizedCollection, rootNode);
			var lookup = optimizedCollection.ToArray();

			return new WordTree(lookup);
		}

		private static void BuildCollectionRecursive(List<WordTreeNode> output, WordTreeBuilderNode nodeBuilder)
		{
			output.Add(new WordTreeNode(
				(byte)nodeBuilder.NodeCharacter,
				nodeBuilder.InclusiveSize,
				nodeBuilder.IsFullWord));

			foreach (var childNode in nodeBuilder.Nodes)
			{
				BuildCollectionRecursive(output, childNode);
			}
		}

		private class WordTreeBuilderNode
		{
			public char NodeCharacter { get; set; }
			public List<WordTreeBuilderNode> Nodes { get; } = new List<WordTreeBuilderNode>();
			public bool IsFullWord { get; set; }

			public ushort InclusiveSize
			{
				get
				{
					static void CountRecursive(WordTreeBuilderNode subNode, ref ushort total)
					{
						foreach (var node in subNode.Nodes)
						{
							total++;
							CountRecursive(node, ref total);
						}
					}

					ushort total = 0;
					CountRecursive(this, ref total);
					return total;
				}
			}

			public WordTreeBuilderNode(char nodeCharacter)
			{
				NodeCharacter = nodeCharacter;
			}

			public override string ToString()
			{
				return $"{NodeCharacter} (+{Nodes.Count})";
			}
		}
	}
}
