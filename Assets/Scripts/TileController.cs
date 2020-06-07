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
    private struct SpriteData
    {
        public Sprite sprite;
        public bool flipX, flipY;

        public SpriteData(Sprite s, bool x = false, bool y = false)
        {
            sprite = s;
            flipX = x;
            flipY = y;
        }
    }
    private SpriteData[] floorSprites = new SpriteData[16];
    private SpriteData[] wallSprites = new SpriteData[16];

    // Start is called before the first frame update
    void Start()
    {
        dungeon = FindObjectOfType<DungeonGenerator>();
        sr = GetComponent<SpriteRenderer>();

        LoadSprites();
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

    private void LoadSprites()
    {
        //   . n .
        //   w x e
        //   . s .
        // 16 combinations to check, using 4-bit binary in order nesw
        // 1 is wall, 0 is notwall
        // 0000 : C
        // 0001 : W
        // 0010 : S
        // 0011 : SW
        // 0100 : E
        // 0101 : S_NS
        // 0110 : SE
        // 0111 : S_S
        // 1000 : N
        // 1001 : NW
        // 1010 : S_WE
        // 1011 : S_W
        // 1100 : NE
        // 1101 : S_N
        // 1110 : S_E
        // 1111 : S_4
        // load sprites in this order
        floorSprites[0] = new SpriteData(tileSet.floor_C);
        floorSprites[1] = new SpriteData(tileSet.floor_W);
        floorSprites[2] = new SpriteData(tileSet.floor_S);
        floorSprites[3] = new SpriteData(tileSet.floor_SW);
        floorSprites[4] = new SpriteData(tileSet.floor_E);
        floorSprites[5] = new SpriteData(tileSet.floorS_NS);
        floorSprites[6] = new SpriteData(tileSet.floor_SE);
        floorSprites[7] = new SpriteData(tileSet.floorS_S);
        floorSprites[8] = new SpriteData(tileSet.floor_N);
        floorSprites[9] = new SpriteData(tileSet.floor_NW);
        floorSprites[10] = new SpriteData(tileSet.floorS_WE);
        floorSprites[11] = new SpriteData(tileSet.floorS_W);
        floorSprites[12] = new SpriteData(tileSet.floor_NE);
        floorSprites[13] = new SpriteData(tileSet.floorS_N);
        floorSprites[14] = new SpriteData(tileSet.floorS_E);
        floorSprites[15] = new SpriteData(tileSet.floorS_4);

        // 1 is wall 0 is notwall
        // 0000 : +
        // 0001 : i+ rot
        // 0010 : i+ y
        // 0011 : NE
        // 0100 : i+ rot
        // 0101 : N
        // 0110 : NW
        // 0111 : TN
        // 1000 : i+
        // 1001 : SE
        // 1010 : W
        // 1011 : TE
        // 1100 : SW
        // 1101 : TS
        // 1110 : TW
        // 1111 : +
        // load sprites in this order
        wallSprites[0] = new SpriteData(tileSet.wall_Cross);
        wallSprites[1] = new SpriteData(tileSet.wall_iCross);
        wallSprites[2] = new SpriteData(tileSet.wall_iCross, false, true);
        wallSprites[3] = new SpriteData(tileSet.wall_LNE);
        wallSprites[4] = new SpriteData(tileSet.wall_iCross);
        wallSprites[5] = new SpriteData(tileSet.wall_NS);
        wallSprites[6] = new SpriteData(tileSet.wall_LNW);
        wallSprites[7] = new SpriteData(tileSet.wall_TN);
        wallSprites[8] = new SpriteData(tileSet.wall_iCross);
        wallSprites[9] = new SpriteData(tileSet.wall_LSE);
        wallSprites[10] = new SpriteData(tileSet.wall_WE);
        wallSprites[11] = new SpriteData(tileSet.wall_TE);
        wallSprites[12] = new SpriteData(tileSet.wall_LSW);
        wallSprites[13] = new SpriteData(tileSet.wall_TS);
        wallSprites[14] = new SpriteData(tileSet.wall_TW);
        wallSprites[15] = new SpriteData(tileSet.wall_oCross);
    }

    private void SetFloorSprite()
    {
        // for brevity in conditionals
        var n = neighbors[(int)Dir.N].type == TileType.Wall ? 8 : 0;
        var e = neighbors[(int)Dir.E].type == TileType.Wall ? 4 : 0;
        var s = neighbors[(int)Dir.S].type == TileType.Wall ? 2 : 0;
        var w = neighbors[(int)Dir.W].type == TileType.Wall ? 1 : 0;
        var index = n + e + s + w;
        sr.sprite = floorSprites[index].sprite;
        sr.flipX = floorSprites[index].flipX;
        sr.flipY = floorSprites[index].flipY;
    }

    private void SetWallSprite()
    {
        // for brevity in conditionals
        var n = neighbors[(int)Dir.N].type == TileType.Wall ? 8 : 0;
        var e = neighbors[(int)Dir.E].type == TileType.Wall ? 4 : 0;
        var s = neighbors[(int)Dir.S].type == TileType.Wall ? 2 : 0;
        var w = neighbors[(int)Dir.W].type == TileType.Wall ? 1 : 0;
        var index = n + e + s + w;
        sr.sprite = wallSprites[index].sprite;
        sr.flipX = wallSprites[index].flipX;
        sr.flipY = wallSprites[index].flipY;
    }
}
