namespace Day5;

public static class Program
{
    class Rule
    {
        public int Order { get; set; }
        public int Update { get; set; }
    }
    
    private static long GetSumMiddleNumbersOfOrderedUpdates(string input, bool correctlyOrdered)
    {
        var lines = input.Split("\n");
        var emptyLineIndex = Array.IndexOf(lines, string.Empty);
        var pageOrderingRules = lines
            .Take(emptyLineIndex)
            .Select(
                line => new Rule {
                    Order = int.Parse(line.Split("|")[0]),
                    Update = int.Parse(line.Split("|")[1])
                })
            .ToArray();
        var updates = lines
            .Skip(emptyLineIndex + 1)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split(",").Select(int.Parse).ToArray())
            .ToArray();
        return GetSumMiddleNumbersOfOrderedUpdates(pageOrderingRules, updates, correctlyOrdered);
    }

    private static long GetSumMiddleNumbersOfOrderedUpdates(
        Rule[] rules, int[][] updates, bool correctlyOrdered)
    {
        long result = 0;
        
        foreach (var update in updates)
        {
            if (update is null || update.Length % 2 != 1)
                throw new ArgumentException(nameof(updates));
            
            bool isValid = true;
            for (int i = 0; i < update.Length; i++)
            {
                var previousNumbers = update.Take(i).ToArray();
                var nextNumbers = update.Skip(i + 1).ToArray();
                isValid = isValid && GetInvalidIndex(rules, previousNumbers, nextNumbers, update[i]) == null;
                if (!isValid)
                    break;
            }

            if (correctlyOrdered)
            {
                if (isValid)
                    result += update[update.Length / 2];
            }
            else
            {
                if (!isValid)
                    result += FixAndGetMiddleNumbersOfOrderedUpdates(rules, update);
            }
        }

        return result;
    }

    private static int? GetInvalidIndex(Rule[] rules, int[] previousNumbers,
        int[] nextNumbers, int number)
    {
        for (int i = 0; i < previousNumbers.Length; i++)
        {
            var previousNumber = previousNumbers[i];
            if (rules.Any(rule => rule.Order == number && rule.Update == previousNumber))
                return i;
        }

        for (int i = 0; i < nextNumbers.Length; i++)
        {
            var nextNumber = nextNumbers[i];
            if (rules.Any(rule => rule.Order == nextNumber && rule.Update == number))
                return previousNumbers.Length + 1 + i;
        }
        
        return null;
    }
    
    private static long FixAndGetMiddleNumbersOfOrderedUpdates(Rule[] rules, int[] numbers)
    {
        var numbersCopy = numbers.ToList();
        for (int i = 0; i < numbersCopy.Count; i++)
        {
            var previousNumbers = numbersCopy.Take(i).ToArray();
            var nextNumbers = numbersCopy.Skip(i + 1).ToArray();
            var invalidIndex =
                GetInvalidIndex(rules, previousNumbers, nextNumbers, numbersCopy[i]);
            if (invalidIndex.HasValue)
            {
                (numbersCopy[i], numbersCopy[invalidIndex.Value]) = (numbersCopy[invalidIndex.Value], numbersCopy[i]);
                i = -1;
            }
        }
        return numbersCopy[numbersCopy.Count / 2];
    }
    
    public static void Main(string[] args)
    {
        var input1 = File.ReadAllText("TestData.txt");
        var input2 = File.ReadAllText("TestData2.txt");

        Console.WriteLine($"Correctly orderer updates:");
        Console.WriteLine("Input 1: " + GetSumMiddleNumbersOfOrderedUpdates(input1, true));
        Console.WriteLine("Input 2: " + GetSumMiddleNumbersOfOrderedUpdates(input2, true));
        
        Console.WriteLine($"Incorrectly orderer updates:");
        Console.WriteLine("Input 1: " + GetSumMiddleNumbersOfOrderedUpdates(input1, false));
        Console.WriteLine("Input 2: " + GetSumMiddleNumbersOfOrderedUpdates(input2, false));
    }
}
