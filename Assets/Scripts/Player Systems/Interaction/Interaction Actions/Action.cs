using UnityEngine;

[RequireComponent(typeof(DynamicObject))]
public abstract class Action : MonoBehaviour
{
    [SerializeField]
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
        return GetType().Name;
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
