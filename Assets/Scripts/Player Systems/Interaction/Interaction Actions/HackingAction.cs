using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HackingAction : Action
{
    public override void Execute()
    {
        PlayerManager.Instance.GetInventoryController().Show();
        PlayerManager.Instance.GetDualCharacterController().SetSwitchAvailability(false);
        PlayerManager.Instance.GetInGameMenuController().ShowHackingExtension(true);
    }
}
