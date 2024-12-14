namespace Day1;

public static class Program
{
    private static List<Tuple<int, int>> ParseInput(string input)
    {
        var lines = input.Split("\n").Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
        return lines.Select(line => new Tuple<int, int>(
                int.Parse(line.Split().Where(s => !string.IsNullOrWhiteSpace(s)).ToList()[0]),
                int.Parse(line.Split().Where(s => !string.IsNullOrWhiteSpace(s)).ToList()[1])))
            .ToList();
    }

    public static long GetTotalDistance(string input)
    {
        var parsedArray = ParseInput(input);
        var array1 = parsedArray.Select(t => t.Item1).ToList();
        var array2 = parsedArray.Select(t => t.Item2).ToList();
        if (array1.Count() != array2.Count())
        {
            throw new Exception("Arrays are not the same length");
        }

        long distance = 0;
        while (array1.Any())
        {
            var firstNumber = array1.Min();
            array1.Remove(firstNumber);
            var secondNumber = array2.Min();
            array2.Remove(secondNumber);
            distance += Math.Abs(firstNumber - secondNumber);
        }

        return distance;
    }

    public static long SimilarityScore(string input)
    {
        var parsedArray = ParseInput(input);
        var array1 = parsedArray.Select(t => t.Item1).ToList();
        var array2 = parsedArray.Select(t => t.Item2).ToList();

        long similarityScore = 0;
        foreach (var number in array1)
        {
            similarityScore += number * array2.Count(c => c == number);
        }

        return similarityScore;
    }

    public static void Main(string[] args)
    {
        var input1 = File.ReadAllText("TestData.txt");
        var input2 = File.ReadAllText("TestData2.txt");

        Console.WriteLine("Distance:");
        Console.WriteLine("Input 1: " + GetTotalDistance(input1));
        Console.WriteLine("Input 2: " + GetTotalDistance(input2));

        Console.WriteLine("Similarity score:");
        Console.WriteLine("Input 1: " + SimilarityScore(input1));
        Console.WriteLine("Input 2: " + SimilarityScore(input2));
    }
}