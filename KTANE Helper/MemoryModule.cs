using System.Speech.Recognition;
using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class MemoryModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;
		
		public static void Execute(string[] words)
		{
			Console.WriteLine("Starting memory...");
			Synthesizer.SpeakAsync("Starting memory");

			using SpeechRecognitionEngine recognizer = new(new System.Globalization.CultureInfo("en-US"));
			recognizer.SetInputToDefaultAudioDevice();

			Choices choices = new("1", "2", "3", "4");
			recognizer.LoadGrammar(new(choices));

			int[] labels = new int[4];
			int[] positions = new int[4];

			Console.Write("The first number on the display is: ");
			switch (int.Parse(recognizer.WaitUntilRecognized()))
			{
				case 1:
					Console.WriteLine("1");
					Synthesizer.SpeakAsync("Position 2");
					Console.WriteLine("Press the button in position 2");
					labels[0] = int.Parse(recognizer.WaitUntilRecognized());
					positions[0] = 2;
					break;
				case 2:
					Console.WriteLine("2");
					Synthesizer.SpeakAsync("Position 2");
					Console.WriteLine("Press the button in position 2");
					labels[0] = int.Parse(recognizer.WaitUntilRecognized());
					positions[0] = 2;
					break;
				case 3:
					Console.WriteLine("3");
					Synthesizer.SpeakAsync("Position 3");
					Console.WriteLine("Press the button in position 3");
					labels[0] = int.Parse(recognizer.WaitUntilRecognized());
					positions[0] = 3;
					break;
				case 4:
					Console.WriteLine("4");
					Synthesizer.SpeakAsync("Position 4");
					Console.WriteLine("Press the button in position 4");
					labels[0] = int.Parse(recognizer.WaitUntilRecognized());
					positions[0] = 4;
					break;
			}
			Console.Write("The second number on the display is: ");
			switch (int.Parse(recognizer.WaitUntilRecognized()))
			{
				case 1:
					Console.WriteLine("1");
					Synthesizer.SpeakAsync("Label 4");
					Console.WriteLine("Press the button with label 4");
					labels[1] = 4;
					positions[1] = int.Parse(recognizer.WaitUntilRecognized());
					break;
				case 2:
					Console.WriteLine("2");
					Synthesizer.SpeakAsync($"Position {positions[0]}");
					Console.WriteLine($"Press the button in position {positions[0]}");
					labels[1] = int.Parse(recognizer.WaitUntilRecognized());
					positions[1] = positions[0];
					break;
				case 3:
					Console.WriteLine("3");
					Synthesizer.SpeakAsync("Position 1");
					Console.WriteLine("Press the button in position 1");
					labels[1] = int.Parse(recognizer.WaitUntilRecognized());
					positions[1] = 1;
					break;
				case 4:
					Console.WriteLine("4");
					Synthesizer.SpeakAsync($"Position {positions[0]}");
					Console.WriteLine($"Press the button in position {positions[0]}");
					labels[1] = int.Parse(recognizer.WaitUntilRecognized());
					positions[1] = positions[0];
					break;
			}
			Console.Write("The third number on the display is: ");
			switch (int.Parse(recognizer.WaitUntilRecognized()))
			{
				case 1:
					Console.WriteLine("1");
					Synthesizer.SpeakAsync($"Label {labels[1]}");
					Console.WriteLine($"Press the button with label {labels[1]}");
					labels[2] = labels[1];
					positions[2] = int.Parse(recognizer.WaitUntilRecognized());
					break;
				case 2:
					Console.WriteLine("2");
					Synthesizer.SpeakAsync($"Label {labels[0]}");
					Console.WriteLine($"Press the button with label {labels[0]}");
					labels[2] = labels[0];
					positions[2] = int.Parse(recognizer.WaitUntilRecognized());
					break;
				case 3:
					Console.WriteLine("3");
					Synthesizer.SpeakAsync("Position 3");
					Console.WriteLine("Press the button in position 3");
					labels[2] = int.Parse(recognizer.WaitUntilRecognized());
					positions[2] = 3;
					break;
				case 4:
					Console.WriteLine("4");
					Synthesizer.SpeakAsync("Label 4");
					Console.WriteLine("Press the button with label 4");
					labels[2] = 4;
					positions[2] = int.Parse(recognizer.WaitUntilRecognized());
					break;
			}
			Console.Write("The fourth number on the display is: ");
			switch (int.Parse(recognizer.WaitUntilRecognized()))
			{
				case 1:
					Console.WriteLine("1");
					Synthesizer.SpeakAsync($"Position {positions[0]}");
					Console.WriteLine($"Press the button in position {positions[0]}");
					labels[3] = int.Parse(recognizer.WaitUntilRecognized());
					positions[3] = positions[0];
					break;
				case 2:
					Console.WriteLine("2");
					Synthesizer.SpeakAsync("Position 1");
					Console.WriteLine("Press the button in position 1");
					labels[3] = int.Parse(recognizer.WaitUntilRecognized());
					positions[3] = 1;
					break;
				case 3:
					Console.WriteLine("3");
					Synthesizer.SpeakAsync($"Position {positions[1]}");
					Console.WriteLine($"Press the button in position {positions[1]}");
					labels[3] = int.Parse(recognizer.WaitUntilRecognized());
					positions[3] = positions[1];
					break;
				case 4:
					Console.WriteLine("4");
					Synthesizer.SpeakAsync($"Position {positions[1]}");
					Console.WriteLine($"Press the button in position {positions[1]}");
					labels[3] = int.Parse(recognizer.WaitUntilRecognized());
					positions[3] = positions[1];
					break;
			}
			Console.Write("The fifth number on the display is: ");
			switch (int.Parse(recognizer.WaitUntilRecognized()))
			{
				case 1:
					Console.WriteLine("1");
					Synthesizer.SpeakAsync($"Label {labels[0]}");
					Console.WriteLine($"Press the button with label {labels[0]}");
					break;
				case 2:
					Console.WriteLine("2");
					Synthesizer.SpeakAsync($"Label {labels[1]}");
					Console.WriteLine($"Press the button with label {labels[1]}");
					break;
				case 3:
					Console.WriteLine("3");
					Synthesizer.SpeakAsync($"Label {labels[3]}");
					Console.WriteLine($"Press the button with label {labels[3]}");
					break;
				case 4:
					Console.WriteLine("4");
					Synthesizer.SpeakAsync($"Label {labels[2]}");
					Console.WriteLine($"Press the button with label {labels[2]}");
					break;
			}
		}
	}
}