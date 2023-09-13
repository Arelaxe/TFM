using UnityEngine;

public class ClearDocumentationAction : Action
{
    public override void Execute()
    {
        PlayerManager.Instance.GetDocumentationController().Clear();
    }
}
