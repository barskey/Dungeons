using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomWindow : EditorWindow
{

    [MenuItem("Dungeons/Popup Example")]
    static void Init()
    {
        CustomWindow window = ScriptableObject.CreateInstance<CustomWindow>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("My window");
    }

}
