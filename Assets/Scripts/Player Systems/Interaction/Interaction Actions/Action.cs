using UnityEngine;

public abstract class Action : MonoBehaviour
{
    [SerializeField]
    protected bool once;
    protected int timesExecuted = 0;

    public void DoAction()
    {
        if (!once || once && timesExecuted == 0)
        {
            Execute();
            timesExecuted++;
        }
    }

    public abstract void Execute();
}
