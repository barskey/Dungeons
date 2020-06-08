using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : TileController
{
    private SpriteData[] floorSprites = new SpriteData[16];


    public override void Initialize()
    {
        base.Initialize();

        SetSprite();
    }

    // run in Start in parent class
    public override void LoadSprites()
    {
        base.LoadSprites();

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
    }

    public override void SetSprite()
    {
        base.SetSprite();

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
}
