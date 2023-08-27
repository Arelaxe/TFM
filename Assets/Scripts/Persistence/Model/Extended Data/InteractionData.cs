using System;

[Serializable]
public class InteractionData
{
    public string name;
    public bool available;
    public bool blocked;
    public int timesExecuted;

    public InteractionData(string name, bool available, bool blocked, int timesExecuted)
    {
        this.name = name;
        this.available = available;
        this.blocked = blocked;
        this.timesExecuted = timesExecuted;
    }
}
