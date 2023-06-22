using UnityEngine;
using System;

[Serializable]
public class Interaction
{
    [SerializeField]
    private string name;

    [SerializeField]
    private ActionType type;

    [SerializeField]
    private bool available;

    [SerializeField]
    private Item item;

    [SerializeField]
    private bool reusable;

    [Space]
    [SerializeField]
    private Action action;

    public enum ActionType
    {
        Normal,
        Hacking,
        Spiritual
    }

    public string Name { get => name; }
    public bool IsAvailable { get => available; }
    public ActionType Type { get => type; }
    public Item RequiredItem { get => item; }
    public bool Reusable { get => reusable; }
    public Action Action { get => action; }

    public void SetAvailable(bool available)
    {
        this.available = available;
    }
}
