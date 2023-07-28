using System;

[Serializable]
public class HackingData : ActionData
{
    public HackingAction.HackingStatus status;

    public HackingData(int timesExecuted, HackingAction.HackingStatus status) : base(timesExecuted)
    {
        this.status = status;
    }

}
