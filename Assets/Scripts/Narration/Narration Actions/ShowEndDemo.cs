using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEndDemo : NarrationAction
{
    public override void Execute()
    {
        base.Execute();
    }

    public override void EndAction()
    {
        base.EndAction();
        SceneLoadManager.Instance.LoadAdditiveScene("EndScene");
    }
}
