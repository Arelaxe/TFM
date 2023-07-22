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
    private static readonly string Status = "hackingStatus";

    private void Start()
    {
        InitDynamicObject();
    }

    public override void Execute()
    {
        PlayerManager.Instance.GetInventoryController().Show();
        PlayerManager.Instance.GetDualCharacterController().SetSwitchAvailability(false);
        PlayerManager.Instance.GetInGameMenuController().ShowHackingExtension(hackingScene, gameObject);
    }

    private void InitDynamicObject()
    {
        DynamicObject dynamicObject = GetComponent<DynamicObject>();
        dynamicObject.OnPrepareToSave += PrepareToSaveObjectState;
        dynamicObject.OnLoadObjectState += LoadObjectState;
    }

    private void PrepareToSaveObjectState(ObjectState objectState)
    {
        objectState.extendedData[Status] = status;
    }

    private void LoadObjectState(ObjectState objectState)
    {
        status = PersistenceUtils.Get<HackingStatus>(objectState.extendedData[Status]);
    }
}
