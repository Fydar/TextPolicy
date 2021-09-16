using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextConstraints.Tree;

namespace TextConstraints.WordTrees.REPL
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("");
			Console.WriteLine(" ╔══ [TextConstraints.WordTrees.REPL] ══════════════════════════════╗");
			Console.WriteLine(" ║ Demo console application for testing word tree lookups.          ║");
			Console.WriteLine(" ║                                                                  ║");
			Console.WriteLine(" ║ Enter text and observe the suggested words being autocompleted.  ║");
			Console.WriteLine(" ║ Press [UP ARROW] and [DOWN ARROW] to navigate the options.       ║");
			Console.WriteLine(" ║ Press [TAB] or [RIGHT ARROW] to auto-complete word.              ║");
			Console.WriteLine(" ╚══════════════════════════════════════════════════════════════════╝");
			Console.WriteLine("");
			Console.Write(">");

			InteractiveLoop();
		}

		private static void InteractiveLoop()
		{
			var tree = WordTree.ReadFromBytes(File.ReadAllBytes("with-spaces.bin"));

			var sb = new StringBuilder();
			string currentInput = sb.ToString();
			var suggestions = new List<string>();
			int selectedSuggestion = 0;

			int startConsoleCursorLeft = Console.CursorLeft;
			int startConsoleCursorTop = Console.CursorTop;

			while (true)
			{
				void RebuildSuggestions()
				{
					var section = tree.GetSection(currentInput.ToLower());
					suggestions.Clear();
					selectedSuggestion = 0;
					if (section != null && !string.IsNullOrEmpty(currentInput))
					{
						int wordsCounted = 0;
						foreach (string suggestion in WordTree.GetAllWordsFrom(section.Value))
						{
							suggestions.Add(suggestion);
							wordsCounted++;

							if (wordsCounted > 999)
							{
								break;
							}
						}
					}
				}

				var input = Console.ReadKey();

				if (input.Key == ConsoleKey.Backspace)
				{
					if (sb.Length > 0)
					{
						sb.Remove(sb.Length - 1, 1);
						currentInput = sb.ToString();
						RebuildSuggestions();

						Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
						Console.Write(" ");
					}
				}
				else if (input.Key == ConsoleKey.Tab
					|| input.Key == ConsoleKey.RightArrow)
				{
					if (suggestions.Count > 0)
					{
						selectedSuggestion = Math.Clamp(selectedSuggestion, 0, suggestions.Count - 1);

						sb.Append(suggestions[selectedSuggestion]);
						currentInput = sb.ToString();
						RebuildSuggestions();
					}
				}
				else if (input.Key == ConsoleKey.UpArrow)
				{
					selectedSuggestion--;
				}
				else if (input.Key == ConsoleKey.DownArrow)
				{
					selectedSuggestion++;
				}
				else if (input.Key == ConsoleKey.Enter)
				{
					sb.Clear();
					currentInput = sb.ToString();
					RebuildSuggestions();
				}
				else if (char.IsLetterOrDigit(input.KeyChar)
					|| input.KeyChar == ' '
					|| input.KeyChar == '\''
					|| input.KeyChar == '.'
					|| input.KeyChar == '-')
				{
					sb.Append(input.KeyChar);
					currentInput = sb.ToString();
					RebuildSuggestions();
				}

				Console.SetCursorPosition(startConsoleCursorLeft, startConsoleCursorTop);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(sb);

				int returnConsoleCursorLeft = Console.CursorLeft;
				int returnConsoleCursorTop = Console.CursorTop;

				if (suggestions.Count > 0)
				{
					selectedSuggestion = Math.Clamp(selectedSuggestion, 0, suggestions.Count - 1);

					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write(suggestions[selectedSuggestion].PadRight(32));
				}
				else
				{
					Console.WriteLine("                                ");
				}

				Console.WriteLine("                                ");
				Console.WriteLine("                                ");

				DrawSuggestionsDialogue(currentInput, suggestions, selectedSuggestion);

				Console.SetCursorPosition(returnConsoleCursorLeft, returnConsoleCursorTop);
			}
		}

		private static void DrawSuggestionsDialogue(string currentInput, List<string> suggestions, int selectedSuggestion, int size = 10)
		{
			int wordsPrinted = 0;
			int startIndex = Math.Clamp(selectedSuggestion - (size / 2), 0, Math.Max(0, suggestions.Count - size - 1));

			for (int i = startIndex; i < suggestions.Count; i++)
			{
				string suggestion = suggestions[i];
				if (i == selectedSuggestion)
				{
					Console.Write(">");
				}
				else
				{
					Console.Write(" ");
				}
				Console.WriteLine((currentInput + suggestion).PadRight(32));
				wordsPrinted++;

				if (wordsPrinted > size)
				{
					break;
				}
			}
			for (int i = wordsPrinted; i <= size; i++)
			{
				Console.WriteLine("                                ");
			}
		}
	}
}
