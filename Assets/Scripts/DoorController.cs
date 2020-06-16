using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : TileController
{
    public bool locked;


    public override void SetSprites()
    {
        base.SetSprites();

        if (neighbors[(int)Dir.N].type == TileType.Wall && neighbors[(int)Dir.S].type == TileType.Wall)
        {
            sr.sprite = tileSet.doorSetClosed[1];
        }
        else if (neighbors[(int)Dir.E].type == TileType.Wall && neighbors[(int)Dir.W].type == TileType.Wall)
        {
            sr.sprite = tileSet.doorSetClosed[0];
        }

    }

}
