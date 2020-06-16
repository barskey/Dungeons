using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : TileController
{
    private List<TileType> types = new List<TileType> { TileType.Wall, TileType.ClosedDoor, TileType.OpenDoor };

    public override void SetSprites()
    {
        base.SetSprites();

        // for brevity in conditionals
        var n = types.Contains(neighbors[(int)Dir.N].type) ? 8 : 0;
        var e = types.Contains(neighbors[(int)Dir.E].type) ? 4 : 0;
        var s = types.Contains(neighbors[(int)Dir.S].type) ? 2 : 0;
        var w = types.Contains(neighbors[(int)Dir.W].type) ? 1 : 0;

        var index = n + e + s + w;
        sr.sprite = tileSet.wallSet[index];
    }
}
