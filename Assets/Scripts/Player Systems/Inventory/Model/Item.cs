using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item")]
public class Item : ScriptableObject
{
    public int ID => GetInstanceID();
    [field: SerializeField]
    public string Name { get; set; }
    [field: SerializeField]
    [field: TextArea]
    public string Description { get; set; }
    [field: SerializeField]
    public Sprite ItemImage { get; set; }
    [field: SerializeField]
    public ItemType Type = ItemType.Basic;

    public enum ItemType 
    {
        Basic, 
        Document
    }
}
