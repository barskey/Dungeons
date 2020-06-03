using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] doorTiles;

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
        Create();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create()
    {
        switch (Random.Range(0, 10))
        {
            case 0:
                CreateRectangle();
                break;
            case 1:
                Debug.Log("Room 1");
                break;
            case 2:
                Debug.Log("Room 2");
                break;
            default:
                Debug.Log("Room other");
                CreateRectangle();
                break;
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

    // returns array of Vector2 containing coords of tiles
    private void CreateRectangle()
    {
        // Make a randomly-sized room but keep the aspect ratio reasonable.
        int shortSide = Random.Range(minSize, 10);
        int longSide = Random.Range(shortSide, Mathf.Min(maxSize, shortSide + 4));

        bool horizontal = Utilities.OneIn(2);
        int width = horizontal ? longSide : shortSide;
        int height = horizontal ? shortSide : longSide;

        // add a floor tile for each coord in this room
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var tile = GameObject.Instantiate(floorTiles[0], new Vector3(x, y), Quaternion.identity, transform);
            }
        }
        
    }
}
