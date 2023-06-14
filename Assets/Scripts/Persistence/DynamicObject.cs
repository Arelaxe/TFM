using UnityEngine;
using System;

[ExecuteInEditMode]
public class DynamicObject : MonoBehaviour
{
    public ObjectState objectState = new();
    private bool initGuid = false;

    public event Action<ObjectState> OnPrepareToSave, OnLoadObjectState;

    private void Awake()
    {
        if (Application.isEditor && !Application.isPlaying && !initGuid)
        {
            GenerateGuid();
            initGuid = true;
        }
    }

    public void GenerateGuid()
    {
        objectState.GenerateGuid();
    }

    public void Load(ObjectState objectState)
    {
        this.objectState = objectState;
        OnLoadObjectState?.Invoke(objectState);
    }

    public void PrepareToSave()
    {
        OnPrepareToSave?.Invoke(objectState);
    }
}
