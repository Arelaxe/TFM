using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public abstract class DeviceIcon : Selectable, IPointerClickHandler, ISubmitHandler
{
    [SerializeField]
    protected AccessLevel accessLevel = AccessLevel.Low;

    [Serializable]
    public enum AccessLevel
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    protected DeviceManager manager;
    protected bool submit = false;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        submit = false;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        submit = false;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (!submit)
        {
            submit = true;
        }
        else
        {
            submit = false;
            Execute();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (eventData.clickCount > 1)
            {
                Execute();
            }
        }
    }

    protected DeviceManager GetManager()
    {
        if (!manager)
        {
            manager = GameObject.FindGameObjectWithTag(GlobalConstants.TagDeviceManager).GetComponent<DeviceManager>();
        }
        return manager;
    }

    public abstract void Execute();

}
