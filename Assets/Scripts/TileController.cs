using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [HideInInspector]
    public TileSet tileSet;
    public Vector2Int coord;
    public TileType type;
    public Tile[] neighbors = new Tile[4];
    [HideInInspector]
    public SpriteRenderer sr;

    public struct SpriteData
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

    // Start is called before the first frame update
    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tileSet = FindObjectOfType<DungeonGenerator>().tileSet;
        //Debug.Log(tileSet.wallSet[0].name);
        Initialize();
    }

    public virtual void Initialize()
    {
        SetSprites();
    }

    public virtual void SetSprites()
    {

    }
}
