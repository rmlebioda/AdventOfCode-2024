namespace Day6;

public class LoopDetector
{
    private List<Position> Positions = new();

    public void DetectLoop(Position position)
    {
        Positions.Add(position);
        if (Positions.GroupBy(x => x).Select(x => x.Count()).Any(x => x >= 2))
            throw new ParadoxException();
    }
}
