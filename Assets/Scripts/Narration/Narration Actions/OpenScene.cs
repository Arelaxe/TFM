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

        if (!PlayerManager.Instance.GetDualCharacterController().Grouped){
            PlayerManager.Instance.GetDualCharacterController().SwitchGrouping();
        }

        PlayerManager.Instance.GetHUDController().EnableHUD();
        SceneLoadManager.Instance.LoadScene(sceneName, -1, true);
    }
}
