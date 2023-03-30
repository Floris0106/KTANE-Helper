using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class ButtonModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;

		private static int Batteries => Program.Batteries!.Value;
		private static HashSet<string> Labels => Program.Labels!;

		public static void Execute(string[] words)
		{
			if (!Program.Batteries.HasValue)
			{
				Synthesizer.SpeakAsync("Error batteries");
				Console.WriteLine("Error: please first input the number of batteries");
				return;
			}
			if (Program.Labels == null)
			{
				Synthesizer.SpeakAsync("Error labels");
				Console.WriteLine("Error: please first input the labels present on the bomb");
				return;
			}
			if (words[0] == "blue" && words[1] == "abort")
				Result(true);
			else if (Batteries > 1 && words[1] == "detonate")
				Result(false);
			else if (words[0] == "white" && Labels.Contains("car"))
				Result(true);
			else if (Batteries > 2 && Labels.Contains("frk"))
				Result(false);
			else if (words[0] == "yellow" )
				Result(true);
			else if (words[0] == "red" && words[1] == "hold")
				Result(false);
			else
				Result(true);
		}

		private static void Result(bool hold)
		{
			Synthesizer.SpeakAsync(hold ? "Hold" : "Press");
			Console.WriteLine(hold ? "Hold the button and release when the digit corresponding to the colored light is in any position of the timer" : "Press and immediately release the button");
		}
	}
}