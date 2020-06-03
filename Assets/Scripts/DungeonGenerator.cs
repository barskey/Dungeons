using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  SB:"Ported" from https://github.com/munificent/hauberk -> dungeon.dart
/// The random dungeon generator.
///
/// Starting with a stage of solid walls, it works like so:
///
/// 1. Place a number of randomly sized and positioned rooms. If a room
///    overlaps an existing room, it is discarded. Any remaining rooms are
///    carved out.
/// 2. Any remaining solid areas are filled in with mazes. The maze generator
///    will grow and fill in even odd-shaped areas, but will not touch any
///    rooms.
/// 3. The result of the previous two steps is a series of unconnected rooms
///    and mazes. We walk the stage and find every tile that can be a
///    "connector". This is a solid tile that is adjacent to two unconnected
///    regions.
/// 4. We randomly choose connectors and open them or place a door there until
///    all of the unconnected regions have been joined. There is also a slight
///    chance to carve a connector between two already-joined regions, so that
///    the dungeon isn't single connected.
/// 5. The mazes will have a lot of dead ends. Finally, we remove those by
///    repeatedly filling in any open tile that's closed on three sides. When
///    this is done, every corridor in a maze actually leads somewhere.
///
/// The end result of this is a multiply-connected dungeon with rooms and lots
/// of winding corridors.
///
public class DTile
{
    public bool isAvailable = true;
}

public class DungeonGenerator : MonoBehaviour
{
    public int levelWidth = 100;
    public int levelHeight = 80;

    public GameObject room;
    public GameObject levelWall;
    public int numRoomTries;
    public int roomMinSize = 3; // must be odd
    public int roomMaxSize = 13; // must be odd

    /// The inverse chance of adding a connector between two regions that have
    /// already been joined. Increasing this leads to more loosely connected
    /// dungeons.
    public int extraConnectorChance = 20;

    /// Increasing this allows rooms to be larger.
    public int roomExtraSize = 0;

    public int windingPercent = 0;

    private DTile[,] dungeonTiles;

    // Start is called before the first frame update
    void Start()
    {
        dungeonTiles = new DTile[levelWidth, levelHeight];
        InitializeLevel();
        Generate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitializeLevel()
    {
        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                dungeonTiles[x, y] = new DTile();
            }
        }
    }

    public void Generate()
    {
        AddWalls();
        AddRooms();
        /*
        // check if width and height are odd
        if (stage.width % 2 == 0 || stage.height % 2 == 0)
        {
            Debug.LogError("The stage must be odd-sized.");
        }
        // fill every coordinate with a wall tile
        fill(Tiles.wall);
        int[,] regions = new int[stage.width, stage.height];

        addRooms();

        // Fill in all of the empty space with mazes.
        for (var y = 1; y < bounds.height; y += 2)
        {
            for (var x = 1; x < bounds.width; x += 2)
            {
                var pos = new Vec(x, y);
                if (getTile(pos) != Tiles.wall) continue;
                _growMaze(pos);
            }
        }

        _connectRegions();
        _removeDeadEnds();

        _rooms.forEach(onDecorateRoom);
        */
    }

    private void AddWalls()
    {
        // add walls to perimeter of dungeon
        for (int y = 0; y < levelHeight; y++)
        {
            if (y == 0 || y == levelHeight - 1)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    Vector2 coord = new Vector2(x, y);
                    GameObject.Instantiate(levelWall, coord, Quaternion.identity, transform);
                }
            }
            else
            {
                GameObject.Instantiate(levelWall, new Vector2(0, y), Quaternion.identity, transform);
                GameObject.Instantiate(levelWall, new Vector2(levelWidth - 1, y), Quaternion.identity, transform);
            }
        }
    }

    private void AddRooms()
    {
        // create a room at a random available coordinate
        // check if the tile coords for all room tiles are available
        //    delete room if they are not
        //    update available tiles if they are
        // repeat until number of allowed failures is reached
        int numFailures = 0;

        Vector2 availCoord = GetAvailableCoord();
        if (availCoord.Equals(Vector2Int.zero))
        {
            numFailures++;
            Debug.Log(string.Format("Couldn't get available coordinate. Failures:{0}", numFailures));
        }
        else
        {
            //while (numFailures < numRoomTries)
            //{
                var rm = GameObject.Instantiate(room, availCoord, Quaternion.identity, transform);
                var roomCoords = rm.GetComponent<RoomController>().tileCoords;

                // check that all tile locations for this room are OK
                bool tilesOK = true;
                foreach (var coord in roomCoords)
                {
                    if (!dungeonTiles[coord.x, coord.y].isAvailable)
                    {
                        tilesOK = false;
                    }
                }

                if (tilesOK)
                {
                    // update available tiles
                    foreach (var coord in rm.GetComponent<RoomController>().RoomTileCoords())
                    {
                        dungeonTiles[(int)coord.x, (int)coord.y].isAvailable = false;
                    }
                }
                else
                {
                    Destroy(rm);
                    numFailures++;
                    Debug.Log(string.Format("Room didn't fit. Failures:{0}", numFailures));
                }
            //}
        }
    }

    private Vector2Int GetAvailableCoord()
    {
        bool isAvailable = false;
        int numTries = 0;
        Vector2Int randCoord = Vector2Int.zero;
        while (!isAvailable || numTries >= 10)
        {
            randCoord = new Vector2Int(Random.Range(1, levelWidth - 1), Random.Range(1, levelHeight - 1));
            if (dungeonTiles[randCoord.x, randCoord.y].isAvailable)
            {
                isAvailable = true;
            }
            numTries++;
        }
        return randCoord;
    }
}
