using System;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class TriggerData
{
    public bool enabled;
    public float colliderOffsetX;
    public float colliderOffsetY;
    public float colliderSizeX;
    public float colliderSizeY;

    public TriggerData(bool enabled, float colliderOffsetX, float colliderOffsetY, float colliderSizeX, float colliderSizeY)
    {
        this.enabled = enabled;
        this.colliderOffsetX = colliderOffsetX;
        this.colliderOffsetY = colliderOffsetY;
        this.colliderSizeX = colliderSizeX;
        this.colliderSizeY = colliderSizeY;
}

    [JsonIgnore]
    public Vector2 Offset { get => new(colliderOffsetX, colliderOffsetY); }
    [JsonIgnore]
    public Vector2 Size { get => new(colliderSizeX, colliderSizeY); }
}
