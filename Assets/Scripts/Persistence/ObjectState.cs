using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ObjectState
{
    public string guid;
    public Vector2 objectPosition;
    public Dictionary<string, object> extendedData;

    public ObjectState()
    {
        GenerateGuid();
        extendedData = new();
    }

    public void GenerateGuid()
    {
        guid = Guid.NewGuid().ToString();
    }

    public void Save(GameObject gameObject)
    {
        objectPosition = gameObject.transform.position;
        gameObject.GetComponent<DynamicObject>()?.PrepareToSave();
    }
}
