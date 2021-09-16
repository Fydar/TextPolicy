using System;
using System.IO;
using TextConstraints.Tree;

namespace RPGCore.Documentation.Samples
{
	public class WordTreeSample
	{
		public static void Run()
		{
			#region tree_lookup
			var tree = WordTree.ReadFromBytes(File.ReadAllBytes("output.bin"));

			var section = tree.GetSection("than");

			// Does this prefix exist in the word tree?
			if (section != null)
			{
				// Find all words that start with the prefix.
				foreach (string suggestion in WordTree.GetAllWordsFrom(section.Value))
				{
					Console.WriteLine(suggestion);
				}
			}
			#endregion tree_lookup
		}
	}
}
