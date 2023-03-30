using System.Speech.Recognition;
using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class WireSequenceModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;

		private static string[] redCutList =
		{
			"c",
			"b",
			"a",
			"ac",
			"b",
			"ac",
			"abc",
			"ab",
			"b"
		};
		private static string[] blueCutList =
		{
			"b",
			"ac",
			"b",
			"a",
			"b",
			"bc",
			"c",
			"ac",
			"a"
		};
		private static string[] blackCutList =
		{
			"abc",
			"ac",
			"b",
			"ac",
			"b",
			"bc",
			"ab",
			"c",
			"c"
		};

		public static void Execute(string[] words)
		{
			Console.WriteLine("Starting wire sequence...");
			Synthesizer.SpeakAsync("Starting wire sequence");

			using SpeechRecognitionEngine recognizer = new(new System.Globalization.CultureInfo("en-US"));
			recognizer.SetInputToDefaultAudioDevice();
			
			Choices colorChoices = new("red", "blue", "black");
			GrammarBuilder wirePhrase = new(colorChoices);
			wirePhrase.Append("to");
			Choices letterChoices = new("alpha", "bravo", "charlie");
			wirePhrase.Append(letterChoices);
			recognizer.LoadGrammar(new(wirePhrase));

			GrammarBuilder resetPhrase = new("reset");
			recognizer.LoadGrammar(new(resetPhrase));

			GrammarBuilder exitPhrase = new("exit");
			recognizer.LoadGrammar(new(exitPhrase));

			int red = 0, blue = 0, black = 0;
			while (true)
			{
				string wire = recognizer.WaitUntilRecognized();
				Console.WriteLine(wire);
				if (wire == "reset")
				{
					red = blue = black = 0;
					Synthesizer.SpeakAsync("Confirm reset");
					continue;
				}
				
				if (wire == "exit")
					break;

				words = wire.Split(' ');
				switch (words[0])
				{
					case "red":
						Result(redCutList[red++].Contains(words[2][0]));
						break;
					case "blue":
						Result(blueCutList[blue++].Contains(words[2][0]));
						break;
					case "black":
						Result(blackCutList[black++].Contains(words[2][0]));
						break;
				}
			}
		}

		private static void Result(bool cut)
		{
			Synthesizer.SpeakAsync(cut ? "Cut" : "Don't cut");
			Console.WriteLine(cut ? "Cut the wire" : "Do not cut the wire");
		}
	}
}