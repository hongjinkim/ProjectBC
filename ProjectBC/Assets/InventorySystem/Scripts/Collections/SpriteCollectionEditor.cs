using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SpriteCollection))]
public class SpriteCollectionEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var spriteCollection = (SpriteCollection)target;

        if (GUILayout.Button("Refresh"))
        {
            Debug.ClearDeveloperConsole();
            SpriteCollectionRefresh.Refresh(spriteCollection);
        }
    }
}