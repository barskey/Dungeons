using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : TileController
{

    public override void SetSprites()
    {
        base.SetSprites();

        // for brevity in conditionals
        var n = neighbors[(int)Dir.N].type == TileType.Wall ? 8 : 0;
        var e = neighbors[(int)Dir.E].type == TileType.Wall ? 4 : 0;
        var s = neighbors[(int)Dir.S].type == TileType.Wall ? 2 : 0;
        var w = neighbors[(int)Dir.W].type == TileType.Wall ? 1 : 0;
        var index = n + e + s + w;

        sr.sprite = tileSet.floorSet[index];
    }

    private void Update()
    {
        var playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        var distToPlayer = transform.position - playerPos;
        if (distToPlayer.magnitude > 4)
        {
            sr.color = Color.gray;
        }
        else
        {
            sr.color = Color.white;
        }
    }
}
