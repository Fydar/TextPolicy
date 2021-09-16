using System.Runtime.InteropServices;

namespace TextConstraints.Tree.Internal
{
	[StructLayout(LayoutKind.Explicit, Size = 3)]
	internal readonly struct WordTreeNode
	{
		[FieldOffset(0)]
		private readonly byte nodeCharacter;

		[FieldOffset(1)]
		private readonly ushort data;

		public byte NodeCharacter => nodeCharacter;

		public bool IsFullWord => data >= 32768;

		public ushort Size
		{
			get
			{
				if (data >= 32768)
				{
					return (ushort)(data - 32768);
				}
				return data;
			}
		}

		internal WordTreeNode(byte nodeCharacter, ushort size, bool isFullWord)
		{
			this.nodeCharacter = nodeCharacter;
			data = size;
			if (isFullWord)
			{
				data += 32768;
			}
		}

		public override string ToString()
		{
			return $"{(char)nodeCharacter}{(IsFullWord ? "" : "...")} (+{Size})";
		}
	}
}
