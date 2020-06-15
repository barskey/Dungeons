using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Dungeons/TileSet", order = 1)]
public class TileSet : ScriptableObject
{
    public string setName;

    public string wallSetName = "Wall0";
    public Sprite[] wallSet = new Sprite[16];
    public string floorSetName = "Floor0";
    public Sprite[] floorSet = new Sprite[16];
    public string doorSetName = "Door0";
    public Sprite[] doorSetOpen = new Sprite[8];
    public Sprite[] doorSetClosed = new Sprite[8];

}