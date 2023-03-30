using System.Speech.Recognition;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public static class SpeechRecognitionEngineExtensions
	{
		public static string WaitUntilRecognized(this SpeechRecognitionEngine recognizer)
		{
			while (true)
			{
				RecognitionResult result = recognizer.Recognize();
				if (result != null)
					return result.Text;
			}
		}
	}
}