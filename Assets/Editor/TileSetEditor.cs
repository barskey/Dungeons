using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSet))]
public class TileSetEditor : Editor
{
    float thumbnailSize = 42f;
    int wallIndex, floorIndex, doorIndex;
    Sprite[] sprites;
    string[] spriteNames;
    string selectedWall;

    //TileSet tileset = (TileSet);
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        TileSet tileset = (TileSet)target;
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites");
        var spriteNames = new List<string>();
        for (int i = 0; i < sprites.Length; i++)
        {
            var name = sprites[i].name.Split('_');
            if (!spriteNames.Contains(name[0])) spriteNames.Add(name[0]);
        }


        //----- Wall Sprites -----//
        // Sprites will get stored in this order
        //   . n .
        //   w x e
        //   . s .
        // 16 combinations to check, using 4-bit binary in order nesw
        // 1 is wall, 0 is notwall
        // 0  0000 : +
        // 1  0001 : i+ rot
        // 2  0010 : i+ y
        // 3  0011 : NE
        // 4  0100 : i+ rot
        // 5  0101 : N
        // 6  0110 : NW
        // 7  0111 : TN
        // 8  1000 : i+
        // 9  1001 : SE
        // 10 1010 : W
        // 11 1011 : TE
        // 12 1100 : SW
        // 13 1101 : TS
        // 14 1110 : TW
        // 15 1111 : O+

        EditorGUILayout.LabelField("Wall Tiles:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        tileset.wallSet[6] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[6], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[5] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[5], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[3] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[3], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        tileset.wallSet[15] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[15], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[7] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[7], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        tileset.wallSet[10] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[10], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[0] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[0], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[10] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[10], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        tileset.wallSet[14] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[14], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[8] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[8], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[11] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[11], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        tileset.wallSet[12] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[12], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[5] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[5], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[9] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[9], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        GUILayout.Space(thumbnailSize);
        tileset.wallSet[8] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[8], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Open Sprite:", GUILayout.Width(120f));

        GUILayout.Space(20f);

        //----- Floor Sprites -----//
        //   . n .
        //   w x e
        //   . s .
        // 16 combinations to check, using 4-bit binary in order nesw
        // 1 is wall, 0 is notwall
        // 0  0000 : C
        // 1  0001 : W
        // 2  0010 : S
        // 3  0011 : SW
        // 4  0100 : E
        // 5  0101 : S_NS
        // 6  0110 : SE
        // 7  0111 : S_S
        // 8  1000 : N
        // 9  1001 : NW
        // 10 1010 : S_WE
        // 11 1011 : S_W
        // 12 1100 : NE
        // 13 1101 : S_N
        // 14 1110 : S_E
        // 15 1111 : S_4
        // load sprites in this order

        EditorGUILayout.LabelField("Floor Tiles:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        tileset.floorSet[9] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[9], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.floorSet[8] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[8], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.floorSet[12] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[12], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        tileset.floorSet[13] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[13], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(thumbnailSize);
        tileset.floorSet[15] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[15], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        tileset.floorSet[1] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[1], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.floorSet[0] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[0], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.floorSet[4] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[4], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        tileset.floorSet[5] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[5], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.floorSet[11] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[11], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.floorSet[10] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[10], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.floorSet[14] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[14], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        tileset.floorSet[3] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[3], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.floorSet[2] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[2], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.floorSet[6] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[6], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        tileset.floorSet[7] = (Sprite)EditorGUILayout.ObjectField(tileset.floorSet[7], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20f);

        //----- Door Sprites -----//
        EditorGUILayout.LabelField("Door Tiles:", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(60f);
        GUILayout.Space(10f);
        EditorGUILayout.LabelField("Horizontal", GUILayout.Width(thumbnailSize * 2 + 10f));
        GUILayout.Space(10f);
        EditorGUILayout.LabelField("Vertical", GUILayout.Width(thumbnailSize * 2 + 10f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(60f);
        EditorGUILayout.LabelField("Closed", GUILayout.Width(thumbnailSize));
        EditorGUILayout.LabelField("Open", GUILayout.Width(thumbnailSize));
        GUILayout.Space(10f);
        EditorGUILayout.LabelField("Closed", GUILayout.Width(thumbnailSize));
        EditorGUILayout.LabelField("Open", GUILayout.Width(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Plain", GUILayout.Width(60f));
        tileset.doorSetClosed[0] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetClosed[0], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.doorSetOpen[0] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetOpen[0], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        tileset.doorSetClosed[1] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetClosed[1], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.doorSetOpen[1] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetOpen[1], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Locked_S", GUILayout.Width(60f));
        tileset.doorSetClosed[2] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetClosed[2], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.doorSetOpen[2] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetOpen[2], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        tileset.doorSetClosed[3] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetClosed[3], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.doorSetOpen[3] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetOpen[3], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Locked_G", GUILayout.Width(60f));
        tileset.doorSetClosed[4] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetClosed[4], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.doorSetOpen[4] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetOpen[4], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        tileset.doorSetClosed[5] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetClosed[5], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.doorSetOpen[5] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetOpen[5], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Broken", GUILayout.Width(60f));
        tileset.doorSetClosed[6] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetClosed[6], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.doorSetOpen[6] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetOpen[6], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        GUILayout.Space(10f);
        tileset.doorSetClosed[7] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetClosed[7], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.doorSetOpen[7] = (Sprite)EditorGUILayout.ObjectField(tileset.doorSetOpen[7], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();
    }
}