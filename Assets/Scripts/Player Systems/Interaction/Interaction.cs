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

    [Space]
    [SerializeField]
    private Item item;

    [SerializeField]
    private bool reusable;

    [Space]
    [SerializeField]
    private string oppositeName;

    [SerializeField]
    private bool once;

    private int timesExecuted;

    [Space]
    [SerializeField]
    private Action action;

    [SerializeField]
    private Action[] additionalActions;

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
    public bool Once { get => once; }
    public int TimesExecuted { get => timesExecuted; }
    public string OppositeName { get => oppositeName; }
    public Action Action { get => action; }
    public Action[] AdditionalActions { get => additionalActions; }

    public bool IsOppositeStatus()
    {
        return !string.IsNullOrEmpty(oppositeName) && timesExecuted % 2 != 0;
    }

    public void IncreaseTimesExecuted()
    {
        timesExecuted++;
    }

    public void SetExecuted(int timesExecuted)
    {
        this.timesExecuted = timesExecuted;
    }

    public void SetAvailable(bool available)
    {
        this.available = available;
    }
}
