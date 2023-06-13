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
}
