using System.Diagnostics;

namespace Day6;

public static class Program
{
    public static void Main()
    {
        var input1 = File.ReadAllText("TestData.txt");
        var input2 = File.ReadAllText("TestData2.txt");

        Console.WriteLine($"Distinct positions in map:");
        Console.WriteLine("Input 1: " + new Map(input1).GetDistinctPositionsCount());
        var stopwatch = Stopwatch.StartNew();
        var result = new Map(input2).GetDistinctPositionsCount();
        stopwatch.Stop();
        Console.WriteLine($"Input 2 ({stopwatch.ElapsedMilliseconds}ms): {result}");
        
        Console.WriteLine($"Different possible paradoxes with single obstruction:");
        Console.WriteLine("Input 1: " + new Map(input1).PossibleParadoxesWithAdditionalObstacle());
        stopwatch.Restart();
        result = new Map(input2).PossibleParadoxesWithAdditionalObstacle();
        stopwatch.Stop();
        Console.WriteLine($"Input 2 ({stopwatch.ElapsedMilliseconds}ms): {result}");
        // on 8745HS: "Input 2 (2511381ms): 2162"
    }
}
