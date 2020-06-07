using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Dungeons/TileSet", order = 1)]
public class TileSet : ScriptableObject
{
    public string setName;

    public Sprite wall_LNW;
    public Sprite wall_NS;
    public Sprite wall_LNE;
    public Sprite wall_WE;
    public Sprite wall_LSW;
    public Sprite wall_LSE;
    public Sprite wall_oCross;
    public Sprite wall_TN;
    public Sprite wall_TW;
    public Sprite wall_TS;
    public Sprite wall_TE;
    public Sprite wall_Cross;
    public Sprite wall_iCross;

    public Sprite floor_NW;
    public Sprite floor_N;
    public Sprite floor_NE;
    public Sprite floor_W;
    public Sprite floor_C;
    public Sprite floor_E;
    public Sprite floor_SW;
    public Sprite floor_S;
    public Sprite floor_SE;

    public Sprite floorS_N;
    public Sprite floorS_NS;
    public Sprite floorS_S;
    public Sprite floorS_W;
    public Sprite floorS_WE;
    public Sprite floorS_E;
    public Sprite floorS_4;

}