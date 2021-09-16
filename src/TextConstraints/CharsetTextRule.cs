using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TextConstraints
{
	/// <summary>
	/// Ensures the evaluated text only contains characters from a charset.
	/// </summary>
	public class CharsetTextRule : ITextRule
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly HashSet<char> charset;

		/// <summary>
		/// The set of all valid characters.
		/// </summary>
		public IEnumerable<char> Charset => charset;

		/// <summary>
		/// Constructs a new instance of the <see cref="CharsetTextRule"/> class.
		/// </summary>
		/// <param name="charsetString">A string representing a set of all valid characters.</param>
		public CharsetTextRule(ReadOnlySpan<char> charsetString)
		{
			charset = new HashSet<char>();
			foreach (char c in charsetString)
			{
				charset.Add(c);
			}
		}

		/// <inheritdoc/>
		public TextRuleResult Evaluate(ReadOnlySpan<char> text)
		{
			var invalidChars = new HashSet<char>();

			foreach (char character in text)
			{
				if (!charset.Contains(character))
				{
					invalidChars.Add(character);
				}
			}

			if (invalidChars.Count > 0)
			{
				return TextRuleResult.Failed();
			}

			return TextRuleResult.Passed();
		}
	}
}
