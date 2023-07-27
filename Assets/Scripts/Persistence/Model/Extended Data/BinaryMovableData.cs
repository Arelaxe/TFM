using System;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class BinaryMovableData
{
    public float initPositionX;
    public float initPositionY;
    public bool moving;
    public bool atTarget;

    public BinaryMovableData(float initPositionX, float initPositionY, bool moving, bool atTarget)
    {
        this.initPositionX = initPositionX;
        this.initPositionY = initPositionY;
        this.moving = moving;
        this.atTarget = atTarget;
    }

    [JsonIgnore]
    public Vector2 InitPosition { get => new(initPositionX, initPositionY); }
}
