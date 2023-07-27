using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(DynamicObject))]
public class DynamicObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DynamicObject dynamicObject = (DynamicObject)target;
        if (GUILayout.Button("Generate GUID"))
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                dynamicObject.GenerateGUID();
                PrefabUtility.RecordPrefabInstancePropertyModifications(dynamicObject);
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            }
        }

        DrawDefaultInspector();
    }
}
