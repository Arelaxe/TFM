using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : NarrationAction
{
    [SerializeField]
    private string sceneName;
    public override void Execute() { 
        base.Execute();
    }

    public override void EndAction()
    {
        base.EndAction();
        SceneLoadManager.Instance.LoadScene(sceneName);
    }
}
