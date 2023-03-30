using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class WiresModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;
		private static bool SerialEndsOdd => int.Parse(Program.Serial[5].ToString()) % 2 == 1;

		public static void Execute(string[] words)
		{
			if (string.IsNullOrEmpty(Program.Serial))
			{
				Synthesizer.SpeakAsync("Error serial");
				Console.WriteLine("Error: please first input the serial number");
				return;
			}
			switch (words.Length)
			{
				case 3:
					if (!words.Any(word => word == "red"))
						Result(2);
					else if (words[2] == "white")
						Result(3);
					else if (words.Count(word => word == "blue") > 1)
						for (int i = 3; i > 0; i--)
						{
							if (words[i - 1] != "blue")
								continue;
							Result(i);
							break;
						}
					else
						Result(3);
					break;
				case 4:
					if (words.Count(word => word == "red") > 1 && SerialEndsOdd)
						for (int i = 4; i > 0; i--)
						{
							if (words[i - 1] != "red")
								continue;
							Result(i);
							break;
						}
					else if (words[3] == "yellow" && !words.Any(word => word == "red"))
						Result(1);
					else if (words.Count(word => word == "blue") == 1)
						Result(1);
					else if (words.Count(word => word == "yellow") > 1)
						Result(4);
					else
						Result(2);
					break;
				case 5:
					if (words[4] == "black" && SerialEndsOdd)
						Result(4);
					else if (words.Count(word => word == "red") == 1 && words.Count(word => word == "yellow") > 1)
						Result(1);
					else if (!words.Any(word => word == "black"))
						Result(2);
					else
						Result(1);
					break;
				case 6:
					if (!words.Any(word => word == "yellow") && SerialEndsOdd)
						Result(3);
					else if (words.Count(word => word == "yellow") == 1 && words.Count(word => word == "white") > 1)
						Result(4);
					else if (!words.Any(word => word == "red"))
						Result(6);
					else
						Result(4);
					break;
			}
		}

		private static void Result(int wire)
		{
			Synthesizer.SpeakAsync($"{wire}");
			Console.WriteLine(wire switch
			{
				1 => "Cut the first wire",
				2 => "Cut the second wire",
				3 => "Cut the third wire",
				4 => "Cut the fourth wire",
				5 => "Cut the fifth wire",
				6 => "Cut the sixth wire",
				_ => throw new ArgumentOutOfRangeException(nameof(wire))
			});
		}
	}
}