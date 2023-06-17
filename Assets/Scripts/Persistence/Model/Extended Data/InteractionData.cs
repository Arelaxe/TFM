using System;

[Serializable]
public class InteractionData
{
    public string name;
    public bool available;

    public InteractionData(string name, bool available)
    {
        this.name = name;
        this.available = available;
    }
}
