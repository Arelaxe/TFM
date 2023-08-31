using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Action), true)]
public class ActionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Action action = (Action)target;
        if (GUILayout.Button("Generate GUID"))
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                action.GenerateGUID();
                PrefabUtility.RecordPrefabInstancePropertyModifications(action);
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            }
        }

        DrawDefaultInspector();
    }
}
