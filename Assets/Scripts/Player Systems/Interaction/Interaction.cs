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
    private bool teamwork;

    [SerializeField]
    private bool available;

    [SerializeField]
    private bool blocked;

    [Space]
    [SerializeField]
    [Tooltip("Required item to execute the interaction.")]
    private Item item;

    [SerializeField]
    [Tooltip("If set to true, the required item will not be consumed when executing the action.")]
    private bool reusable;

    [Space]
    [SerializeField]
    [Tooltip("Name used when the interaction has been executed. If it is executed again, the original name is used.")]
    private string oppositeName;

    [SerializeField]
    [Tooltip("Interaction sets to unavailable when executed.")]
    private bool once;

    private int timesExecuted;

    [Space]
    [SerializeField]
    [Tooltip("Main action to be executed.")]
    private Action action;

    [SerializeField]
    [Tooltip("Actions that will be executed in order after the main one.")]
    private Action[] additionalActions;

    public enum ActionType
    {
        Normal,
        Hacking,
        Spiritual
    }

    public string Name { get => name; }
    public bool IsAvailable { get => available; }
    public bool IsBlocked { get => blocked; }
    public ActionType Type { get => type; }
    public bool RequiredTeamwork { get => teamwork; }
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

    public void SetBlocked(bool blocked)
    {
        this.blocked = blocked;
    }
}
