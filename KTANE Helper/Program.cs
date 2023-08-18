using System.Speech.Recognition;
using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class Program
	{
		public static SpeechSynthesizer Synthesizer { get; } = new();

		public static string Serial { get; private set; } = "";
		public static int? Batteries { get; private set; } = null;
		public static HashSet<string>? Labels { get; private set; } = null;
		public static HashSet<string>? Ports { get; private set; } = null;

		private static void Main(string[] args)
		{
			Console.WriteLine("#########################################");
			Console.WriteLine("#                                       #");
			Console.WriteLine("#           KTANE Helper v1.2           #");
			Console.WriteLine("#    by Floris van Onna (Floris0106)    #");
			Console.WriteLine("#                                       #");
			Console.WriteLine("#########################################");
			Console.WriteLine();

			using SpeechRecognitionEngine recognizer = new(new System.Globalization.CultureInfo("en-US"));
			recognizer.SetInputToDefaultAudioDevice();
			Synthesizer.SetOutputToDefaultAudioDevice();
			Synthesizer.SelectVoice("Microsoft David Desktop");

			GrammarBuilder serialPhrase = new("Serial");
			Choices serialChoices = new("alpha", "bravo", "charlie", "delta", "echo", "foxtrot", "golf", "hotel", "india", "juliet", "kilo", "lima", "mike", "november", "oscar", "papa", "quebec", "romeo", "sierra", "tango", "uniform", "victor", "whiskey", "x-ray", "yankee", "zulu", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9");
			serialPhrase.Append(serialChoices, 6, 6);
			recognizer.LoadGrammar(new Grammar(serialPhrase));

			GrammarBuilder batteriesPhrase = new("Batteries");
			Choices batteriesChoices = new("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10");
			batteriesPhrase.Append(batteriesChoices);
			recognizer.LoadGrammar(new Grammar(batteriesPhrase));

			GrammarBuilder labelPhrase = new("Labels");
			Choices labelChoices = new("none", "snd", "clr", "car", "ind", "frq", "sig", "nsa", "msa", "trn", "bob", "frk");
			labelPhrase.Append(labelChoices);
			recognizer.LoadGrammar(new Grammar(labelPhrase));

			GrammarBuilder portPhrase = new("Ports");
			Choices portChoices = new("none", "dvi", "parallel", "ps2", "rj45", "serial", "stereo");
			portPhrase.Append(portChoices);
			recognizer.LoadGrammar(new Grammar(portPhrase));

			GrammarBuilder wiresPhrase = new("Wires");
			Choices wiresChoices = new("red", "white", "blue", "yellow", "black");
			wiresPhrase.Append(wiresChoices, 3, 6);
			recognizer.LoadGrammar(new Grammar(wiresPhrase));

			GrammarBuilder buttonPhrase = new("Button");
			Choices buttonColorChoices = new("blue", "white", "yellow", "red");
			buttonPhrase.Append(buttonColorChoices);
			Choices buttonWordChoices = new("abort", "detonate", "hold", "press");
			buttonPhrase.Append(buttonWordChoices);
			recognizer.LoadGrammar(new Grammar(buttonPhrase));

			GrammarBuilder keypadPhrase = new("Keypad");
			Choices keypadChoices = new(KeypadModule.Words);
			keypadPhrase.Append(keypadChoices, 4, 4);
			recognizer.LoadGrammar(new Grammar(keypadPhrase));

			GrammarBuilder memoryPhrase = new("Memory");
			recognizer.LoadGrammar(new Grammar(memoryPhrase));

			GrammarBuilder mazePhraze = new("Maze");
			Choices mazeChoices = new("1", "2", "3", "4", "5", "6");
			mazePhraze.Append(mazeChoices, 2, 2);
			GrammarBuilder positionFirstPhrase = new("position");
			positionFirstPhrase.Append(mazeChoices, 2, 2);
			positionFirstPhrase.Append("target");
			positionFirstPhrase.Append(mazeChoices, 2, 2);
			GrammarBuilder targetFirstPhrase = new("target");
			targetFirstPhrase.Append(mazeChoices, 2, 2);
			targetFirstPhrase.Append("position");
			targetFirstPhrase.Append(mazeChoices, 2, 2);
			Choices pathChoices = new(positionFirstPhrase, targetFirstPhrase);
			mazePhraze.Append(pathChoices);
			recognizer.LoadGrammar(new Grammar(mazePhraze));

			GrammarBuilder whosOnFirstPhrase = new("Who's on first");
			recognizer.LoadGrammar(new Grammar(whosOnFirstPhrase));

			GrammarBuilder passwordPhrase = new("Password");
			recognizer.LoadGrammar(new Grammar(passwordPhrase));

			GrammarBuilder morseCodePhrase = new("Morse code");
			Choices morseCodeChoices = new("shell", "halls", "slick", "trick", "boxes", "leaks", "strobe", "bistro", "flick", "bombs", "break", "brick", "steak", "sting", "vector", "beats");
			morseCodePhrase.Append(morseCodeChoices);
			recognizer.LoadGrammar(new Grammar(morseCodePhrase));

			GrammarBuilder complicatedWiresPhrase = new("Complicated wires");
			recognizer.LoadGrammar(new Grammar(complicatedWiresPhrase));

			GrammarBuilder wireSequencePhrase = new("Wire sequence");
			recognizer.LoadGrammar(new Grammar(wireSequencePhrase));

			GrammarBuilder resetPhrase = new("Reset");
			recognizer.LoadGrammar(new Grammar(resetPhrase));

			GrammarBuilder quitPhrase = new("Quit");
			recognizer.LoadGrammar(new Grammar(quitPhrase));
			
			while (true)
			{
				Console.Write('>');
				string command = recognizer.WaitUntilRecognized();
				Console.WriteLine(command);
				string[] words = command.Split(' ');
				switch (words[0])
				{
					case "Serial":
						Serial = words[1..].Select(word => word[0]).Aggregate("", (serial, letter) => serial + letter);
						Synthesizer.SpeakAsync($"Confirm serial {Serial}");
						Console.WriteLine($"Set serial number to {Serial.Aggregate("", (serial, symbol) => $"{serial} {symbol}")}");
						break;
					case "Batteries":
						Batteries = int.Parse(words[1]);
						Synthesizer.SpeakAsync($"Confirm batteries {Batteries}");
						Console.WriteLine($"Set number of batteries to {Batteries}");
						break;
					case "Labels":
						Labels ??= new();
						Labels.Add(words[1]);
						Synthesizer.SpeakAsync($"Confirm label {words[1]}");
						Console.WriteLine($"Added label: {words[1].ToUpper()}");
						break;
					case "Ports":
						Ports ??= new();
						Ports.Add(words[1]);
						Synthesizer.SpeakAsync($"Confirm port {words[1]}");
						Console.WriteLine($"Added port: {words[1]}");
						break;
					case "Wires":
						WiresModule.Execute(words[1..]);
						break;
					case "Button":
						ButtonModule.Execute(words[1..]);
						break;
					case "Keypad":
						KeypadModule.Execute(words[1..]);
						break;
					case "Memory":
						MemoryModule.Execute(Array.Empty<string>());
						break;
					case "Maze":
						MazeModule.Execute(words[1..]);
						break;
					case "Who's":
						if (words[1] == "on" && words[2] == "first")
							WhosOnFirstModule.Execute(Array.Empty<string>());
						break;
					case "Password":
						PasswordModule.Execute(Array.Empty<string>());
						break;
					case "Morse":
						if (words[1] == "code")
							MorseCodeModule.Execute(words[2..]);
						break;
					case "Complicated":
						if (words[1] == "wires")
							ComplicatedWiresModule.Execute(Array.Empty<string>());
						break;
					case "Wire":
						if (words[1] == "sequence")
							WireSequenceModule.Execute(Array.Empty<string>());
						break;
					case "Reset":
						Synthesizer.SpeakAsync("Confirm reset");
						Serial = "";
						Batteries = null;
						Labels = null;
						Ports = null;
						Console.Clear();
						Console.WriteLine("#########################################");
						Console.WriteLine("#                                       #");
						Console.WriteLine("#           KTANE Helper v1.2           #");
						Console.WriteLine("#    by Floris van Onna (Floris0106)    #");
						Console.WriteLine("#                                       #");
						Console.WriteLine("#########################################");
						Console.WriteLine();
						break;
					case "Quit":
						Console.WriteLine("Quitting helper...");
						Synthesizer.Dispose();
						return;
				}
			}
		}
	}
}
