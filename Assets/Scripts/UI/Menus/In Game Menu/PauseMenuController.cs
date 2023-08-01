using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    private GameObject optionsPage;

    [SerializeField]
    private GameObject confirmationMenu;

    [SerializeField]
    private GameObject cantSavePopup;

    [SerializeField]
    private Button resumeButton;

    [SerializeField]
    private Button saveGameButton;

    [SerializeField]
    private Button loadGameButton;

    [SerializeField]
    private Button saveAndQuitGameButton;

    [SerializeField]
    private Button firstOptionButton;

    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button noButton;

    [SerializeField]
    private Button closeButton;

    private GameObject currentMenu;
    private bool inMainPage = true;

    [SerializeField]
    private TextMeshProUGUI confirmMessage;

    private GameObject lastSelected;

    private static string CONFIRM_MESSAGE_SAVE = "¿Seguro que quieres guardar la partida?";
    private static string CONFIRM_MESSAGE_SAVE_AND_QUIT = "¿Seguro que quieres salir del juego? Se guardará el progreso actual.";
    private static string CONFIRM_MESSAGE_LOAD = "¿Seguro que quieres cargar tu última partida guardada? Perderás el progreso actual.";

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

    private void CheckSaveGameAvailable()
    {
        saveGameButton.interactable = !SceneLoadManager.Instance.InAdditive;
        saveAndQuitGameButton.interactable = !SceneLoadManager.Instance.InAdditive;
    }

    private void CheckLoadGameAvailable()
    {
        string savedScene = SceneLoadManager.Instance.Progress.player.selectedCharacter.scene;
        loadGameButton.interactable = !string.IsNullOrEmpty(savedScene) && !SceneLoadManager.Instance.Progress.newGame;
    }

    public void Resume()
    {
        Pause(false);
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
            SoundManager.Instance.UpdateMusicVolume(50);
        }

        SceneLoadManager.Instance.Pause(pause);

        background.enabled = pause;

        CheckSaveGameAvailable();
        ShowPage(pause, mainPage, resumeButton);

        if (!pause)
        {
            SoundManager.Instance.BackToDefaultMusicVolume();
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }

    public void OpenOptionsPage()
    {
        SwitchPage(optionsPage, firstOptionButton, true);
    }

    public void BackToMainPage()
    {
        SwitchPage(mainPage, resumeButton);
    }

    public void Save()
    {
        if (CanSaveOnScene()){
            OpenConfirmationMenu(true, CONFIRM_MESSAGE_SAVE);
        }
        else{
            OpenCantSavePopup();
        }
    }

    public void Load()
    {
        OpenConfirmationMenu(false, CONFIRM_MESSAGE_LOAD);
    }

    public void SaveAndQuit()
    {
        if (this.CanSaveOnScene()){
            OpenConfirmationMenu(true, CONFIRM_MESSAGE_SAVE_AND_QUIT, true);
        }
        else{
            OpenCantSavePopup();
        }
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

    private void OpenCantSavePopup(){
        ShowPage(true, cantSavePopup, closeButton);
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
    private void ShowPage(bool show, GameObject page, Button initialButton, bool invokeClick = false)
    {
        page.SetActive(show);
        if (show)
        {
            currentMenu = page;
            initialButton.Select();
            if (invokeClick)
            {
                initialButton.onClick.Invoke();
            }
        }
        inMainPage = page.Equals(mainPage);
    }

    private void SwitchPage(GameObject destinationPage, Button initialButton, bool invokeClick = false)
    {
        ShowPage(false, currentMenu, null);
        ShowPage(true, destinationPage, initialButton, invokeClick);
    }

    private void Back()
    {
        if (currentMenu != null)
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
    }

    private void Close()
    {
        BackToMainPage();
        Pause(false);
    }

    private bool CanSaveOnScene(){
        return SceneManager.GetActiveScene().name != "TutorialScene" && SceneManager.GetActiveScene().name != "IntroScene";
    }

}
