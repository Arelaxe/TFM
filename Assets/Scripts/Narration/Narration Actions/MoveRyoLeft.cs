using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRyoLeft : NarrationAction
{
    private int moved;
    public override void Execute(){
        base.Execute();
        moved = 0;
    }

    public override void EndAction()
    {
        base.EndAction();
        PlayerManager.Instance.StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while( moved < 3 )
        {
            PlayerManager.Instance.GetDualCharacterController().GetRB().velocity = Vector2.left * 2.0f;
            moved++;

            if (moved == 3){
                PlayerManager.Instance.GetDualCharacterController().GetRB().velocity = Vector2.up * 0.0f;
            }

            yield return new WaitForSeconds( 1.0f );
        }
    }
}
