using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSet))]
public class TileSetEditor : Editor
{
    private bool showWallImport = false;
    private bool showFloorImport = false;
    private bool showDoorImport = false;

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
        EditorGUILayout.LabelField("Wall Tiles:", EditorStyles.boldLabel);
        GUILayout.Box(Resources.Load<Texture>("Sprites/" + tileset.wallSetName));

        showWallImport = EditorGUILayout.Foldout(showWallImport, "Choose Wall Set");
        if (showWallImport)
        {
            var cnt = 1;
            EditorGUILayout.BeginHorizontal();
            foreach (var item in spriteNames.FindAll(n => n.StartsWith("Wall")))
            {
                if (GUILayout.Button(Resources.Load<Texture>("Sprites/" + item)))
                {
                    SetWallSprites(tileset, item);

                    showWallImport = false;
                }
                if (cnt % 3 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
                cnt++;
            }
            if (cnt % 3 != 0) EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(20f);

        //----- Floor Sprites -----//
        EditorGUILayout.LabelField("Floor Tiles:", EditorStyles.boldLabel);
        GUILayout.Box(Resources.Load<Texture>("Sprites/" + tileset.floorSetName));

        showFloorImport = EditorGUILayout.Foldout(showFloorImport, "Choose Floor Set");
        if (showFloorImport)
        {
            var cnt = 1;
            EditorGUILayout.BeginHorizontal();
            foreach (var item in spriteNames.FindAll(n => n.StartsWith("Floor")))
            {
                if (GUILayout.Button(Resources.Load<Texture>("Sprites/" + item)))
                {
                    SetFloorSprites(tileset, item);

                    showFloorImport = false;
                }
                if (cnt % 3 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
                cnt++;
            }
            if (cnt % 3 != 0) EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(20f);
        
        //----- Door Sprites -----//
        EditorGUILayout.LabelField("Door Tiles:", EditorStyles.boldLabel);
        GUILayout.Box(Resources.Load<Texture>("Sprites/" + tileset.doorSetName));

        showDoorImport = EditorGUILayout.Foldout(showDoorImport, "Choose Door Set");
        if (showDoorImport)
        {
            var cnt = 1;
            EditorGUILayout.BeginHorizontal();
            foreach (var item in spriteNames.FindAll(n => n.StartsWith("Door")))
            {
                if (GUILayout.Button(Resources.Load<Texture>("Sprites/" + item)))
                {
                    SetDoorSprites(tileset, item);

                    showDoorImport = false;
                }
                if (cnt % 2 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
                cnt++;
            }
            if (cnt % 2 != 0) EditorGUILayout.EndHorizontal();
        }
        

    }
    // Sprites will get stored in this order
    //   . n .
    //   w x e
    //   . s .
    // 16 combinations to check, using 4-bit binary in order nesw
    // 1 is wall, 0 is notwall
    // Index     Wall     Floor
    // 0  0000 : O+       C
    // 1  0001 : i+ rot   W
    // 2  0010 : i+ y     S
    // 3  0011 : NE       SW
    // 4  0100 : i+ rot   E
    // 5  0101 : N        S_NS
    // 6  0110 : NW       SE
    // 7  0111 : TN       S_S
    // 8  1000 : i+       N
    // 9  1001 : SE       NW
    // 10 1010 : W        S_WE
    // 11 1011 : TE       S_W
    // 12 1100 : SW       NE
    // 13 1101 : TS       S_N
    // 14 1110 : TW       S_E
    // 15 1111 : T+       S_4

    void SetWallSprites(TileSet set, string spriteName)
    {
        var map = new int[] {6, 5, 3, 8, 7, 10, 8, 14, 15, 11, 12, 9, 13 };
        for (int i = 0; i < map.Length; i++)
        {
            set.wallSet[map[i]] = Resources.Load<Sprite>("Sprites/" + spriteName + "_" + i.ToString());
        }
        set.wallSetName = spriteName;
    }

    void SetFloorSprites(TileSet set, string spriteName)
    {
        var map = new int[] { 9, 8, 12, 13, 15, 1, 0, 4, 5, 11, 10, 14, 3, 2, 6, 7 };
        for (int i = 0; i < map.Length; i++)
        {
            set.floorSet[map[i]] = Resources.Load<Sprite>("Sprites/" + spriteName + "_" + i.ToString());
        }
        set.floorSetName = spriteName;
    }

    void SetDoorSprites(TileSet set, string spriteName)
    {
        for (int i = 0; i < set.doorSetClosed.Length; i++)
        {
            set.doorSetClosed[i] = Resources.Load<Sprite>("Sprites/" + spriteName + "_" + i.ToString());
            set.doorSetOpen[i] = Resources.Load<Sprite>("Sprites/" + spriteName + "_" + (i + 8).ToString());
        }
        set.doorSetName = spriteName;
    }

}