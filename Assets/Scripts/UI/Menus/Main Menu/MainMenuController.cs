using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private bool loading;

    [SerializeField]
    private string destinationScene; 

    public void LoadScene()
    {
        if (!loading)
        {
            loading = true;
            SceneLoadManager.Instance.LoadSceneFromMenu(destinationScene);
        }
    }

    public void Continue()
    {
        if (!loading)
        {
            loading = true;
            string continueScene = SceneLoadManager.Instance.Progress.player.selectedCharacter.scene;
            SceneLoadManager.Instance.LoadSceneFromMenu(continueScene, false);
        }
    }
}
