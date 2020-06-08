using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : TileController
{
    private SpriteData[] wallSprites = new SpriteData[16];


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

    public override void SetSprite()
    {
        base.SetSprite();

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
