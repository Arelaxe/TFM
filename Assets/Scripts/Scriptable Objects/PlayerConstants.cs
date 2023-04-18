using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class PlayerConstants : ScriptableObject
{
    [Header("Input Actions")]
    [SerializeField]
    private string move;

    public string ActionMove { get => move; }

    [Space]
    [Header("Movement")]
    [SerializeField]
    private float speed;

    public float Speed { get => speed; }
}
