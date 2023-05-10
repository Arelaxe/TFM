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

    [Header("Inventory")]
    [SerializeField]
    private int inventorySize;

    public float Speed { get => movementSpeed; }
    public float InteractionRange { get => interactionRange; }
    public float GroupingMaxDistance { get => groupingMaxDistance; }
    public int InventorySize { get => inventorySize; }
}
