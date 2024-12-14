namespace Day2;

public static class Program
{
    public static List<List<int>> ParseInput(string input)
    {
        var content = File.ReadAllLines(input);
        return content.Select(line => line.Split(" ").Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(int.Parse).ToList()).ToList();
    }

    public static long GetSafeReports(List<List<int>> input)
    {
        long safeLevel = 0;
        foreach (var report in input.Where(l => l.Count != 0))
        {
            var isSafe = true;
            for (int i = 1; i < report.Count(); i++)
            {
                var previousNumber = report[i - 1];
                var number = report[i];
                if (Math.Abs(number - previousNumber) < 1 || Math.Abs(number - previousNumber) > 3)
                {
                    isSafe = false;
                    break;
                }

                if (i > 1)
                {
                    var secondPreviousNumber = report[i - 2];
                    var lastDifference = number - previousNumber;
                    var secondLastDifference = previousNumber - secondPreviousNumber;
                    if ((lastDifference < 0 && secondLastDifference > 0) ||
                        (lastDifference > 0 && secondLastDifference < 0))
                    {
                        isSafe = false;
                        break;
                    }
                }
            }

            if (isSafe)
                safeLevel++;
        }

        return safeLevel;
    }

    public static long GetSafeReports(List<List<int>> input, int badInputTolerance)
    {
        long safeLevel = 0;
        var newInput = input.Where(l => l.Count != 0).ToList();
        foreach (var report in newInput)
        {
            var combinationList = badInputTolerance == 0
                ? new List<List<int>>(){ report }
                : report.DifferentCombinations(report.Count() - badInputTolerance).Select(s => s.ToList()).ToList();

            var didFindSafe = false;
            foreach (var combination in combinationList)
            {
                var isSafe = true;
                
                for (int i = 1; i < combination.Count(); i++)
                {
                    var previousNumber = combination[i - 1];
                    var number = combination[i];
                    if (Math.Abs(number - previousNumber) < 1 || Math.Abs(number - previousNumber) > 3)
                    {
                        isSafe = false;
                        break;
                    }

                    if (i > 1)
                    {
                        var secondPreviousNumber = combination[i - 2];
                        var lastDifference = number - previousNumber;
                        var secondLastDifference = previousNumber - secondPreviousNumber;
                        if ((lastDifference < 0 && secondLastDifference > 0) ||
                            (lastDifference > 0 && secondLastDifference < 0))
                        {
                            isSafe = false;
                            break;
                        }
                    }
                }

                if (isSafe)
                {
                    didFindSafe = true;
                    break;
                }
            }

            if (didFindSafe)
                safeLevel++;
        }

        return safeLevel;
    }

    public static void Main(string[] args)
    {
        var data1 = ParseInput("TestData.txt");
        var data2 = ParseInput("TestData2.txt");

        Console.WriteLine("Safe reports:");
        Console.WriteLine("Input 1: " + GetSafeReports(data1));
        Console.WriteLine("Input 2: " + GetSafeReports(data2));

        Console.WriteLine("Safe reports with 0 bad tolerance");
        Console.WriteLine("Input 1: " + GetSafeReports(data1, 0));
        Console.WriteLine("Input 2: " + GetSafeReports(data2, 0));

        Console.WriteLine("Safe reports with 1 bad tolerance");
        Console.WriteLine("Input 1: " + GetSafeReports(data1, 1));
        Console.WriteLine("Input 2: " + GetSafeReports(data2, 1));
    }
}