using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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

public class DungeonGenerator : MonoBehaviour
{
    public int levelWidth = 101;
    public int levelHeight = 81;

    public GameObject room;
    public GameObject levelWall;
    public int numRoomTries = 100; // how many times to attempt to add a room -> more attempts should result in more rooms
    public int roomMinSize = 3; // must be odd
    public int roomMaxSize = 13; // must be odd

    /// The inverse chance of adding a connector between two regions that have
    /// already been joined. Increasing this leads to more loosely connected
    /// dungeons.
    public int extraConnectorChance = 20;

    public int windingPercent = 0;

    private Tile[,] dungeonTiles;

    // For each position in the dungeon, the index of the connected region that position is a part of
    private int[,] regions;
    private int currentRegion = -1; // the index of the current region being carved

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(levelHeight % 2 != 0, "levelHeight must be odd!");
        Debug.Assert(levelWidth % 2 != 0, "levelWidth must be odd!");
        InitializeLevel();
        Generate();
    }

    void Update()
    {

    }

    private void InitializeLevel()
    {
        dungeonTiles = new Tile[levelWidth, levelHeight];
        regions = new int[levelWidth, levelHeight];
        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                Tile tile = new Tile
                {
                    coord = new Vector2Int(x, y)
                };
                if (y == 0 || y == levelHeight - 1)
                {
                    tile.type = TileType.Wall;
                    dungeonTiles[x, y] = tile;
                }
                else if (x == 0 || x == levelWidth - 1) 
                {
                    tile.type = TileType.Wall;
                    dungeonTiles[x, y] = tile;
                }
                else
                {
                    tile.type = TileType.Unused;
                    dungeonTiles[x, y] = tile;
                }
                regions[x, y] = -1;
            }
        }
    }

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
    public void Generate()
    {
        AddRooms();

        // Fill in all of the empty space with mazes.
        for (var y = 1; y < levelHeight; y += 2)
        {
            for (var x = 1; x < levelWidth; x += 2)
            {
                var pos = new Vector2Int(x, y);
                if (dungeonTiles[x,y].type == TileType.Unused)
                {
                    //Debug.Log("growMaze:" + pos);
                    GrowMaze(pos);
                }
            }
        }

        ConnectRegions();

        /*
        for (int y = 0; y < regions.GetLength(1); y++)
        {
            string r = "";
            for (int x = 0; x < regions.GetLength(0); x++)
            {
                r += regions[x, y].ToString();
            }
            Debug.Log(r);
        }
        */
        
    }

    private void AddRooms()
    {
        List<Room> rooms = new List<Room>();
        float timeTaken = 0f;

        // try a certain number of times to add rooms to the list of rooms
        for (int i = 0; i < numRoomTries; i++)
        {
            // create a room of random (odd shaped) size
            Room thisRoom = Utilities.GenerateRoom(roomMinSize, roomMaxSize, "r");
            //pick a random coordinate in which room will fit to check for overlapping
            int x = Random.Range(0, Mathf.FloorToInt((levelWidth - thisRoom.size.x) / 2)) * 2 + 1;
            int y = Random.Range(0, Mathf.FloorToInt((levelHeight - thisRoom.size.y) / 2)) * 2 + 1;
            Vector2Int newCoord = new Vector2Int(x, y);

            bool overlaps = false;
            foreach (var tile in thisRoom.tiles)
            {
                Vector2Int tileCoord = newCoord + tile.coord;
                if (dungeonTiles[tileCoord.x, tileCoord.y].type != TileType.Unused)
                {
                    overlaps = true;
                }
            }

            if (!overlaps)
            {
                rooms.Add(thisRoom);
                StartRegion();

                thisRoom.coord = newCoord;
                // update available tiles
                foreach (var tile in thisRoom.tiles)
                {
                    Vector2Int pos = new Vector2Int(thisRoom.coord.x + tile.coord.x, thisRoom.coord.y + tile.coord.y);
                    Carve(pos, TileType.Floor);
                }
                //Debug.Log("Added Room! Failures:" + numFailures);
            }
            timeTaken += Time.deltaTime;
        }

        // add each room to the scene
        foreach (var thisRoom in rooms)
        {
            thisRoom.gameobj = Instantiate(room, new Vector3(thisRoom.coord.x, thisRoom.coord.y), Quaternion.identity, transform);
            thisRoom.gameobj.GetComponent<RoomController>().Create(thisRoom);
        }
        Debug.Log("AddRooms completed in " + timeTaken + "s.");
    }

    private void ConnectRegions()
    {
        // find all the tiles that can connect two or more regions
        Dictionary<Vector2Int, HashSet<int>> connectorRegions = new Dictionary<Vector2Int, HashSet<int>>();
        foreach (var tile in dungeonTiles)
        {
            // can't already be part of a region
            if (dungeonTiles[tile.coord.x, tile.coord.y].type != TileType.Unused) continue;

            HashSet<int> _regions = new HashSet<int>();
            foreach (var dir in Utilities.directions)
            {
                Vector2Int newPos = tile.coord + dir;
                if (newPos.x >= 0 && newPos.x < regions.GetUpperBound(0) && newPos.y >= 0 && newPos.y < regions.GetUpperBound(1))
                {
                    int _region = regions[newPos.x, newPos.y];
                    if (_region >= 0) _regions.Add(_region);
                }
            }
            if (_regions.Count < 2) continue;
            connectorRegions[tile.coord] = _regions;
        }

        var connectorTiles = connectorRegions.Keys.ToList();

        // Keep track of which regions have been merged. This maps an original
        // region index to the one it has been merged to.
        var merged = new Dictionary<int, int>();
        var openRegions = new HashSet<int>();
        for (int i = 0; i <= currentRegion; i++)
        {
            merged[i] = i;
            openRegions.Add(i); // they're all open so far
        }

        // Keep connecting regions until we're down to one.
        while (openRegions.Count > 1)
        {
            // grab a random connector from our list
            var rand = Random.Range(0, connectorTiles.Count - 1);
            var connector = connectorTiles.ElementAt(rand);
            connectorTiles.Remove(connector); // and remove it

            // carve the connection
            AddJunction(connector);

            // Merge the connected regions
            var mapRegions = new HashSet<int>();
            foreach (var r in new HashSet<int>(connectorRegions[connector]))
            {
                mapRegions.Add(merged[r]);
            }
            var mergeTo = mapRegions.First(); // use the first region to keep
            List<int> sources = mapRegions.Skip(1).ToList(); // put the rest of the regions into a list

            // Merge all of the affected regions. We have to look at *all* of the
            // regions because other regions may have previously been merged with
            // some of the ones we're merging now.
            for (var i = 0; i <= currentRegion; i++)
            {
                if (sources.Contains(merged[i]))
                {
                    merged[i] = mergeTo;
                }
            }

            // remove the sources since they are no longer in use
            foreach (var source in sources) openRegions.Remove(source);

            // Remove any connectors that aren't needed anymore.
            // Don't allow connectors right next to each other.
            connectorTiles.RemoveAll(pos => (connector - pos).sqrMagnitude < 2);
            // If the connector no longer spans different regions, we don't need it.
            var test = connectorTiles.Select(pos => connectorRegions[pos]);
            var toRemove = new List<Vector2Int>();
            int setcount=0;
            int regcount=0;
            foreach (var tile in connectorTiles)
            {
                var _mapRegions = new HashSet<int>();
                foreach (var r in new HashSet<int>(connectorRegions[tile]))
                {
                    _mapRegions.Add(merged[r]);
                    setcount++;
                }

                if (_mapRegions.Count <= 1)
                {
                    toRemove.Add(tile);
                    regcount++;
                }
                else
                {
                    if (Utilities.OneIn(extraConnectorChance)) AddJunction(tile);
                    toRemove.Add(tile);
                }
            }
            foreach (var item in toRemove)
            {
                connectorTiles.Remove(item);
            }
        }
    }

    void AddJunction(Vector2Int pos)
    {
        TileType type;
        if (Utilities.OneIn(4))
        {
            type = Utilities.OneIn(3) ? TileType.OpenDoor : TileType.Floor;
        }
        else
        {
            type = TileType.ClosedDoor;
        }
        dungeonTiles[pos.x, pos.y].type = type;
        Instantiate(levelWall, new Vector3(pos.x, pos.y), Quaternion.identity, transform);
        //Debug.Log("Juction added at " + pos + " type " + type);
    }


    /// Implementation of the "growing tree" algorithm from here:
    /// http://www.astrolog.org/labyrnth/algrithm.htm.
    private void GrowMaze(Vector2Int start)
    {
        List<Vector2Int> cells = new List<Vector2Int>();
        Vector2Int lastDir = Vector2Int.up; // TODO is this OK to set to up?

        StartRegion();
        Carve(start);

        cells.Add(start);
        while (cells.Count > 0)
        {
            var cell = cells[cells.Count - 1]; // grab the last cell from the list

            // see which adjacent cells are open
            List<Vector2Int> unmadeCells = new List<Vector2Int>();
            
            foreach (var dir in Utilities.directions)
            {
                if (CanCarve(cell, dir))
                {
                    unmadeCells.Add(dir);
                }
            }

            if (unmadeCells.Count > 0)
            {
                // try to prefer carving in the same direction to avoid being too windy
                Vector2Int dir;
                if (unmadeCells.Contains(lastDir) && Random.Range(0, 100) > windingPercent)
                {
                    dir = lastDir;
                }
                else
                {
                    int randIndex = Random.Range(0, unmadeCells.Count - 1); // find random index of unmade cell
                    dir = unmadeCells[randIndex]; // set dir to this cell
                    unmadeCells.RemoveAt(randIndex); // remove it from list
                }

                Carve(cell + dir);
                Carve(cell + dir * 2);

                cells.Add(cell + dir * 2);
                lastDir = dir;
            }
            else
            {
                // no adjacent uncarved cells - remove last cell 
                cells.RemoveAt(cells.Count - 1);

                // this path has ended
                // lastDir = null; TODO do I need this?
            }
        }
    }

    /// Gets whether or not an opening can be carved from the given starting
    /// [cell] at [pos] to the adjacent cell facing [direction]. Returns true
    /// if the starting cell is in bounds and the destination cell is filled
    /// (or out of bounds).
    bool CanCarve(Vector2Int pos, Vector2Int direction)
    {
        // Must end inside dungeon
        Vector2Int endPos = pos + direction * 3;
        if (endPos.x >= levelWidth || endPos.y >= levelHeight)
        {
            return false;
        }
        if (endPos.x < 0 || endPos.y < 0)
        {
            return false;
        }

        // Destination must not be unused.
        Vector2Int openPos = pos + direction * 2;
        return dungeonTiles[openPos.x, openPos.y].type == TileType.Unused;
    }

    private void StartRegion()
    {
        currentRegion++;
    }

    private void Carve(Vector2Int pos, TileType type = TileType.Floor)
    {
        //Debug.Log("Carving:" + pos);
        dungeonTiles[pos.x, pos.y].type = type;
        regions[pos.x, pos.y] = currentRegion;
        dungeonTiles[pos.x, pos.y].region = currentRegion;
        // debug: instantiate tile so we cal see
        Instantiate(levelWall, new Vector3(pos.x, pos.y), Quaternion.identity, transform);
    }

}
