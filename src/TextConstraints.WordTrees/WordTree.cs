using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using TextConstraints.Tree.Internal;

namespace TextConstraints.Tree
{
	/// <summary>
	/// A data structure for performing optimized word lookups in large data sets.
	/// </summary>
	public class WordTree
	{
		internal WordTreeNode[] nodes;

		/// <summary>
		/// A <see cref="WordTreeSection"/> positioned at the root of this <see cref="WordTree"/>.
		/// </summary>
		public WordTreeSection RootSection => new(this, 0);

		/// <summary>
		/// All words contained within this <see cref="WordTree"/>.
		/// </summary>
		public IEnumerable<string> Words => GetAllWordsFrom(RootSection);

		internal WordTree(WordTreeNode[] nodes)
		{
			this.nodes = nodes;
		}

		/// <summary>
		/// Writes this <see cref="WordTree"/> to a destination <see cref="Stream"/>.
		/// </summary>
		/// <param name="target">A <see cref="Stream"/> to write to.</param>
		public void WriteToStream(Stream target)
		{
			var byteSpan = MemoryMarshal.AsBytes(nodes.AsSpan());
			target.Write(byteSpan);
		}

		/// <summary>
		/// Reads a <see cref="WordTree"/> from supplied <paramref name="data"/>.
		/// </summary>
		/// <param name="data">Data to read a <see cref="WordTree"/> from.</param>
		/// <returns>A <see cref="WordTree"/> loaded from the <paramref name="data"/>.</returns>
		public static WordTree ReadFromBytes(byte[] data)
		{
			var nodesSpan = MemoryMarshal.Cast<byte, WordTreeNode>(data.AsSpan());

			return new WordTree(nodesSpan.ToArray());
		}

		/// <summary>
		/// Determines whether this <see cref="WordTree"/> contains a <paramref name="word"/>.
		/// </summary>
		/// <param name="word">The word to search within this <see cref="WordTree"/>.</param>
		/// <returns><c>true</c> if this <see cref="WordTree"/> contains the <paramref name="word"/>; otherwise <c>false</c>.</returns>
		public bool Contains(ReadOnlySpan<char> word)
		{
			var wordSection = GetSection(word);

			if (wordSection == null)
			{
				return false;
			}

			return wordSection.Value.IsFullWord;
		}

		public WordTreeSection? GetSection(ReadOnlySpan<char> word)
		{
			var currentSection = RootSection;

			foreach (char character in word)
			{
				bool found = false;
				foreach (var childSection in currentSection.ChildSections)
				{
					if (character == childSection.Character)
					{
						currentSection = childSection;
						found = true;
						break;
					}
				}

				if (!found)
				{
					return null;
				}
			}

			return currentSection;
		}

		/// <summary>
		/// Enumerates all remaining words from a section within this <see cref="WordTree"/>.
		/// </summary>
		/// <param name="section">The section within this <see cref="WordTree"/> to query from.</param>
		/// <returns>An enumerable of all words after the <paramref name="section"/>.</returns>
		public static IEnumerable<string> GetAllWordsFrom(WordTreeSection section)
		{
			var outputStack = new Stack<WordTreeSection>();
			foreach (string childOutput in OutputRecursive(outputStack, section))
			{
				yield return childOutput;
			}

			static IEnumerable<string> OutputRecursive(Stack<WordTreeSection> output, WordTreeSection current)
			{
				foreach (var child in current.ChildSections)
				{
					output.Push(child);

					if (child.IsFullWord)
					{
						yield return string.Join("", output.Reverse().Select(o => o.Character));
					}

					foreach (string childOutput in OutputRecursive(output, child))
					{
						yield return childOutput;
					}
					output.Pop();
				}
			}
		}
	}
}
