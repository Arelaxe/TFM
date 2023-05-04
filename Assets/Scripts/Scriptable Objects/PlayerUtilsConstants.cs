using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class PlayerUtilsConstants : ScriptableObject
{
    [Header("Input Actions")]
    [SerializeField]
    private string move;

    [SerializeField]
    private string split;

    public string ActionMove { get => move; }
    public string ActionSplit { get => split; }

    [Header("Camera states")]
    [SerializeField]
    private string ryoCamera;

    [SerializeField]
    private string shinenCamera;

    public string CameraStateRyo { get => ryoCamera; }
    public string CameraStateShinen { get => shinenCamera; }

    [Header("Animation Params")]
    [SerializeField]
    private string velocity;

    [SerializeField]
    private string verticalMovement;

    [SerializeField]
    private string positiveMovement;

    public string AnimParamVelocity { get => velocity; }
    public string AnimParamVerticalMovement { get => verticalMovement; }
    public string AnimParamPositiveMovement { get => positiveMovement; }
}
