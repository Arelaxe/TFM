using System;

[Serializable]
public class InteractionData
{
    public string name;
    public bool available;
    public int timesExecuted;

    public InteractionData(string name, bool available, int timesExecuted)
    {
        this.name = name;
        this.available = available;
        this.timesExecuted = timesExecuted;
    }
}
