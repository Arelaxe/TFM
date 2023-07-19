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
        Failed,
        Completed,
        Maximum
    }

    private static readonly string HACKING_STATUS = "hackingStatus";

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
        objectState.extendedData[HACKING_STATUS] = status;
    }

    private void LoadObjectState(ObjectState objectState)
    {
        status = PersistenceUtils.Get<HackingStatus>(objectState.extendedData[HACKING_STATUS]);
    }
}
