using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class KeypadModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;
		
		private static readonly string[][] sequences =
		{
			new string[] { "weird q", "weird a", "lambda", "lightning", "creature", "weird h", "backwards c" },
			new string[] { "backwards euro", "weird q", "backwards c", "squiggle", "empty star", "weird h", "upside down question mark" },
			new string[] { "copyright", "tooth", "squiggle", "double k", "incomplete 3", "lambda", "empty star" },
			new string[] { "weird 6", "paragraph", "weird b", "creature", "double k", "upside down question mark", "smiley" },
			new string[] { "psi", "smiley", "weird b", "weird c", "paragraph", "weird 3", "full star" },
			new string[] { "weird 6", "backwards euro", "puzzle piece", "a e", "psi", "backwards n", "omega" },
		};

		private static readonly Dictionary<string, string> aliases = new()
		{
			{ "weird q", "weird q" },
			{ "rotated q", "weird q" },
			{ "racket", "weird q" },
			{ "weird a", "weird a" },
			{ "a with extra line", "weird a" },
			{ "a with an extra line", "weird a" },
			{ "lambda", "lambda" },
			{ "weird lambda", "lambda" },
			{ "lambda with extra dash", "lambda" },
			{ "lambda with an extra dash", "lambda" },
			{ "lightning", "lightning" },
			{ "lightning bolt", "lightning" },
			{ "creature", "creature" },
			{ "spider", "creature" },
			{ "weird spider", "creature" },
			{ "cat", "creature" },
			{ "weird cat", "creature" },
			{ "weird h", "weird h" },
			{ "h with comma", "weird h" },
			{ "h with a comma", "weird h" },
			{ "backwards c", "backwards c" },
			{ "backwards c with dot", "backwards c" },
			{ "backwards c with a dot", "backwards c" },
			{ "backwards euro", "backwards euro" },
			{ "weird euro", "backwards euro" },
			{ "squiggle", "squiggle" },
			{ "weird o", "squiggle" },
			{ "empty star", "empty star" },
			{ "upside down question mark", "upside down question mark" },
			{ "copyright", "copyright" },
			{ "tooth", "tooth" },
			{ "weird tooth", "tooth" },
			{ "double k", "double k" },
			{ "weird k", "double k" },
			{ "backwards k", "double k" },
			{ "mirrored k", "double k" },
			{ "incomplete 3", "incomplete 3" },
			{ "unfinished 3", "incomplete 3" },
			{ "weird 6", "weird 6" },
			{ "paragraph", "paragraph" },
			{ "backwards p", "paragraph" },
			{ "weird b", "weird b" },
			{ "smiley", "smiley" },
			{ "psi", "psi" },
			{ "trident", "psi" },
			{ "weird c", "weird c" },
			{ "c with dot", "weird c" },
			{ "c with a dot", "weird c" },
			{ "weird 3", "weird 3" },
			{ "3 with tail", "weird 3" },
			{ "3 with a tail", "weird 3" },
			{ "full star", "full star" },
			{ "filled in star", "full star" },
			{ "puzzle piece", "puzzle piece" },
			{ "a e", "a e" },
			{ "a with e", "a e" },
			{ "a with an e", "a e" },
			{ "backwards n", "backwards n" },
			{ "capital n", "backwards n" },
			{ "backwards capital n", "backwards n" },
			{ "omega", "omega" },
		};

		public static string[] Words => aliases.Keys.ToArray();

		public static void Execute(string[] words)
		{
			string[] symbols = new string[4];
			string keypad = words.Aggregate((current, word) => current + " " + word);
			for (int i = 0; i < 4; i++)
			{
				foreach (KeyValuePair<string, string> alias in aliases)
					if (keypad.StartsWith(alias.Key))
					{
						symbols[i] = alias.Value;
						keypad = keypad[alias.Key.Length..].TrimStart();
						break;
					}
			}

			foreach (string[] sequence in sequences)
				if (symbols.All(symbol => sequence.Contains(symbol)))
				{
					Console.WriteLine("Input the symbols in the following order:");
					foreach (string symbol in sequence)
						if (symbols.Contains(symbol))
						{
							Console.WriteLine(" " + symbol);
							Synthesizer.Speak(symbol);
						}
					return;
				}
			Synthesizer.Speak("Error no sequence");
			Console.WriteLine("Error: no sequence with these symbols exists");
		}
	}
}