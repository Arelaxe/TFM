using System.Collections;
using System.Numerics;
using UnityEngine;

public class MoveRyo : NarrationAction
{
    private int moved;
    [SerializeField]
    private string direction;
    [SerializeField]
    private int times;
    private UnityEngine.Vector2 vector;
    private UnityEngine.Vector2 contraryVector;
    public override void Execute(){
        base.Execute();
        moved = 0;
    }

    public override void EndAction()
    {
        base.EndAction();
        if (direction == "D"){
            vector = UnityEngine.Vector2.down;
            contraryVector = UnityEngine.Vector2.up;
        }
        else if (direction == "L"){
            vector = UnityEngine.Vector2.left;
            contraryVector = UnityEngine.Vector2.right;
        }
        else if (direction == "U"){
            vector = UnityEngine.Vector2.up;
            contraryVector = UnityEngine.Vector2.down;
        }
        PlayerManager.Instance.StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while( moved < times )
        {
            PlayerManager.Instance.GetDualCharacterController().GetRB().velocity = vector * 2.0f;
            moved++;

            if (moved == times){
                PlayerManager.Instance.GetDualCharacterController().GetRB().velocity = contraryVector * 0.0f;
            }

            yield return new WaitForSeconds( 1.0f );
        }
    }
}
