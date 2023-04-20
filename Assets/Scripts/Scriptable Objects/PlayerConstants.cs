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

    [SerializeField]
    private string split;

    public string ActionMove { get => move; }
    public string ActionSplit { get => split; }

    [Space]
    [Header("Movement")]
    [SerializeField]
    private float speed;

    public float Speed { get => speed; }

    [Space]
    [Header("Grouping")]
    [SerializeField]
    private float groupingMaxDistance;

    public float GroupingMaxDistance { get => groupingMaxDistance; }
}
