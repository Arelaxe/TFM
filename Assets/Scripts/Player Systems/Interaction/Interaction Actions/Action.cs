using UnityEngine;
using System;

[RequireComponent(typeof(DynamicObject))]
[ExecuteInEditMode]
public abstract class Action : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Required if there is more than one action of the same type on the GameObject")]
    private string guid;

    [Space]
    [SerializeField]
    [Tooltip("Action will be executed just once.")]
    protected bool once;
    protected int timesExecuted = 0;

    private void Start()
    {
        InitDynamicObject();
    }

    private void InitDynamicObject()
    {
        DynamicObject dynamicObject = GetComponent<DynamicObject>();
        dynamicObject.OnPrepareToSave += PrepareToSaveObjectState;
        dynamicObject.OnLoadObjectState += LoadObjectState;
    }

    protected virtual void PrepareToSaveObjectState(ObjectState objectState)
    {
        objectState.extendedData[GetPersistentName()] = new ActionData(timesExecuted);
    }

    protected virtual void LoadObjectState(ObjectState objectState)
    {
        timesExecuted = PersistenceUtils.Get<ActionData>(objectState.extendedData[GetPersistentName()]).timesExecuted;
    }

    protected string GetPersistentName()
    {
        string persistenName = GetType().Name;
        if (!string.IsNullOrEmpty(guid))
        {
            persistenName += "-" + guid;
        }
        return persistenName;
    }

    public void GenerateGUID()
    {
        guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("==", "");
    }

    public void DoAction()
    {
        if (!once || once && timesExecuted == 0)
        {
            Execute();
            timesExecuted++;
        }
    }

    public abstract void Execute();
}
