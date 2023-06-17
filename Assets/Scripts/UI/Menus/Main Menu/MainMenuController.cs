using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private bool loading;

    [SerializeField]
    private string destinationScene;

    [SerializeField]
    private Button button;

    private void Start()
    {
        CheckContinueAvailable();
    }

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

    private void CheckContinueAvailable()
    {
        string continueScene = SceneLoadManager.Instance.Progress.player.selectedCharacter.scene;
        if (string.IsNullOrEmpty(continueScene))
        {
            button.interactable = false;
        }
    }
}
