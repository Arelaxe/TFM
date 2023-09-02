using UnityEngine;

public class ClearInventoryAction : Action
{
    public override void Execute()
    {
        PlayerManager.Instance.GetInventoryController().Clear();
    }
}
