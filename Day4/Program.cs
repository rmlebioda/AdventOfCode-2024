using System.Text;

namespace Day4;

public static class Program
{
    private enum Direction
    {
        South,
        East,
        North,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    }

    private const string Pattern = "XMAS";
    
    private static int CountPatternAppearance(string text)
    {
        var result = 0;
        var linesOfText = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        for (int y = 0; y < linesOfText.Length; y++)
        {
            var line = linesOfText[y];
            for (int x = 0; x < line.Length; x++)
            {
                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    if (DidPatternAppear(linesOfText, y, x, direction))
                        result++;
                }
            }
        }
        
        return result;
    }

    private static bool DidPatternAppear(string[] lines, int lineIndex, int characterIndex, Direction direction)
    {
        var currentX = characterIndex;
        var currentY = lineIndex;
        var currentCharacterIndex = 0;

        while (currentCharacterIndex < Pattern.Length)
        {
            if (currentX < 0 || currentY < 0 || currentY >= lines.Length || currentX >= lines[currentY].Length)
                return false;

            if (lines[currentY][currentX] != Pattern[currentCharacterIndex])
                return false;

            switch (direction)
            {
                case Direction.South:
                    currentY++;
                    break;
                case Direction.East:
                    currentX++;
                    break;
                case Direction.North:
                    currentY--;
                    break;
                case Direction.West:
                    currentX--;
                    break;
                case Direction.NorthEast:
                    currentY--;
                    currentX++;
                    break;
                case Direction.NorthWest:
                    currentY--;
                    currentX--;
                    break;
                case Direction.SouthEast:
                    currentY++;
                    currentX++;
                    break;
                case Direction.SouthWest:
                    currentY++;
                    currentX--;
                    break;
                default:
                    throw new NotImplementedException(direction.ToString());
            }

            currentCharacterIndex++;
        }

        // Console.WriteLine("Found at: " + lineIndex + ", " + characterIndex + " in direction: " + direction);

        return true;
    }

    private static int CountX_MasAppearance(string text)
    {
        var pattern = "M.S\n.A.\nM.S";
        var rotations = new int[] { 0, 1, 2, 3 };
        var linesOfText = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var result = 0;
        for (int y = 0; y < linesOfText.Length; y++)
        {
            var line = linesOfText[y];
            for (int x = 0; x < line.Length; x++)
            {
                foreach (var rotate in rotations)
                {
                    var newPattern = RotateClockwise(pattern, rotate);
                    if (DidMultilinePatternAppear(newPattern, linesOfText, y, x))
                    {
                        result++;
                    }
                }
            }
        }

        return result;
    }

    private static string RotateClockwise(string text, int count = 1)
    {
        var input = text
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.ToCharArray())
            .ToArray();
        
        var output = RotateClockwise<char>(input, count);

        return string.Join("\n", output
            .Select(arrayOfCharacters => new string(arrayOfCharacters)));
    }

    private static T[][] RotateClockwise<T>(T[][] array, int count = 1)
    {
        var newArray = new List<List<T>>();
        
        for (int i = 0; i < array.Length; i++)
            newArray.Add(new List<T>(array[i]));
        
        for (int iteration = 0; iteration < count; iteration++)
        {
            var arrayAfterRotation = new List<List<T>>();
            for (int x = 0;; x++)
            {
                var row = new List<T>();
                for (int y = newArray.Count - 1; y >= 0; y--)
                {
                    if (x >= newArray[y].Count)
                        break;
                    row.Add(newArray[y][x]);
                }
                if (row.Any())
                    arrayAfterRotation.Add(row);
                else
                    break;
            }
            newArray = arrayAfterRotation;
        }

        return newArray
            .Select(a => a.ToArray())
            .ToArray();
    }

    private static bool DidMultilinePatternAppear(string pattern, string[] lines, int lineIndex,
        int characterIndex)
    {
        var currentX = characterIndex;
        var currentY = lineIndex;
        var currentLineIndex = 0;
        var patternByLines = pattern.Split('\n');
        var patternSingleLineLength = patternByLines[0].Length;
        var patternLines = patternByLines.Length;
        
        while (currentLineIndex < patternSingleLineLength)
        {
            if (currentX < 0 || currentY < 0)
                return false;

            for (int i = 0; i < patternLines; i++)
            {
                if ((currentY + i) >= lines.Length || currentX >= lines[currentY + i].Length || (lines[currentY + i][currentX] != patternByLines[i][currentLineIndex]) && patternByLines[i][currentLineIndex] != '.')
                    return false;
            }

            currentX++;
            currentLineIndex++;
        }

        return true;
    }
    
    public static void Main(string[] args)
    {
        var input1 = File.ReadAllText("TestData.txt");
        var input2 = File.ReadAllText("TestData2.txt");

        Console.WriteLine($"{Pattern} appearance count:");
        Console.WriteLine("Input 1: " + CountPatternAppearance(input1));
        Console.WriteLine("Input 2: " + CountPatternAppearance(input2));
        
        Console.WriteLine($"X_Mas appearance count:");
        Console.WriteLine("Input 1: " + CountX_MasAppearance(input1));
        Console.WriteLine("Input 2: " + CountX_MasAppearance(input2));
    }
}
