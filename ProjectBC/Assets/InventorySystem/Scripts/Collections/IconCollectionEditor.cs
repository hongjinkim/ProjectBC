
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IconCollection))]
public class IconCollectionEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var collection = (IconCollection)target;

        if (GUILayout.Button("Refresh"))
        {
            collection.Refresh();
        }
    }
}
