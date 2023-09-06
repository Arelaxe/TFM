using UnityEngine;
using System;

public class HackingAction : Action
{
    [SerializeField]
    private string hackingScene;

    public HackingStatus status = HackingStatus.Failed;

    [Serializable]
    public enum HackingStatus
    {
        Failed = 0,
        Completed = 1,
        Maximum = 2
    }

    public static readonly string ObjectName = "hackingAction";

    public override void Execute()
    {
        PlayerManager.Instance.GetInGameMenuController().ShowHackingExtension(hackingScene, gameObject);

        bool inventoryEmpty = PlayerManager.Instance.GetInventoryController().IsCharacterInventoryEmpty(PlayerManager.Instance.SelectedCharacterOne);
        if (!inventoryEmpty)
        {
            PlayerManager.Instance.GetInventoryController().Show();
        }
        else
        {
            PlayerManager.Instance.GetDocumentationController().Show();
        }

        PlayerManager.Instance.GetDualCharacterController().SetSwitchAvailability(false);
    }

    protected override void PrepareToSaveObjectState(ObjectState objectState)
    {
        objectState.extendedData[GetPersistentName()] = new HackingData(timesExecuted, status);
    }

    protected override void LoadObjectState(ObjectState objectState)
    {
        HackingData hackingData = PersistenceUtils.Get<HackingData>(objectState.extendedData[GetPersistentName()]);
        timesExecuted = hackingData.timesExecuted;
        status = hackingData.status;
    }
}
