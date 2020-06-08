using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : TileController
{
    public bool locked;

    private SpriteData[] doorSprites = new SpriteData[4];
    private SpriteData openSprite;
    private SpriteData closedSprite;

    public override void Initialize()
    {
        base.Initialize();

        SetSprite();
    }

    // run in Start in parent class
    public override void LoadSprites()
    {
        base.LoadSprites();

        if (locked)
        {
            openSprite = doorSprites[2];
            closedSprite = doorSprites[3];
        }
        else
        {
            openSprite = doorSprites[0];
            closedSprite = doorSprites[1];
        }
    }

    public override void SetSprite()
    {
        base.SetSprite();

        sr.sprite = closedSprite.sprite;
        sr.flipX = closedSprite.flipX;
        sr.flipX = closedSprite.flipY;
    }

}
