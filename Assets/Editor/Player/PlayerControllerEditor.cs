using UnityEditor;

[CustomEditor(typeof(DualCharacterController))]
public class PlayerControllerEditor : Editor
{
    private Editor cachedEditor;

    public void OnEnable()
    {
        cachedEditor = null;
    }

    public override void OnInspectorGUI()
    {
        DualCharacterController editedMonobehaviour = (DualCharacterController) target;

        if (cachedEditor == null)
        {
            cachedEditor = CreateEditor(editedMonobehaviour.Params);
        }

        base.OnInspectorGUI();
        cachedEditor.DrawDefaultInspector();
    }
}
