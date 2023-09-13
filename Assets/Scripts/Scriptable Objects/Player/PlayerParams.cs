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

    [SerializeField]
    private float distanceOnLoadScene;

    [Header("Inventory")]
    [SerializeField]
    private int inventorySize;

    public float Speed { get => movementSpeed; }
    public float InteractionRange { get => interactionRange; }
    public float GroupingMaxDistance { get => groupingMaxDistance; }
    public float DistanceOnLoadScene { get => distanceOnLoadScene; }
    public int InventorySize { get => inventorySize; }
}
