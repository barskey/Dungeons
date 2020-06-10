using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSet))]
public class TileSetEditor : Editor
{
    float thumbnailSize = 48f;

    //TileSet tileset = (TileSet);
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        TileSet tileset = (TileSet)target;

        GUILayout.Space(10f);

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
        EditorGUILayout.LabelField("Wall Tiles: Outside", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        tileset.wallSet[6] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[6], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[5] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[5], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[3] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[3], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        tileset.wallSet[10] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[10], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[0] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[0], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[10] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[10], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        tileset.wallSet[12] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[12], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[5] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[5], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[9] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[9], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Wall Tiles: Inside", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        tileset.wallSet[15] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[15], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[7] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[7], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        tileset.wallSet[14] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[14], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[8] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[8], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wallSet[11] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[11], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(thumbnailSize);
        tileset.wallSet[8] = (Sprite)EditorGUILayout.ObjectField(tileset.wallSet[8], typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();
    }
}
