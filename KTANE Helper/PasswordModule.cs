using System.Speech.Recognition;
using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class PasswordModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;

		private static readonly string[] passwords =
		{
			"about", "after", "again", "below", "could",
			"every", "first", "found", "great", "house",
			"large", "learn", "never", "other", "place",
			"plant", "point", "right", "small", "sound",
			"spell", "still", "study", "their", "there",
			"these", "thing", "think", "three", "water",
			"where", "which", "world", "would", "write"
		};

		public static void Execute(string[] words)
		{
			Console.WriteLine("Starting password module...");

			using SpeechRecognitionEngine recognizer = new(new System.Globalization.CultureInfo("en-US"));
			recognizer.SetInputToDefaultAudioDevice();

			Choices choices = new("alpha", "bravo", "charlie", "delta", "echo", "foxtrot", "golf", "hotel", "india", "juliet", "kilo", "lima", "mike", "november", "oscar", "papa", "quebec", "romeo", "sierra", "tango", "uniform", "victor", "whiskey", "x-ray", "yankee", "zulu");
			recognizer.LoadGrammar(new Grammar(new GrammarBuilder(choices, 6, 6)));

			string[] possiblePasswords = passwords;
			HashSet<int> checkedColumns = new();
			for (int i = 0; i < 5; i++)
			{
				HashSet<char>[] passwordLetterOccurrances = new HashSet<char>[6];
				foreach (string password in possiblePasswords)
					for (int j = 0; j < 5; j++)
					{
						passwordLetterOccurrances[j] ??= new HashSet<char>();
						passwordLetterOccurrances[j].Add(password[j]);
					}

				int maxDifferentLetters = 0;
				int columnIndex = 0;
				for (int j = 0; j < 5; j++)
					if (passwordLetterOccurrances[j].Count > maxDifferentLetters && !checkedColumns.Contains(j))
					{
						maxDifferentLetters = passwordLetterOccurrances[j].Count;
						columnIndex = j;
					}

				checkedColumns.Add(columnIndex);
				Synthesizer.SpeakAsync($"Column {columnIndex + 1}");
				Console.Write($"Column {columnIndex + 1}: ");
				string column = recognizer.WaitUntilRecognized();
				Console.WriteLine(column);
				char[] columnLetters = column.Split(' ').Select(word => word[0]).ToArray();
				possiblePasswords = possiblePasswords.Where(p => columnLetters.Contains(p[columnIndex])).ToArray();
				if (possiblePasswords.Length == 0)
				{
					Synthesizer.SpeakAsync("Error invalid password");
					Console.WriteLine("Error: there is no password that matches the given letters");
					return;
				}
				if (possiblePasswords.Length == 1)
				{
					Synthesizer.SpeakAsync(possiblePasswords[0]);
					Console.WriteLine($"The password is {possiblePasswords[0]}");
					return;
				}
			}
			Synthesizer.SpeakAsync("Error invalid password");
			Console.WriteLine("Error: there is no password that matches the given letters");
		}
	}
}