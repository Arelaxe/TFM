using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    [SerializeField]
    private bool testMode;

    [Space]
    [SerializeField]
    private InGameProgress inGameProgress;
    [SerializeField]
    private InGameProgress newGameProgress;
    [SerializeField]
    private GameObject playerUtils;

    [Header("Fading")]
    [SerializeField]
    private Image fadingPanel;

    [SerializeField]
    private float fadingSpeed = 1.5f;
    private bool isFading = false;

    private string unselectedScene;

    private bool loading;
    private bool paused;
    private bool inMinigame;

    protected override void LoadData()
    {
        if (testMode)
        {
            TestLoad();
        }
        else
        {
            LoadProgress(false);
        }
    }

    private void TestLoad()
    {
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        dualCharacterController.SetMobility(false);
        SetInScenePosition(-1, false);
        LoadSceneElements();
        dualCharacterController.SetMobility(true);
        DisableFadePanel();
    }

    public void LoadProgress(bool newGame)
    {
        if (!newGame)
        {
            inGameProgress.Clear();
            SavedProgress savedProgress = PersistenceUtils.Load();
            if (savedProgress != null)
            {
                inGameProgress.Load(savedProgress);
            }
        }
        else
        {
            inGameProgress.Copy(newGameProgress);
        }   
    }

    public void LoadNewGame()
    {
        PersistenceUtils.ClearSave();
        LoadProgress(true);
        LoadSceneFromMenu(inGameProgress.player.selectedCharacter.scene, false);
    }

    public void LoadFromPause()
    {
        LoadProgress(false);
        LoadSceneFromMenu(inGameProgress.player.selectedCharacter.scene, false);
    }

    public void LoadScene(string destinationScene, int destinationPassage = -1, bool reverseLookingAt = false)
    {
        StartCoroutine(LoadSceneCoroutine(destinationScene, destinationPassage, reverseLookingAt));
    }

    public void LoadSceneFromMenu(string destinationScene, bool init = true)
    {
        StartCoroutine(LoadSceneFromMenuCoroutine(destinationScene, init));
    }

    public void LoadMinigameScene(string minigameScene)
    {
        StartCoroutine(LoadMinigameSceneCoroutine(minigameScene));
    }

    public void ReturnFromMinigameScene()
    {
        StartCoroutine(ReturnFromMinigameSceneCoroutine());
    }

    public IEnumerator LoadSceneCoroutine(string destinationScene, int destinationPassage, bool reverseLookingAt)
    {
        loading = true;

        DisableControl();

        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        if (!dualCharacterController.Grouped && unselectedScene == null)
        {
            unselectedScene = SceneManager.GetActiveScene().name;
        }

        yield return StartCoroutine(Fade(true));

        SaveSceneProgress();

        yield return StartCoroutine(LoadDestinationScene(destinationScene));

        LoadSceneElements();

        SetInScenePosition(destinationPassage, reverseLookingAt);

        // Disable/enable unselected character in scene
        if (dualCharacterController.IsCharacterActive(false))
        {
            if (!dualCharacterController.Grouped && !destinationScene.Equals(unselectedScene))
            {
                dualCharacterController.SetCharacterActive(false, false);
            }
        }
        else
        {
            if (!dualCharacterController.Grouped && destinationScene.Equals(unselectedScene))
            {
                dualCharacterController.SetCharacterActive(false, true);
            }
        }

        // Enable unselected character movement
        if (dualCharacterController.IsCharacterActive(false))
        {
            dualCharacterController.SetCharacterMobility(false, true);
        }
        
        dualCharacterController.SetCharacterMobility(true, true);

        yield return StartCoroutine(Fade(false));

        EnableControl();

        loading = false;
    }

    public IEnumerator LoadSceneSwitchCoroutine()
    {
        loading = true;

        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        dualCharacterController.SetSwitchAvailability(false);
        
        DisableControl();

        string newFollowerScene = SceneManager.GetActiveScene().name;

        yield return StartCoroutine(Fade(true));

        SaveSceneProgress();

        yield return StartCoroutine(LoadDestinationScene(unselectedScene));

        LoadSceneElements();

        unselectedScene = newFollowerScene;

        // Change character which is active
        dualCharacterController.SetCharacterActive(true, false);
        dualCharacterController.SetCharacterActive(false, true);

        dualCharacterController.SetCharacterMobility(true, true);

        // Switch character without camera transition
        dualCharacterController.SetCameraTransitionTime(true);
        dualCharacterController.SwitchCharacter();

        yield return StartCoroutine(Fade(false));

        EnableControl();

        dualCharacterController.SetCameraTransitionTime(false);
        dualCharacterController.SetSwitchAvailability(true);

        loading = false;
    }

    public IEnumerator LoadSceneFromMenuCoroutine(string destinationScene, bool init)
    {
        loading = true;

        yield return StartCoroutine(Fade(true));

        yield return StartCoroutine(LoadDestinationScene(destinationScene));

        DisableControl();

        LoadSceneElements();

        if (init)
        {
            SetInScenePosition(-1, false);
            PlayerManager.Instance.GetDualCharacterController().SetMobility(true);
        }
        else
        {
            LoadPlayer();
        }

        yield return StartCoroutine(Fade(false));

        PlayerManager.Instance.GetDualCharacterController().SetCameraTransitionTime(false);

        EnableControl();

        loading = false;
    }

    public IEnumerator LoadMinigameSceneCoroutine(string minigameScene)
    {
        loading = true;
        inMinigame = true;

        yield return StartCoroutine(Fade(true));

        SceneManager.LoadScene(minigameScene, LoadSceneMode.Additive);

        PlayerManager.Instance.GetDualCharacterController().SwitchToMinigameCamera();

        yield return StartCoroutine(Fade(false));

        loading = false;
    }

    public IEnumerator ReturnFromMinigameSceneCoroutine()
    {
        loading = true;

        yield return StartCoroutine(Fade(true));

        yield return SceneManager.UnloadSceneAsync(2);

        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        dualCharacterController.SetCharacterMobility(true, true);
        dualCharacterController.SetSwitchAvailability(true);
        dualCharacterController.SwitchToCharacterCamera();

        PlayerManager.Instance.GetInteractionController().SetInteractivity(true);

        yield return StartCoroutine(Fade(false));

        inMinigame = false;
        loading = false;
    }

    private void DisableControl()
    {
        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();
        interactionController.DestroyInteractions();
        interactionController.SetInteractivity(false);

        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        dualCharacterController.SetMobility(false);
        dualCharacterController.SetSwitchAvailability(false);
    }

    private void EnableControl()
    {
        PlayerManager.Instance.GetInteractionController().SetInteractivity(true);
        PlayerManager.Instance.GetDualCharacterController().SetSwitchAvailability(true);
    }

    public void SaveSceneProgress()
    {
        GameObject rootDynamicObjects = GameObject.Find(GlobalConstants.PathDynamicObjectsRoot);
        Dictionary<string, ObjectState> objectStates = PersistenceUtils.SaveObjects(rootDynamicObjects);
        string sceneName = SceneManager.GetActiveScene().name;
        if (inGameProgress.scenes.ContainsKey(sceneName))
        {
            inGameProgress.scenes[sceneName] = objectStates;
        }
        else
        {
            inGameProgress.scenes.Add(sceneName, objectStates);
        }
    }

    private void LoadPlayer()
    {
        PlayerData playerData = inGameProgress.player;
        LoadItems(playerData.itemsOne, playerData.itemsTwo);
        LoadDocuments(playerData.documents);

        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();

        dualCharacterController.SetCharacterActive(true, true);
        dualCharacterController.SetCharacterActive(false, true);

        if (playerData.selectedCharacterOne != dualCharacterController.SelectedCharacterOne)
        {
            dualCharacterController.SetCameraTransitionTime(true);
            dualCharacterController.SwitchCharacter();
        }
        if (playerData.grouped != dualCharacterController.Grouped)
        {
            dualCharacterController.SwitchGrouping();
        }

        dualCharacterController.GetCharacter(true).transform.position = playerData.selectedCharacter.Position;
        dualCharacterController.SetCharacterLookingAt(true, playerData.selectedCharacter.LookingAt);

        dualCharacterController.GetCharacter(false).transform.position = playerData.unselectedCharacter.Position;
        dualCharacterController.SetCharacterLookingAt(false, playerData.unselectedCharacter.LookingAt);

        dualCharacterController.SetCharacterMobility(true, true);
        dualCharacterController.SetCharacterMobility(false, true);

        if (!playerData.grouped && !playerData.selectedCharacter.scene.Equals(playerData.unselectedCharacter.scene))
        {
            unselectedScene = playerData.unselectedCharacter.scene;
            dualCharacterController.SetCharacterActive(false, false);
        }
        else
        {
            dualCharacterController.SetCharacterMobility(false, true);
        }
    }

    private void LoadItems(List<string> itemsOne, List<string> itemsTwo)
    {
        InventoryController inventoryController = PlayerManager.Instance.GetInventoryController();
        inventoryController.Clear();

        foreach (string itemId in itemsOne)
        {
            inventoryController.AddItem(true, ItemDataManager.Instance.Get(itemId));
        }

        foreach (string itemId in itemsTwo)
        {
            inventoryController.AddItem(false, ItemDataManager.Instance.Get(itemId));
        }
    }

    private void LoadDocuments(List<string> documents)
    {
        DocumentationController documentationController = PlayerManager.Instance.GetDocumentationController();
        documentationController.Clear();
        foreach (string documentId in documents)
        {
            documentationController.Add(ItemDataManager.Instance.Get(documentId), false);
        }
    }

    private IEnumerator LoadDestinationScene(string destinationScene)
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(destinationScene, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
    }

    private void LoadSceneElements()
    {
        PolygonCollider2D boundaries = GameObject.Find(GlobalConstants.PathBoundaries).GetComponent<PolygonCollider2D>();
        GameObject[] vCams = GameObject.FindGameObjectsWithTag(GlobalConstants.TagVirtualCamera);
        foreach (GameObject vCam in vCams)
        {
            vCam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = boundaries;
        }

        GameObject rootGameObject = GameObject.Find(GlobalConstants.PathDynamicObjectsRoot);
        Dictionary<string, ObjectState> objectStates = new();
        if (inGameProgress.scenes.ContainsKey(SceneManager.GetActiveScene().name))
        {
            objectStates = inGameProgress.scenes[SceneManager.GetActiveScene().name];
        }
        PersistenceUtils.LoadObjects(rootGameObject, objectStates);
    }

    private void SetInScenePosition(int destinationPassage, bool reverseLookingAt)
    {
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();

        Transform inSceneTransform = GetInSceneTransform(destinationPassage);
        Tuple<bool, bool> inSceneLookingAt = GetInSceneLookingAt(inSceneTransform, reverseLookingAt);

        dualCharacterController.GetCharacter(true).transform.position = inSceneTransform.position;
        dualCharacterController.SetCharacterLookingAt(true, inSceneLookingAt);

        if (dualCharacterController.Grouped)
        {
            dualCharacterController.GetCharacter(false).transform.position = GetUnselectedCharacterPosition(inSceneTransform.position, inSceneLookingAt);
            dualCharacterController.SetCharacterLookingAt(false, inSceneLookingAt);
        }
    }

    private Transform GetInSceneTransform(int destinationPassage)
    {
        Transform inSceneTransform;
        if (destinationPassage == -1)
        {
            inSceneTransform = GameObject.FindGameObjectWithTag(GlobalConstants.TagPlayerInit).transform;
        }
        else
        {
            GameObject[] passages = GameObject.FindGameObjectsWithTag(GlobalConstants.TagPassage);
            inSceneTransform = passages[destinationPassage].transform;
        }
        return inSceneTransform;
    }

    private Tuple<bool, bool> GetInSceneLookingAt(Transform inSceneTransform, bool reverseLookingAt)
    {
        bool verticalMovement = true;
        int angle = (int)inSceneTransform.eulerAngles.z;
        bool positiveMovement = angle == 270;

        if (angle == 0 || angle == 180)
        {
            verticalMovement = false;
            positiveMovement = angle == 180;
        }

        if (reverseLookingAt)
        {
            positiveMovement = !positiveMovement;
        }

        return Tuple.Create(verticalMovement, positiveMovement);
    }

    private Vector3 GetUnselectedCharacterPosition(Vector3 selectedCharacterPosition, Tuple<bool, bool> inSceneLookingAt)
    {
        Vector3 position = selectedCharacterPosition;
        int direction = inSceneLookingAt.Item2 ? -1 : 1;
        float distance = PlayerManager.Instance.GetDualCharacterController().Params.DistanceOnLoadScene;

        if (inSceneLookingAt.Item1)
        {
            position += distance * direction * Vector3.up;
        }
        else
        {
            position += distance * direction * Vector3.right;
        }

        return position;
    }

    public IEnumerator Fade(bool fadeToBlack)
    {
        if (!isFading)
        {
            isFading = true;

            if (fadeToBlack)
            {
                while (fadingPanel.color.a < 1)
                {
                    yield return StartCoroutine(Fade(fadingPanel.color, fadingSpeed));
                }
            }
            else
            {
                while (fadingPanel.color.a > 0)
                {
                    yield return StartCoroutine(Fade(fadingPanel.color, -fadingSpeed));
                }
            }

            isFading = false;
        } 
    }

    private IEnumerator Fade(Color objectColor, float fadeSpeed)
    {
        float fadeAmount;

        fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

        objectColor = new(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
        fadingPanel.color = objectColor;
        yield return null;
    }

    private void DisableFadePanel()
    {
        Color objectColor = fadingPanel.color;
        objectColor = new(objectColor.r, objectColor.g, objectColor.b, 0);
        fadingPanel.color = objectColor;
    }

    public void ResetFollowerInSceneData()
    {
        unselectedScene = null;
    }

    public void Pause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        paused = pause;
    }

    public InGameProgress Progress { get => inGameProgress; }
    public GameObject PlayerUtils { get => playerUtils; }
    public string UnselectedScene { get => unselectedScene; set => unselectedScene = value; }
    public bool Paused { get => paused; }
    public bool Loading { get => loading; }
    public bool InMinigame { get => inMinigame; set => inMinigame = value; }
    public bool LoadSceneOnSwitch { get => unselectedScene != null && !SceneManager.GetActiveScene().name.Equals(unselectedScene); }
}
