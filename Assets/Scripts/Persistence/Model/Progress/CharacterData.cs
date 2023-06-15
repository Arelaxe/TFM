using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
public class CharacterData
{
    public string scene;
    public float positionX;
    public float positionY;
    public bool verticalMovement;
    public bool positiveMovement;

    [JsonIgnore]
    public Vector2 Position { get => new(positionX, positionY); }

    [JsonIgnore]
    public Tuple<bool, bool> LookingAt { get => new(verticalMovement, positiveMovement); }

    public void Save(string scene, Vector2 position, Tuple<bool, bool> lookingAt)
    {
        this.scene = scene;
        positionX = position.x;
        positionY = position.y;
        verticalMovement = lookingAt.Item1;
        positiveMovement = lookingAt.Item2;
    }
}
