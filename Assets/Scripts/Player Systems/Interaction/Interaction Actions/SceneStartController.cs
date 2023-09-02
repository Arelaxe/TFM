using UnityEngine;

public class SceneStartController : MonoBehaviour
{
    [SerializeField]
    private Action[] actions;

    void Start()
    {
        foreach (Action action in actions)
        {
            action.DoAction();
        }
    }

}
