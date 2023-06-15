using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class ObjectState
{
    public string name;
    public string tag;
    public int layer;
    public float positionX;
    public float positionY;
    public Dictionary<string, object> extendedData = new();

    public void Save(GameObject gameObject)
    {
        name = gameObject.name;
        tag = gameObject.tag;
        layer = gameObject.layer;
        positionX = gameObject.transform.position.x;
        positionY = gameObject.transform.position.y;
        gameObject.GetComponent<DynamicObject>()?.PrepareToSave();
    }

    [JsonIgnore]
    public Vector2 Position { get => new(positionX, positionY); }
}
