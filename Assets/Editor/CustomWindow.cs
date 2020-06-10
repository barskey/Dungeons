using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class CustomWindow : EditorWindow
{

    [MenuItem("Dungeons/CreateTileSet")]
    public static void ShowWindow()
    {
        GetWindow<CustomWindow>("TileSet Editor");
    }

}
