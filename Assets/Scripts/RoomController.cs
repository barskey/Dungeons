using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<Vector2Int> tileCoords = new List<Vector2Int>();

    private int minSize;
    private int maxSize;
    private DungeonGenerator dg;

    // Start is called before the first frame update
    void Start()
    {
        dg = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonGenerator>();
        minSize = dg.roomMinSize;
        maxSize = dg.roomMaxSize;
        //Create();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create(Room room)
    {
        // add gameobject tiles for every roomtile
        foreach (var tile in room.tiles)
        {
            Vector3 coord = new Vector3(tile.coord.x, tile.coord.y);
            //Debug.Log("Creating tile at " + coord);
            var go = Instantiate(floorTiles[0], transform, false);
            go.transform.Translate(coord, Space.Self);
        }
    }

    public List<Vector3> RoomTileCoords()
    {
        List<Vector3> coords = new List<Vector3>();
        foreach(Transform child in transform)
        {
            coords.Add(transform.position);
        }
        return coords;
    }
}
