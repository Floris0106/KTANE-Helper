using System.Speech.Recognition;
using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class ComplicatedWiresModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;
		private static bool SerialEndsEven => int.Parse(Program.Serial[5].ToString()) % 2 == 0;

		private static int Batteries => Program.Batteries!.Value;
		private static HashSet<string> Ports => Program.Ports!;

		public static void Execute(string[] words)
		{
			if (string.IsNullOrEmpty(Program.Serial))
			{
				Synthesizer.SpeakAsync("Error serial");
				Console.WriteLine("Error: please first input the serial number");
				return;
			}
			if (!Program.Batteries.HasValue)
			{
				Synthesizer.SpeakAsync("Error batteries");
				Console.WriteLine("Error: please first input the number of batteries");
				return;
			}
			if (Program.Ports == null)
			{
				Synthesizer.SpeakAsync("Error ports");
				Console.WriteLine("Error: please first input the ports present on the bomb");
				return;
			}

			Console.WriteLine("Starting complicated wires...");
			Synthesizer.SpeakAsync("Starting complicated wires");

			using SpeechRecognitionEngine recognizer = new(new System.Globalization.CultureInfo("en-US"));
			recognizer.SetInputToDefaultAudioDevice();

			Choices choices = new("red", "blue", "star", "lit", "exit");
			GrammarBuilder phrase = new(choices, 1, 4);
			recognizer.LoadGrammar(new(phrase));

			while (true)
			{
				string wire = recognizer.WaitUntilRecognized();
				Console.WriteLine(wire);
				if (wire == "exit")
					break;

				int wireFlags = 0;
				if (wire.Contains("red"))
					wireFlags |= 0b0001;
				if (wire.Contains("blue"))
					wireFlags |= 0b0010;
				if (wire.Contains("star"))
					wireFlags |= 0b0100;
				if (wire.Contains("lit"))
					wireFlags |= 0b1000;

				switch (wireFlags)
				{
					case 0b0100:
					case 0b0101:
						Result(true);
						break;
					case 0b0110:
					case 0b1000:
					case 0b1111:
						Result(false);
						break;
					case 0b0001:
					case 0b0010:
					case 0b0011:
					case 0b1011:
						Result(SerialEndsEven);
						break;
					case 0b0111:
					case 0b1010:
					case 0b1110:
						Result(Ports.Contains("parallel"));
						break;
					case 0b1001:
					case 0b1100:
					case 0b1101:
						Result(Batteries >= 2);
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