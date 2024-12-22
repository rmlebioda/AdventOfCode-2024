using System.Diagnostics;

namespace Day6;

public class Map
{
    public const char Obstacle = '#';
    public const char Path = '.';
    public const char VisitedPath = 'X';
    
    public char[][] MapData { get; private set; }
    public Position? Position { get; private set; }

    public Map(string input)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var y = lines.Length;
        var x = lines[0].Length;
        MapData = new char[y][];
        for (int i = 0; i < MapData.Length; i++)
            MapData[i] = lines[i].ToCharArray();
        
        Init();
    }

    public Map(Map anotherMap) : this(anotherMap.MapData)
    {
    }
    
    public Map(char[][] mapData)
    {
        MapData = new char[mapData.Length][];
        for (int i = 0; i < mapData.Length; i++)
        {
            MapData[i] = mapData[i].ToArray();
        }

        Init();
    }
    
    private void Init()
    {
        for (int y = 0; y < MapData.Length; y++)
        {
            for (int x = 0; x < MapData[y].Length; x++)
            {
                if (MapData[y][x] == '^')
                    Position = new Position { X = x, Y = y, Direction = Direction.North };
                if (MapData[y][x] == '>')
                    Position = new Position { X = x, Y = y, Direction = Direction.East};
                if (MapData[y][x] == 'v')
                    Position = new Position { X = x, Y = y, Direction = Direction.South};
                if (MapData[y][x] == '<')
                    Position = new Position { X = x, Y = y, Direction = Direction.West };
                
                if (Position is not null)
                    break;
            }

            if (Position is not null)
                break;
        }
        
        if (Position is null)
            throw new Exception("No starting position found");
    }
    
    public int GetDistinctPositionsCount()
    {
        return GetDistinctPositionsCount(new Map(this));
    }
    
    private static int GetDistinctPositionsCount(Map map)
    {
        var loopDetector = new LoopDetector();
        
        while (map.MoveNext())
        {
            if (map.Position is not null)
                loopDetector.DetectLoop(map.Position.Value);
        }

        int result = 0;
        for (int y = 0; y < map.MapData.Length; y++)
        {
            for (int x = 0; x < map.MapData[y].Length; x++)
            {
                if (map.MapData[y][x] == VisitedPath)
                    result++;
            }
        }

        return result;
    }

    public bool MoveNext()
    {
        if (Position is null)
            return false;

        do
        {
            var positionAfterMove = new Position()
            {
                X = Position.Value.X,
                Y = Position.Value.Y,
                Direction = Position.Value.Direction
            };
            if (positionAfterMove.Direction == Direction.East)
                positionAfterMove.X++;
            else if (positionAfterMove.Direction == Direction.West)
                positionAfterMove.X--;
            else if (positionAfterMove.Direction == Direction.North)
                positionAfterMove.Y--;
            else if (positionAfterMove.Direction == Direction.South)
                positionAfterMove.Y++;
            else
                throw new NotImplementedException(positionAfterMove.Direction.ToString());

            if (IsPositionOutOfBounds(positionAfterMove))
            {
                MapData[Position.Value.Y][Position.Value.X] = VisitedPath;
                Position = null;
                break;
            }
            else if (MapData[positionAfterMove.Y][positionAfterMove.X] == Obstacle)
            {
                Position = Position.Value.Rotated();
            }
            else if (IsWalkable(positionAfterMove))
            {
                MapData[Position.Value.Y][Position.Value.X] = VisitedPath;
                Position = positionAfterMove;
                break;
            }
            else
            {
                throw new Exception("Unsupported map data value: "
                                    + MapData[positionAfterMove.Y][positionAfterMove.X]);
            }
        } while (true);

        return true;
    }

    private bool IsPositionOutOfBounds(Position position)
    {
        return position.X < 0 || position.Y < 0 ||
               position.Y >= MapData.Length || position.X >= MapData[position.Y].Length;
    }

    private bool IsWalkable(Position position)
    {
        return MapData[position.Y][position.X] == Path
               || MapData[position.Y][position.X] == VisitedPath;
    }
    
    public bool IsParadoxWithAdditionalObstacle(int obstacleX, int obstacleY)
    {
        var stopwatch = Stopwatch.StartNew();
        var newMapData = new char[MapData.Length][];
        for (int i = 0; i < MapData.Length; i++)
            newMapData[i] = MapData[i].ToArray();
        newMapData[obstacleY][obstacleX] = Obstacle;

        try
        {
            _ = GetDistinctPositionsCount(new Map(newMapData));
            return false;
        }
        catch (ParadoxException)
        {
            return true;
        }
        finally
        {
            Console.WriteLine($"Executed point {obstacleX}/{obstacleY} in {stopwatch.ElapsedMilliseconds}ms");
        }
    }

    public int PossibleParadoxesWithAdditionalObstacle()
    {
        var traversedMap = new Map(this);
        _ = GetDistinctPositionsCount(traversedMap);
        var obstacles = new List<Tuple<int, int>>();
        for (int y = 0; y < MapData.Length; y++)
        {
            for (int x = 0; x < MapData[y].Length; x++)
            {
                if (MapData[y][x] == Path && traversedMap.MapData[y][x] == VisitedPath)
                // if (MapData[y][x] == Path)
                    obstacles.Add(new Tuple<int, int>(x, y));
            }
        }

        return obstacles
            .AsParallel()
            .Select(position =>
                IsParadoxWithAdditionalObstacle(position.Item1, position.Item2))
            .Select(paradox => paradox ? 1 : 0)
            .Sum();
    }
}