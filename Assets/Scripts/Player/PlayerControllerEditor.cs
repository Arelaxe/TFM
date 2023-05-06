using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    private Editor cachedEditor;

    public void OnEnable()
    {
        cachedEditor = null;
    }

    public override void OnInspectorGUI()
    {
        PlayerController editedMonobehaviour = (PlayerController) target;

        if (cachedEditor == null)
        {
            cachedEditor = CreateEditor(editedMonobehaviour.PlayerParams);
        }

        base.OnInspectorGUI();
        cachedEditor.DrawDefaultInspector();
    }
}
