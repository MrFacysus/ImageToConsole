using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToAscii
{
	internal class Program
	{
		// define a list of characters
		private static readonly List<char> characters = new List<char>()
		{
			' ', '.', ',', ':', ';', '+', '*', '?', '%', 'S', '#', '@'
		};

		// define a list of colors
		private static readonly Dictionary<Color, ConsoleColor> colorMap = new Dictionary<Color, ConsoleColor>()
		{
			{ Color.Black, ConsoleColor.Black },
			{ Color.DarkBlue, ConsoleColor.DarkBlue },
			{ Color.DarkGreen, ConsoleColor.DarkGreen },
			{ Color.DarkCyan, ConsoleColor.DarkCyan },
			{ Color.DarkRed, ConsoleColor.DarkRed },
			{ Color.DarkMagenta, ConsoleColor.DarkMagenta },
			{ Color.Gray, ConsoleColor.Gray },
			{ Color.DarkGray, ConsoleColor.DarkGray },
			{ Color.Blue, ConsoleColor.Blue },
			{ Color.Green, ConsoleColor.Green },
			{ Color.Cyan, ConsoleColor.Cyan },
			{ Color.Red, ConsoleColor.Red },
			{ Color.Magenta, ConsoleColor.Magenta },
			{ Color.Yellow, ConsoleColor.Yellow },
			{ Color.White, ConsoleColor.White },
			{ Color.Brown, ConsoleColor.DarkYellow },
			{ Color.RosyBrown, ConsoleColor.DarkYellow },
			{ Color.SaddleBrown, ConsoleColor.DarkYellow },
			{ Color.SandyBrown, ConsoleColor.DarkYellow }
		};

		static void Main(string[] args)
		{
			try
			{
				// first args is the path to the image
				var path = args[0];
				// define console size
				var consoleWidth = Console.WindowWidth;
				var consoleHeight = Console.WindowHeight;
				// load image
				var image = new Bitmap(path);
				// resize image
				var resizedImage = new Bitmap(image, new Size(consoleWidth, consoleHeight));
				// convert image to ascii
				ImageToAscii(resizedImage);
				// wait for input
				Console.ReadLine();
			}
			catch (Exception ex)
			{
				throw new Exception("Please drag an Image on the exe file! (" + ex.Message + ")");
			}
		}

		private static void ImageToAscii(Bitmap resizedImage)
		{
			// loop through each pixel
			for (int y = 0; y < resizedImage.Height; y++)
			{
				for (int x = 0; x < resizedImage.Width; x++)
				{
					// get the pixel
					Color pixel = resizedImage.GetPixel(x, y);
					// if the pixel is transparent
					if (pixel.A == 0)
					{
						// console write a space
						Console.Write(" ");
						// continue to the next pixel
						continue;
					}
					// get the brightness of the pixel
					var brightness = pixel.GetBrightness();
					// convert both to a character
					var character = GetCharacter(pixel, brightness);
					// console write the character
					Console.Write(character);
				}
				// console write a new line
				Console.Write("\n");
			}
		}

		private static string GetCharacter(Color color, float brightness)
		{
			// set foreground color to what the map says
			try
			{
				Console.ForegroundColor = getClosestColor(color);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + " " + color);
				Console.Read();
				throw new Exception("Color not found! (" + ex.Message + ")");
			}
			// get the index of the character
			var index = (int)Math.Floor(brightness * characters.Count);
			// return the character
			return characters[index].ToString();
		}

		private static ConsoleColor getClosestColor(Color color)
		{
			// get the closest color
			var closestColor = colorMap.OrderBy(x => colorDiff(x.Key, color)).First().Value;
			// return the closest color
			return closestColor;
		}

		private static object colorDiff(Color key, Color color)
		{
			// get the difference between the colors
			var diff = Math.Abs(key.R - color.R) + Math.Abs(key.G - color.G) + Math.Abs(key.B - color.B);
			// return the difference
			return diff;
		}
	}
}
