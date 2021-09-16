using System.Collections.Generic;
using System.Diagnostics;

namespace TextConstraints.Tree
{
	/// <summary>
	/// A scoped projection inside a <see cref="WordTree"/>.
	/// </summary>
	[DebuggerTypeProxy(typeof(WordTreeSectionDebugView))]
	public readonly struct WordTreeSection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly uint nodeIndex;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public WordTree Tree { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public char Character => (char)Tree.nodes[nodeIndex].NodeCharacter;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsFullWord => Tree.nodes[nodeIndex].IsFullWord;

		public IEnumerable<WordTreeSection> ChildSections
		{
			get
			{
				var currentNode = Tree.nodes[nodeIndex];
				int size = currentNode.Size;
				if (nodeIndex == 0)
				{
					size = Tree.nodes.Length - 2;
				}

				uint currentOffset = 1;

				while (currentOffset <= size)
				{
					uint childNodeOffset = nodeIndex + currentOffset;
					var childNode = Tree.nodes[childNodeOffset];

					yield return new WordTreeSection(Tree, childNodeOffset);

					currentOffset += (uint)(childNode.Size + 1);
				}
			}
		}

		internal WordTreeSection(WordTree tree, uint nodeIndex)
		{
			Tree = tree;
			this.nodeIndex = nodeIndex;
		}

		public override string ToString()
		{
			return $"{Character}";
		}

		private class WordTreeSectionDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key,nq}")]
			internal struct DebuggerRow
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public WordTreeSection Value;
			}

			private readonly WordTreeSection source;

			public WordTreeSectionDebugView(WordTreeSection source)
			{
				this.source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					var rows = new List<DebuggerRow>();

					foreach (var section in source.ChildSections)
					{
						rows.Add(new DebuggerRow()
						{
							Key = section.Character.ToString(),
							Value = section
						});
					}
					return rows.ToArray();
				}
			}
		}
	}
}
