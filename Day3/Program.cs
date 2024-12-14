namespace Day3;

public static class Program
{
    private static int CustomIndexOf(this string input, int startIndex, bool handleDoDont, bool isEnabled)
    {
        if (handleDoDont)
        {
            if (isEnabled)
            {
                var mul = input.IndexOf("mul(", startIndex, StringComparison.Ordinal); 
                var dont = input.IndexOf("don't()", startIndex, StringComparison.Ordinal);
                if (mul == -1)
                    return dont;
                if (dont == -1)
                    return mul;
                return Math.Min(mul, dont);
            }
            else
            {
                return input.IndexOf("do()", startIndex, StringComparison.Ordinal);
            }
        }
        else
        {
            return input.IndexOf("mul(", startIndex, StringComparison.Ordinal);
        }
    }

    public static long SumOfNonCorruptedMulInstructions(string input, bool handleDoDont = false)
    {
        long sum = 0;
        var enabled = true;

        var startIndex = input.CustomIndexOf(0, handleDoDont, enabled);

        while (startIndex != -1)
        {
            if (handleDoDont && enabled && input.Substring(startIndex).StartsWith("don't()"))
            {
                enabled = false;
            }
            else if (handleDoDont && !enabled)
            {
                enabled = true;
            }
            else
            {
                var endIndex = input.IndexOf(")", startIndex, StringComparison.Ordinal);
                var mulInstruction = input.Substring(startIndex + 4, endIndex - startIndex - 4);
                if (!mulInstruction.Contains(" ") && mulInstruction.Count(c => c == ',') == 1)
                {
                    try
                    {
                        var firstNumber = int.Parse(mulInstruction.Split(',')[0]);
                        var secondNumber = int.Parse(mulInstruction.Split(',')[1]);
                        if (Math.Abs(firstNumber) < 1000 && Math.Abs(secondNumber) < 1000)
                        {
                            sum += firstNumber * secondNumber;
                        }
                    }
                    catch
                    {
                        // ignore exceptions
                    }
                }
            }

            startIndex = input.CustomIndexOf(startIndex + 1, handleDoDont, enabled);
        }

        return sum;
    }

    public static void Main(string[] args)
    {
        var input1 = File.ReadAllText("TestData.txt");
        var inputv2 = File.ReadAllText("TestDatav2.txt");
        var input2 = File.ReadAllText("TestData2.txt");

        Console.WriteLine("Sum of non-corrupted mul instructions");
        Console.WriteLine("Input 1: " + SumOfNonCorruptedMulInstructions(input1));
        Console.WriteLine("Input 2: " + SumOfNonCorruptedMulInstructions(input2));

        Console.WriteLine("Sum of non-corrupted mul instructions with do/don't handling");
        Console.WriteLine("Input 1v2: " + SumOfNonCorruptedMulInstructions(inputv2, true));
        Console.WriteLine("Input 2: " + SumOfNonCorruptedMulInstructions(input2, true));
    }
}