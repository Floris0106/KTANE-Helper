using System.Speech.Synthesis;

namespace KTANEHelper
{
	public interface IModule
	{
		public static abstract void Execute(string[] words);
	}
}