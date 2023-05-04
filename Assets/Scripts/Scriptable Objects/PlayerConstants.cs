using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class PlayerConstants : ScriptableObject
{
    [Header("Movement")]
    [SerializeField]
    private float speed;

    public float Speed { get => speed; }

    [Space]
    [Header("Grouping")]
    [SerializeField]
    private float groupingMaxDistance;

    public float GroupingMaxDistance { get => groupingMaxDistance; }

    [Space]
    [SerializeField]
    private PlayerUtilsConstants utilsConstants;

    public string ActionMove { get => utilsConstants.ActionMove; }
    public string ActionSplit { get => utilsConstants.ActionSplit; }

    public string CameraStateRyo { get => utilsConstants.CameraStateRyo; }
    public string CameraStateShinen { get => utilsConstants.CameraStateShinen; }

    public string AnimParamVelocity { get => utilsConstants.AnimParamVelocity; }
    public string AnimParamVerticalMovement { get => utilsConstants.AnimParamVerticalMovement; }
    public string AnimParamPositiveMovement { get => utilsConstants.AnimParamPositiveMovement; }
}
