using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class PlayerParams : ScriptableObject
{
    [Header("Movement")]
    [SerializeField]
    private float movementSpeed;

    [Header("Interaction")]
    [SerializeField]
    private float interactionRange;

    [Header("Grouping")]
    [SerializeField]
    private float groupingMaxDistance;

    public float Speed { get => movementSpeed; }
    public float InteractionRange { get => interactionRange; }
    public float GroupingMaxDistance { get => groupingMaxDistance; }
}
