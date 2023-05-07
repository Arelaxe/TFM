using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interaction
{
    [SerializeField]
    private string name;

    [SerializeField]
    private ActionType type;

    [SerializeField]
    private bool available;

    [SerializeField]
    private string requiredItem;

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

    public Action Action { get => action; }
}
