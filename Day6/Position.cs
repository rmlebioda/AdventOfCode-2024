namespace Day6;

public record struct Position
{
    public int X;
    public int Y;
    public Direction Direction;

    public Position Rotated()
    {
        Direction newDirection;
        if (Direction == Direction.East)
            newDirection = Direction.South;
        else if (Direction == Direction.South)
            newDirection = Direction.West;
        else if (Direction == Direction.West)
            newDirection = Direction.North;
        else if (Direction == Direction.North)
            newDirection = Direction.East;
        else
            throw new NotImplementedException(Direction.ToString());

        return new Position()
        {
            X = X,
            Y = Y,
            Direction = newDirection
        };
    }
}