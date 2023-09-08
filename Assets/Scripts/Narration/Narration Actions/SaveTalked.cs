using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTalked : NarrationAction
{
    public override void Execute(){
        base.Execute();
        SceneLoadManager.Instance.SaveKeyAction(KeyActions.TalkedToNPC, "completed");
    }

    public override void EndAction()
    {
        base.EndAction();
    }
}
