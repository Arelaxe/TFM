using UnityEngine;

public class HideHUDAction : Action
{
    public override void Execute()
    {
        PlayerManager.Instance.GetHUDController().HideHUD();
    }
}
