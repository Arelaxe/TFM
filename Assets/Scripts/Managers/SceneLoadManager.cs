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
    private GameObject playerUtils;

    [Header("Fading")]
    [SerializeField]
    private Image fadingPanel;

    [SerializeField]
    private float fadingSpeed = 1.5f;
    private bool isFading = false;

    private string unselectedScene;

    protected override void LoadData()
    {
        if (testMode)
        {
            TestLoad();
        }
        else
        {
            LoadProgress();
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

    private void LoadProgress()
    {
        SavedProgress savedProgress = PersistenceUtils.Load();
        if (savedProgress != null)
        {
            inGameProgress.Load(savedProgress);
        }
    }

    public void LoadScene(string destinationScene, int destinationPassage = -1, bool reverseLookingAt = false)
    {
        StartCoroutine(LoadSceneCouroutine(destinationScene, destinationPassage, reverseLookingAt));
    }

    public void LoadSceneFromMenu(string destinationScene, bool init = true)
    {
        StartCoroutine(LoadSceneFromMenuCouroutine(destinationScene, init));
    }

    public IEnumerator LoadSceneCouroutine(string destinationScene, int destinationPassage, bool reverseLookingAt)
    {
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
    }

    public IEnumerator LoadSceneSwitchCouroutine()
    {
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
    }

    public IEnumerator LoadSceneFromMenuCouroutine(string destinationScene, bool init)
    {
        yield return StartCoroutine(Fade(true));

        yield return StartCoroutine(LoadDestinationScene(destinationScene));

        LoadSceneElements();

        DisableControl();

        if (init)
        {
            SetInScenePosition(-1, false);
            PlayerManager.Instance.GetDualCharacterController().SetMobility(true);
        }
        else
        {
            LoadPlayerData();
        }

        yield return StartCoroutine(Fade(false));

        EnableControl();
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

    private void SaveSceneProgress()
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

    private void LoadPlayerData()
    {
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        if (!inGameProgress.player.selectedCharacterOne)
        {
            dualCharacterController.SwitchCharacter();
        }
        if (!inGameProgress.player.grouped)
        {
            dualCharacterController.SwitchGrouping();
        }

        CharacterData selectedCharacter = inGameProgress.player.selectedCharacter;
        CharacterData unselectedCharacter = inGameProgress.player.unselectedCharacter;

        dualCharacterController.GetCharacter(true).transform.position = selectedCharacter.Position;
        dualCharacterController.SetCharacterLookingAt(true, selectedCharacter.LookingAt);

        dualCharacterController.GetCharacter(false).transform.position = unselectedCharacter.Position;
        dualCharacterController.SetCharacterLookingAt(false, unselectedCharacter.LookingAt);

        dualCharacterController.SetCharacterMobility(true, true);
        if (!inGameProgress.player.grouped && !selectedCharacter.scene.Equals(unselectedCharacter.scene))
        {
            unselectedScene = unselectedCharacter.scene;
            dualCharacterController.SetCharacterActive(false, false);
        }
        else
        {
            dualCharacterController.SetCharacterMobility(false, true);
        }
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

    public InGameProgress Progress { get => inGameProgress; }
    public GameObject PlayerUtils { get => playerUtils; }
    public string UnselectedScene { get => unselectedScene; }
    public bool LoadSceneOnSwitch { get => unselectedScene != null && !SceneManager.GetActiveScene().name.Equals(unselectedScene); }
}
