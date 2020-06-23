using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

#region Camera Marker Editor
[CustomEditor(typeof(MagicMenu_CameraMarker))]
public class MagicMenu_Editor_CamMarker : Editor
{
    public override void OnInspectorGUI()
    {
        MagicMenu_CameraMarker instance = (MagicMenu_CameraMarker)target;
        DrawDefaultInspector();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        
        //GUILayout.Label("Add Flag", EditorStyles.boldLabel);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        //GUILayout.Label("Name:", EditorStyles.boldLabel);
        //var newContentText = GUILayout.TextArea(instance.textBox, GUILayout.Width(150), GUILayout.Height(25));
        //if (instance.textBox != newContentText.ToString())
        //    instance.textBox = newContentText.ToString();

        if (GUILayout.Button("Add Flag", GUILayout.Width(150), GUILayout.Height(25)))
        {
            instance.SpawnFlag();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

    }
}
#endregion
#region Flag Editor
[CustomEditor(typeof(MagicMenuFlag))]
public class MagicMenu_Editor_Flag : Editor
{
    public override void OnInspectorGUI()
    {
        MagicMenuFlag instance = (MagicMenuFlag)target;
        DrawDefaultInspector();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();

        //GUILayout.Label("Add Flag", EditorStyles.boldLabel);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        //GUILayout.Label("Name:", EditorStyles.boldLabel);
        //var newContentText = GUILayout.TextArea(instance.textBox, GUILayout.Width(150), GUILayout.Height(25));
        //if (instance.textBox != newContentText.ToString())
        //    instance.textBox = newContentText.ToString();

        if (GUILayout.Button("Add Text Object", GUILayout.Width(150), GUILayout.Height(25)))
        {
            instance.SpawnObject();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

    }
}
#endregion

#endif
