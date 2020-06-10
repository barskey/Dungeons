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

        EditorGUILayout.Space(10f);

        EditorGUILayout.LabelField("Wall Tiles: Outside", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        tileset.wall_LNW = (Sprite)EditorGUILayout.ObjectField(tileset.wall_LNW, typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wall_NS = (Sprite)EditorGUILayout.ObjectField(tileset.wall_NS, typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        tileset.wall_LNE = (Sprite)EditorGUILayout.ObjectField(tileset.wall_LNE, typeof(Sprite), false, GUILayout.Width(thumbnailSize), GUILayout.Height(thumbnailSize));
        EditorGUILayout.EndHorizontal();
    }
}
