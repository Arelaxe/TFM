using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput input;
    private InputAction pauseAction;
    private InputAction backAction;

    [SerializeField]
    private Image background;

    [SerializeField]
    private GameObject mainPage;

    [SerializeField]
    private GameObject controlsPage;

    [SerializeField]
    private GameObject confirmationMenu;

    [SerializeField]
    private Button resumeButton;

    [SerializeField]
    private Button loadGameButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button noButton;

    private GameObject currentMenu;
    private bool inMainPage = true;

    [SerializeField]
    private TextMeshProUGUI confirmMessage;

    private static string CONFIRM_MESSAGE_SAVE = "¿Seguro que quieres guardar la partida?";
    private static string CONFIRM_MESSAGE_SAVE_AND_QUIT = "¿Seguro que quieres salir del juego? Se guardará el progreso actual.";
    private static string CONFIRM_MESSAGE_LOAD = "¿Seguro que quieres cargar tú última partida guardada? Perderás el progreso actual.";

    void Start()
    {
        InitInputActions();
        CheckLoadGameAvailable();
    }

    void Update()
    {
        if (!SceneLoadManager.Instance.Loading)
        {
            if (pauseAction.triggered && inMainPage)
            {
                Pause(!SceneLoadManager.Instance.Paused);
            }
            if (backAction.triggered)
            {
                Back();
            }
        }
    }

    private void InitInputActions()
    {
        pauseAction = input.actions[PlayerConstants.ActionPause];
        backAction = input.actions[PlayerConstants.ActionCancel];
    }

    private void CheckLoadGameAvailable()
    {
        string savedScene = SceneLoadManager.Instance.Progress.player.selectedCharacter.scene;
        loadGameButton.interactable = !string.IsNullOrEmpty(savedScene);
    }

    public void Resume()
    {
        Pause(false);
    }

    public void Pause(bool pause)
    {
        SceneLoadManager.Instance.Pause(pause);

        background.enabled = pause;
        ShowPage(pause, mainPage, resumeButton);
    }

    public void OpenControlsPage()
    {
        SwitchPage(controlsPage, backButton);
    }

    public void BackToMainPage()
    {
        SwitchPage(mainPage, resumeButton);
    }

    public void Save()
    {
        OpenConfirmationMenu(true, CONFIRM_MESSAGE_SAVE);
    }

    public void Load()
    {
        OpenConfirmationMenu(false, CONFIRM_MESSAGE_LOAD);
    }

    public void SaveAndQuit()
    {
        OpenConfirmationMenu(true, CONFIRM_MESSAGE_SAVE_AND_QUIT, true);
    }

    private void OpenConfirmationMenu(bool save, string message, bool quit = false)
    {
        confirmMessage.text = message;
        yesButton.onClick.RemoveAllListeners();
        if (save)
        {
            yesButton.onClick.AddListener(!quit ? SaveGame : SaveGameAndQuit);
        }
        else {
            yesButton.onClick.AddListener(LoadGame);
        }
        ShowPage(true, confirmationMenu, noButton);
    }

    private void SaveGame()
    {
        PersistenceUtils.Save();
        CheckLoadGameAvailable();
        BackToMainPage();
    }

    private void LoadGame()
    {
        SceneLoadManager sceneLoadManager = SceneLoadManager.Instance;
        if (!sceneLoadManager.Loading)
        {
            Close();
            SceneLoadManager.Instance.LoadFromPause();
        }
    }

    private void SaveGameAndQuit()
    {
        SaveGame();
        Application.Quit();
    }

    // Auxiliar methods
    private void ShowPage(bool show, GameObject page, Button initialButton)
    {
        page.SetActive(show);
        if (show)
        {
            currentMenu = page;
            initialButton.Select();
        }
        inMainPage = page.Equals(mainPage);
    }

    private void SwitchPage(GameObject destinationPage, Button initialButton)
    {
        ShowPage(false, currentMenu, null);
        ShowPage(true, destinationPage, initialButton);
    }

    private void Back()
    {
        if (!currentMenu.Equals(mainPage))
        {
            BackToMainPage();
        }
        else
        {
            Pause(false);
        }
    }

    private void Close()
    {
        BackToMainPage();
        Pause(false);
    }

}
