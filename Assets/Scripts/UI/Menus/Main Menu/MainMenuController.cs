using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private PlayerInput input;
    private InputAction backAction;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject controlsMenu;

    [SerializeField]
    private GameObject confirmationMenu;

    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button noButton;

    [SerializeField]
    private string newGameScene;

    private void Start()
    {
        InitInputActions();
        CheckContinueAvailable();
    }

    private void Update()
    {
        if (backAction.triggered)
        {
            if (controlsMenu.activeSelf)
            {
                CloseControlsMenu();
            }
            else if (confirmationMenu.activeSelf)
            {
                CloseConfirmationMenu();
            }
        }
    }

    private void InitInputActions()
    {
        input = GetComponent<PlayerInput>();
        backAction = input.actions[PlayerConstants.ActionCancel];
    }

    public void NewGame()
    {
        if (continueButton.interactable)
        {
            OpenConfirmationMenu();
        }
        else {
            StartNewGame();
        }
    }

    private void OpenConfirmationMenu()
    {
        confirmationMenu.SetActive(true);
        noButton.Select();
    }

    public void CloseConfirmationMenu()
    {
        confirmationMenu.SetActive(false);
        newGameButton.Select();
    }

    public void StartNewGame()
    {
        if (!SceneLoadManager.Instance.Loading)
        {
            PersistenceUtils.ClearSave();
            SceneLoadManager.Instance.LoadSceneFromMenu(newGameScene);
        }
    }

    public void Continue()
    {
        if (!SceneLoadManager.Instance.Loading)
        {
            string continueScene = SceneLoadManager.Instance.Progress.player.selectedCharacter.scene;
            SceneLoadManager.Instance.LoadSceneFromMenu(continueScene, false);
        }
    }

    public void OpenControlsMenu()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
        backButton.Select();
    }

    public void CloseControlsMenu()
    {
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
        newGameButton.Select();
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void CheckContinueAvailable()
    {
        string continueScene = SceneLoadManager.Instance.Progress.player.selectedCharacter.scene;
        continueButton.interactable = !string.IsNullOrEmpty(continueScene);
    }
}
