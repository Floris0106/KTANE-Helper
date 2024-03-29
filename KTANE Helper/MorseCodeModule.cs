﻿using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class MorseCodeModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;

		private static readonly Dictionary<string, string> frequencies = new()
		{
			{ "shell", "3.505" },
			{ "halls", "3.515" },
			{ "slick", "3.522" },
			{ "trick", "3.532" },
			{ "boxes", "3.535" },
			{ "leaks", "3.542" },
			{ "strobe", "3.545" },
			{ "bistro", "3.552" },
			{ "flick", "3.555" },
			{ "bombs", "3.565" },
			{ "break", "3.572" },
			{ "brick", "3.575" },
			{ "steak", "3.582" },
			{ "sting", "3.592" },
			{ "vector", "3.595" },
			{ "beats", "3.600" },
		};
		
		public static void Execute(string[] words)
		{
			if (!frequencies.TryGetValue(words[0], out string? frequency))
			{
				Synthesizer.SpeakAsync("Error invalid word");
				Console.WriteLine("Error: that word does not have an associated frequency");
				return;
			}
			Synthesizer.SpeakAsync(frequency);
			Console.WriteLine($"The frequency is {frequency} MHz");
		}
	}
}