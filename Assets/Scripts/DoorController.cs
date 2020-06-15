using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : TileController
{
    public bool locked;

    private SpriteData openSprite;
    private SpriteData closedSprite;


    public override void SetSprites()
    {
        base.SetSprites();

        sr.sprite = tileSet.doorSetClosed[0];
    }

}
