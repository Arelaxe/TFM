using UnityEngine;
using System.Collections.Generic;
using System;

public class OpenDeviceAction : Action
{
    [SerializeField]
    private string deviceScene;

    [SerializeField]
    private HackingAction hackingAction;

    public override void Execute()
    {
        PlayerControl();

        Dictionary<string, UnityEngine.Object> deviceData = new();
        deviceData.Add(HackingAction.ObjectName, hackingAction);
        SceneLoadManager.Instance.LoadAdditiveScene(deviceScene, deviceData);
    }

    private void PlayerControl()
    {
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();

        dualCharacterController.SetCharacterMobility(true, false);
        dualCharacterController.SetSwitchAvailability(false);
        interactionController.SetInteractivity(false);
        interactionController.DestroyInteractions();
    }

}
