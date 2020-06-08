using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
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

        LoadSprites();
        Initialize();
    }

    public virtual void Initialize()
    {

    }

    public virtual void LoadSprites()
    {

    }

    public virtual void SetSprite()
    {

    }
}
