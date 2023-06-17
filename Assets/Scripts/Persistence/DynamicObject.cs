using UnityEngine;
using System;

[ExecuteInEditMode]
public class DynamicObject : MonoBehaviour
{
    [SerializeField]
    private string guid;

    [SerializeField]
    private ObjectState objectState = new();

    public event Action<ObjectState> OnPrepareToSave, OnLoadObjectState;

    private void Awake()
    {
        if (Application.isEditor && !Application.isPlaying && string.IsNullOrEmpty(guid))
        {
            guid = System.Guid.NewGuid().ToString();
        }
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

    public string Guid { get => guid; }
    public ObjectState State { get => objectState; }
}
