using System.Speech.Recognition;
using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class WhosOnFirstModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;

		private static readonly Dictionary<string, string> sequences = new()
		{
			{ "ready", "yes, okay, what, middle, left, press, right, blank, ready" },
			{ "first", "left, okay, yes, middle, no, right, nothing, letters u h h h, wait, ready, blank, what, press, first" },
			{ "no", "blank, letters u h h h, wait, first, what, ready, right, yes, nothing, left, press, okay, no" },
			{ "blank", "wait, right, okay, middle, blank" },
			{ "nothing", "letters u h h h, right, okay, middle, yes, blank, no, press, left, what, wait, first, nothing" },
			{ "yes", "okay, right, letters u h h h, middle, first, what, press, ready, nothing, yes" },
			{ "what", "letters u h h h, what" },
			{ "letters u h h h", "ready, nothing, left, what, okay, yes, right, no, press, blank, letters u h h h" },
			{ "left", "right, left" },
			{ "right", "yes, nothing, ready, press, no, wait, what, right" },
			{ "middle", "blank, ready, okay, what, nothing, press, no, wait, left, middle" },
			{ "okay", "middle, no, first, yes, letters u h h h, nothing, wait, okay" },
			{ "wait", "letters u h h h, no, blank, okay, yes, left, first, press, what, wait" },
			{ "press", "right, middle, yes, ready, press" },
			{ "you", "sure, you are, your, you apostrophe are, next, letters u h h u h, letters u r, hold, what question mark, you" },
			{ "you are", "your, next, like, letters u h h u h, what question mark, done, letters u h u h, hold, you, letter u, you apostrophe are, sure, letters u r, you are" },
			{ "your", "letters u h u h, you are, letters u h h u h, your" },
			{ "you apostrophe are", "you, you apostrophe are" },
			{ "letters u r", "done, letter u, letters u r" },
			{ "letter u", "letters u h h u h, sure, next, what question mark, you apostrophe are, letters u r, letters u h u h, done, letter u" },
			{ "letters u h h u h", "letters u h h u h" },
			{ "letters u h u h", "letters u r, letter u, you are, you apostrophe are, next, letters u h u h" },
			{ "what question mark", "you, hold, you apostrophe are, your, letter u, done, letters u h u h, like, you are, letters u h h u h, letters u r, next, what question mark" },
			{ "done", "sure, letters u h h u h, next, what question mark, your, letters u r, you apostrophe are, HOLD, like, you, letter u, you are, letters u h u h, done" },
			{ "next", "what question mark, letters u h h u h, letters u h u h, your, hold, sure, next" },
			{ "hold", "you are, letter u, done, letters u h u h, you, letters u r, sure, what question mark, you apostrophe are, next, hold" },
			{ "sure", "you are, done, like, you apostrophe are, you, hold, letters u h h u h, letters u r, sure" },
			{ "like", "you apostrophe are, next, letter u, letters u r, hold, done, letters u h u h, what question mark, letters u h h u h, you, like" },
		};

		private static readonly Choices choices = new
		(
			"ready",
			"first",
			"no",
			"blank",
			"nothing",
			"yes",
			"what",
			"letters u h h h",
			"left",
			"right",
			"middle",
			"okay",
			"wait",
			"press",
			"you",
			"you are",
			"your",
			"you apostrophe are",
			"letters u r",
			"letter u",
			"letters u h h u h",
			"letters u h u h",
			"what question mark",
			"done",
			"next",
			"hold",
			"sure",
			"like",
			"letter c",
			"word nothing",
			"letters l e d",
			"they are",
			"word blank",
			"letters r e a d",
			"letters r e d",
			"posessive their",
			"letters r e e d",
			"letters l e e d",
			"they apostrophe are",
			"display",
			"says",
			"letters l e a d",
			"hold on",
			"location there",
			"see",
			"letters c e e",
			"exit"
		);

		public static void Execute(string[] words)
		{
			Console.WriteLine("Starting who's on first...");
			Synthesizer.SpeakAsync("Starting who's on first");

			using SpeechRecognitionEngine recognizer = new(new System.Globalization.CultureInfo("en-US"));
			recognizer.SetInputToDefaultAudioDevice();

			recognizer.LoadGrammar(new Grammar(choices));

			while (true)
			{
				string display = recognizer.WaitUntilRecognized();
				Console.WriteLine($"The display reads: {display}");
				switch (display)
				{
					case "letters u r":
						Synthesizer.SpeakAsync("Top left");
						Console.WriteLine("Read the top left button");
						break;
					case "first":
					case "okay":
					case "letter c":
						Synthesizer.SpeakAsync("Top right");
						Console.WriteLine("Read the top right button");
						break;
					case "yes":
					case "word nothing":
					case "letters l e d":
					case "they are":
						Synthesizer.SpeakAsync("Middle left");
						Console.WriteLine("Read the middle left button");
						break;
					case "word blank":
					case "letters r e a d":
					case "letters r e d":
					case "you":
					case "your":
					case "you apostrophe are":
					case "posessive their":
						Synthesizer.SpeakAsync("Middle right");
						Console.WriteLine("Read the middle right button");
						break;
					case "nothing":
					case "blank":
					case "letters r e e d":
					case "letters l e e d":
					case "they apostrophe are":
						Synthesizer.SpeakAsync("Bottom left");
						Console.WriteLine("Read the bottom left button");
						break;
					case "display":
					case "says":
					case "no":
					case "letters l e a d":
					case "hold on":
					case "you are":
					case "location there":
					case "see":
					case "letters c e e":
						Synthesizer.SpeakAsync("Bottom right");
						Console.WriteLine("Read the bottom right button");
						break;
					case "exit":
						return;
				}

				if (!sequences.TryGetValue(recognizer.WaitUntilRecognized(), out string? sequence))
				{
					Synthesizer.SpeakAsync("Error invalid sequence");
					Console.WriteLine("Error: that word does not have a sequence");
					continue;
				}
				Synthesizer.SpeakAsync(sequence);
				Console.WriteLine($"The sequence is: {sequence}");

				for (string? word = ""; word != "next"; word = recognizer.Recognize()?.Text)
					if (word == "exit")
					{
						Synthesizer.SpeakAsyncCancelAll();
						Console.WriteLine("Exiting who's on first...");
						return;
					}
				Synthesizer.SpeakAsyncCancelAll();
				Console.WriteLine("Proceeding to next stage...");
			}
		}
	}
}