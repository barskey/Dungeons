using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public TileSet tileSet;
    public Vector2Int coord;
    public TileType type;
    public Tile[] neighbors = new Tile[4];

    private DungeonGenerator dungeon;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        dungeon = FindObjectOfType<DungeonGenerator>();
        sr = GetComponent<SpriteRenderer>();

        Initialize();
    }

    public void Initialize()
    {
        switch (type)
        {
            case TileType.Wall:
                SetWallSprite();
                break;
            case TileType.Floor:
                SetFloorSprite();
                break;
            default:
                break;
        }
    }

    private void SetFloorSprite()
    {
        var n = neighbors[(int)Dir.N].type;
        var e = neighbors[(int)Dir.E].type;
        var s = neighbors[(int)Dir.S].type;
        var w = neighbors[(int)Dir.W].type;

        if (n == TileType.Wall && e == TileType.Wall && s == TileType.Wall && w == TileType.Wall)
        {
            sr.sprite = tileSet.floor_SF;
        }
        else if (n == TileType.Wall && e == TileType.Wall && s == TileType.Wall)
        {
            sr.sprite = tileSet.floor_SN; // TODO rotate CCW
        }
        else if (n == TileType.Wall && w == TileType.Wall && s == TileType.Wall)
        {
            sr.sprite = tileSet.floor_SS; // TODO rotate CW
        }
        else if (n == TileType.Wall && s == TileType.Wall)
        {
            sr.sprite = tileSet.floor_SC;
        }
        else if (n == TileType.Wall && e == TileType.Wall)
        {
            sr.sprite = tileSet.floor_NE;
        }
        else if (n == TileType.Wall && w == TileType.Wall)
        {
            sr.sprite = tileSet.floor_NW;
        }
        else if (n == TileType.Wall)
        {
            sr.sprite = tileSet.floor_N;
        }
        else
        {
            sr.sprite = tileSet.floor_C;
        }

    }

    private void SetWallSprite()
    {
        sr.sprite = tileSet.wall_WE;
    }
}
