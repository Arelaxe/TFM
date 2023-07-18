
public class HackingAction : Action
{
    public override void Execute()
    {
        PlayerManager.Instance.GetInventoryController().Show();
        PlayerManager.Instance.GetDualCharacterController().SetSwitchAvailability(false);
        PlayerManager.Instance.GetInGameMenuController().ShowHackingExtension(true);
    }
}
