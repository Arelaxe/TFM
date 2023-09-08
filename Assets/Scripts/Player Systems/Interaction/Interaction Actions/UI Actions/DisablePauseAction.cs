using UnityEngine;

public class DisablePauseAction : Action
{
    public override void Execute()
    {
        SceneLoadManager.Instance.Pausable = false;
    }
}
