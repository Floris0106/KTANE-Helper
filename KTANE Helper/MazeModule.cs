using System.Speech.Synthesis;

#pragma warning disable CA1416

namespace KTANEHelper
{
	public class MazeModule : IModule
	{
		private static SpeechSynthesizer Synthesizer => Program.Synthesizer;

		private static readonly Dictionary<(int, int), int> mazeIdentifiers = new()
		{
			{ (1, 6), 0 },
			{ (6, 1), 0 },
			{ (2, 5), 1 },
			{ (5, 2), 1 },
			{ (4, 6), 2 },
			{ (6, 4), 2 },
			{ (1, 1), 3 },
			{ (4, 5), 4 },
			{ (5, 4), 4 },
			{ (3, 5), 5 },
			{ (5, 3), 5 },
			{ (2, 2), 6 },
			{ (3, 4), 7 },
			{ (4, 3), 7 },
			{ (1, 3), 8 },
			{ (3, 1), 8 },
		};

		private static readonly string[][] mazes =
		{
			new string[] {
				"┏━┓┏━┥",
				"┃┏┛┗━┓",
				"┃┗┓┏━┫",
				"┃┝┻┛┝┫",
				"┣━┓┏┥┃",
				"┗┥┗┛┝┛",
			},
			new string[] {
				"┝┳┥┏┳┥",
				"┏┛┏┛┗┓",
				"┃┏┛┏━┫",
				"┣┛┏┛┰┃",
				"┃┰┃┏┛┃",
				"┸┗┛┗━┛",
			},
			new string[] {
				"┏━┓┰┏┓",
				"┸┰┃┗┛┃",
				"┏┫┃┏┓┃",
				"┃┃┃┃┃┃",
				"┃┗┛┃┃┃",
				"┗━━┛┗┛",
			},
			new string[] {
				"┏┓┝━━┓",
				"┃┃┏━━┫",
				"┃┗┛┏┥┃",
				"┃┝━┻━┫",
				"┣━━━┓┃",
				"┗━┥┝┛┸",
			},
			new string[] {
				"┝━━━┳┓",
				"┏━━┳┛┸",
				"┣┓┝┛┏┓",
				"┃┗━┓┸┃",
				"┃┏━┻┥┃",
				"┸┗━━━┛",
			},
			new string[] {
				"┰┏┓┝┳┓",
				"┃┃┃┏┛┃",
				"┣┛┸┃┏┛",
				"┗┓┏┫┃┰",
				"┏┛┸┃┗┫",
				"┗━━┛┝┛",
			},
			new string[] {
				"┏━━┓┏┓",
				"┃┏┥┗┛┃",
				"┗┛┏┥┏┛",
				"┏┓┣━┛┰",
				"┃┸┗━┓┃",
				"┗━━━┻┛",
			},
			new string[] {
				"┰┏━┓┏┓",
				"┣┻┥┗┛┃",
				"┃┏━━┓┃",
				"┃┗┓┝┻┛",
				"┃┰┗━━┥",
				"┗┻━━━┥",
			},
			new string[] {
				"┰┏━━┳┓",
				"┃┃┏┥┃┃",
				"┣┻┛┏┛┃",
				"┃┰┏┛┝┫",
				"┃┃┃┏┓┸",
				"┗┛┗┛┗┥",
			},
		};

		private static readonly Dictionary<char, Direction> possibleDirections = new()
		{
			{ '┃', Direction.Up | Direction.Down },
			{ '━', Direction.Left | Direction.Right },


			{ '┛', Direction.Up | Direction.Left },
			{ '┗', Direction.Up | Direction.Right },
			{ '┓', Direction.Down | Direction.Left },
			{ '┏', Direction.Down | Direction.Right },


			{ '┻', Direction.Up | Direction.Left | Direction.Right },
			{ '┳', Direction.Down | Direction.Left | Direction.Right },
			{ '┫', Direction.Up | Direction.Down | Direction.Left },
			{ '┣', Direction.Up | Direction.Down | Direction.Right },


			{ '┸', Direction.Up },
			{ '┰', Direction.Down },
			{ '┥', Direction.Left },
			{ '┝', Direction.Right }
		};

		public static void Execute(string[] words)
		{
			int column1 = int.Parse(words[0]);
			int column2 = int.Parse(words[1]);
			if (!mazeIdentifiers.TryGetValue((column1, column2), out int mazeId))
			{
				Synthesizer.Speak("Error invalid maze");
				Console.WriteLine("Error: there is no maze corresponding to the given markers");
				return;
			}
			string[] maze = mazes[mazeId];

			(int, int) position;
			(int, int) target;
			if (words[2] == "position")
			{
				position = (int.Parse(words[3]) - 1, int.Parse(words[4]) - 1);
				target = (int.Parse(words[6]) - 1, int.Parse(words[7]) - 1);
			}
			else
			{
				target = (int.Parse(words[3]) - 1, int.Parse(words[4]) - 1);
				position = (int.Parse(words[6]) - 1, int.Parse(words[7]) - 1);
			}

			Direction directions = possibleDirections[maze[position.Item2][position.Item1]];
			foreach (Direction newDirection in Enum.GetValues<Direction>())
				if (directions.HasFlag(newDirection))
				{
					Stack<Direction>? path = GoDirection(maze, position, newDirection, target);
					if (path != null)
					{
						Console.Write("The path to the target is:");
						while (path.TryPop(out Direction direction))
						{
							Console.Write(" " + direction.ToString().ToLower());
							Synthesizer.Speak(direction.ToString());
						}
						Console.WriteLine();
						return;
					}
				}
		}

		private static Stack<Direction>? GoDirection(string[] maze, (int, int) position, Direction direction, (int, int) target)
		{
			switch (direction)
			{
				case Direction.Up:
					position.Item2--;
					break;
				case Direction.Down:
					position.Item2++;
					break;
				case Direction.Left:
					position.Item1--;
					break;
				case Direction.Right:
					position.Item1++;
					break;
			}

			if (position == target)
				return new(new Direction[] { direction });

			Direction directions = possibleDirections[maze[position.Item2][position.Item1]];
			foreach (Direction newDirection in Enum.GetValues<Direction>())
				if (directions.HasFlag(newDirection) && newDirection != GetOppositeDirection(direction))
				{
					Stack<Direction>? path = GoDirection(maze, position, newDirection, target);
					if (path != null)
					{
						path.Push(direction);
						return path;
					}
				}

			return null;
		}

		private static Direction GetOppositeDirection(Direction direction)
		{
			return direction switch
			{
				Direction.Up => Direction.Down,
				Direction.Down => Direction.Up,
				Direction.Left => Direction.Right,
				Direction.Right => Direction.Left,
				_ => throw new ArgumentException("Invalid direction", nameof(direction))
			};
		}

		private enum Direction
		{
			Up = 1,
			Down = 2,
			Left = 4,
			Right = 8
		}
	}
}