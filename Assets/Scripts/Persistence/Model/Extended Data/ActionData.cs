using System;

[Serializable]
public class ActionData
{
    public int timesExecuted;

    public ActionData(int timesExecuted)
    {
        this.timesExecuted = timesExecuted;
    }

}
