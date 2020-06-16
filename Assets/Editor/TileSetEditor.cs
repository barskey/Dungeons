using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSet))]
public class TileSetEditor : Editor
{
    private SerializedProperty wallSet;
    private SerializedProperty wallSetName;
    private SerializedProperty floorSet;
    private SerializedProperty floorSetName;
    private SerializedProperty doorSetOpen;
    private SerializedProperty doorSetClosed;
    private SerializedProperty doorSetName;

    List<string> spriteNames = new List<string>();
    Sprite[] sprites;

    private bool showWallImport = false;
    private bool showFloorImport = false;
    private bool showDoorImport = false;


    private void OnEnable()
    {
        wallSet = serializedObject.FindProperty("wallSet");
        wallSetName = serializedObject.FindProperty("wallSetName");
        floorSet = serializedObject.FindProperty("floorSet");
        floorSetName = serializedObject.FindProperty("floorSetName");
        doorSetOpen = serializedObject.FindProperty("doorSetOpen");
        doorSetClosed = serializedObject.FindProperty("doorSetClosed");
        doorSetName = serializedObject.FindProperty("doorSetName");

        sprites = Resources.LoadAll<Sprite>("Sprites");
        for (int i = 0; i < sprites.Length; i++)
        {
            var name = sprites[i].name.Split('_');
            if (!spriteNames.Contains(name[0])) spriteNames.Add(name[0]);
        }
    }

    //TileSet tileset = (TileSet);
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        //----- Wall Sprites -----//
        EditorGUILayout.LabelField("Wall Tiles:", EditorStyles.boldLabel);
        GUILayout.Box(Resources.Load<Texture>("Sprites/" + wallSetName.stringValue));

        showWallImport = EditorGUILayout.Foldout(showWallImport, "Choose Wall Set");
        if (showWallImport)
        {
            var cnt = 1;
            EditorGUILayout.BeginHorizontal();
            foreach (var item in spriteNames.FindAll(n => n.StartsWith("Wall")))
            {
                if (GUILayout.Button(Resources.Load<Texture>("Sprites/" + item)))
                {
                    SetWallSprites(item);
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
        GUILayout.Box(Resources.Load<Texture>("Sprites/" + floorSetName.stringValue));

        showFloorImport = EditorGUILayout.Foldout(showFloorImport, "Choose Floor Set");
        if (showFloorImport)
        {
            var cnt = 1;
            EditorGUILayout.BeginHorizontal();
            foreach (var item in spriteNames.FindAll(n => n.StartsWith("Floor")))
            {
                if (GUILayout.Button(Resources.Load<Texture>("Sprites/" + item)))
                {
                    SetFloorSprites(item);
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
        GUILayout.Box(Resources.Load<Texture>("Sprites/" + doorSetName.stringValue));

        showDoorImport = EditorGUILayout.Foldout(showDoorImport, "Choose Door Set");
        if (showDoorImport)
        {
            var cnt = 1;
            EditorGUILayout.BeginHorizontal();
            foreach (var item in spriteNames.FindAll(n => n.StartsWith("Door")))
            {
                if (GUILayout.Button(Resources.Load<Texture>("Sprites/" + item)))
                {
                    SetDoorSprites(item);
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

        serializedObject.ApplyModifiedProperties();
    }
    // Sprites will get stored in this order
    //   . n .
    //   w x e
    //   . s .
    // 16 combinations to check, using 4-bit binary in order nesw
    // 1 is wall, 0 is notwall
    // Index     Wall     Floor
    // 0  0000 : O+       C
    // 1  0001 : N        W
    // 2  0010 : NW       S
    // 3  0011 : NE       SW
    // 4  0100 : N        E
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

    int GetIndex(string n)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name == n) return i;
        }
        return -1;
    }

    void SetWallSprites(string spriteName)
    {
        // map the sprite names to their index in array above
        // e.g. index 0 above is O+, hence it would map to wall_3
        var map = new int[] {3, 1, 0, 2, 1, 1, 0, 4, 6, 11, 5, 9, 10, 12, 7, 8 };
        var i = 0;
        foreach (int mapi in map)
        {
            var index = GetIndex(spriteName + "_" + mapi.ToString());
            if (index >= 0)
            {
                wallSet.GetArrayElementAtIndex(i).objectReferenceValue = sprites[index];
                wallSetName.stringValue = spriteName;
            }
            i++;
        }
    }

    void SetFloorSprites(string spriteName)
    {
        var map = new int[] { 9, 8, 12, 13, 15, 1, 0, 4, 5, 11, 10, 14, 3, 2, 6, 7 };
        for (int i = 0; i < map.Length; i++)
        {
            var index = GetIndex(spriteName + "_" + i.ToString());
            if (index >= 0)
            {
                floorSet.GetArrayElementAtIndex(map[i]).objectReferenceValue = sprites[index];
                floorSetName.stringValue = spriteName;
            }
        }
    }

    void SetDoorSprites(string spriteName)
    {
        for (int i = 0; i < doorSetClosed.arraySize; i++)
        {
            var index = GetIndex(spriteName + "_" + i.ToString());
            if (index >= 0)
            {
                doorSetClosed.GetArrayElementAtIndex(i).objectReferenceValue = sprites[index];
                doorSetName.stringValue = spriteName;
            }
            index = GetIndex(spriteName + "_" + (i + 8).ToString());
            if (index >= 0)
            {
                doorSetOpen.GetArrayElementAtIndex(i).objectReferenceValue = sprites[index];
                doorSetName.stringValue = spriteName;
            }
        }
    }

}