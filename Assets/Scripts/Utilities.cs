using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Floor,
    Wall,
    OpenDoor,
    ClosedDoor,
    Unused,
    Void
}

// for convenience so we can reference neighbor to the east as neighbors[Dir.E]
public enum Dir
{
    N, // 0
    E, // 1
    S, // 2
    W  // 3
}

public enum RoomType
{
    Dungeon,
    Boss,
    Treasure,
    Portal,
    Forest,
    Plains
}

public class Tile
{
    public TileType type = TileType.Unused;
    public Vector2Int coord;
    public int region = -1;
    public Tile[] neighbors = new Tile[4];
    public GameObject gameobj;
}

public class Room
{
    public RoomType type = RoomType.Dungeon;
    public Vector2Int coord;
    public List<Tile> tiles;
    public GameObject gameobj;
    public Vector2Int size;
}

public class Utilities
{
    public static Vector2Int[] directions = new Vector2Int[] {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    public static Vector2Int[] diagonals = new Vector2Int[] {
        Vector2Int.up + Vector2Int.left,
        Vector2Int.up + Vector2Int.right,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.down + Vector2Int.right
    };

    // returns true if random int chosen between 1 and chance was 1
    public static bool OneIn(int chance)
    {
        if (Random.Range(1, chance) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Room GenerateRoom(int minSize, int maxSize, string shape = "r")
    {
        // Generates representation of a random room given range for size
        // and shape preference
        // returns array of tiles
        Room room = new Room
        {
            type = RoomType.Dungeon
        };

        // Pick a random room size. Make sure rooms are odd-sized to line up with maze.1
        var width = Random.Range(minSize, maxSize);
        if (width % 2 == 0) width++;
        var height = Random.Range(minSize, maxSize);
        if (height % 2 == 0) height++;
        room.size = new Vector2Int(width, height);

        switch (shape)
        {
            case "r":
                room.tiles = CreateRectangle(width, height);
                break;
            default:
                room.tiles = CreateRectangle(width, height);
                break;
        }

        return room;
    }

    private static List<Tile> CreateRectangle(int width, int height)
    {
        List<Tile> tiles = new List<Tile>();

        // add a floor tile for each coord in this room
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tile = new Tile
                {
                    coord = new Vector2Int(x, y),
                    type = TileType.Floor
                };
                tiles.Add(tile);
            }
        }

        return tiles;
    }
}