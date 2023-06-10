using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    [SerializeField]
    private bool testMode;

    [Space]
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
    }

    private void TestLoad()
    {
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();
        dualCharacterController.SetMobility(false);
        SetInScenePosition(dualCharacterController, -1, false);
        dualCharacterController.SetMobility(true);
        DisableFadePanel();
    }

    public void LoadScene(string destinationScene, int destinationPassage = -1, bool reverseLookingAt = false)
    {
        StartCoroutine(LoadSceneCouroutine(destinationScene, destinationPassage, reverseLookingAt));
    }

    public void LoadSceneFromMenu(string destinationScene)
    {
        StartCoroutine(LoadSceneFromMenuCouroutine(destinationScene));
    }

    public IEnumerator TryLoadSceneOnSwitch()
    {
        if (unselectedScene != null)
        {
            yield return StartCoroutine(LoadSceneSwitchCouroutine());
        }
    }

    public IEnumerator LoadSceneCouroutine(string destinationScene, int destinationPassage, bool reverseLookingAt)
    {
        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();

        PreCharacterControl(interactionController, dualCharacterController);

        if (!dualCharacterController.Grouped && !destinationScene.Equals(unselectedScene))
        {
            unselectedScene = SceneManager.GetActiveScene().name;
        }

        yield return StartCoroutine(Fade(true));

        yield return StartCoroutine(LoadDestinationScene(destinationScene));

        SetBoundaries();

        SetInScenePosition(dualCharacterController, destinationPassage, reverseLookingAt);

        if (dualCharacterController.Grouped || destinationScene.Equals(unselectedScene))
        {
            SetCharacterActive(dualCharacterController.GetUnselectedCharacter(), true);
            dualCharacterController.SetUnselectedCharacterMobility(true);

            if (destinationScene.Equals(unselectedScene))
            {
                ResetFollowerInSceneData();
            }
        }
        else
        {
            SetCharacterActive(dualCharacterController.GetUnselectedCharacter(), false);
        }
        
        dualCharacterController.SetSelectedCharacterMobility(true);

        yield return StartCoroutine(Fade(false));

        PostCharacterControl(interactionController, dualCharacterController);
    }

    public IEnumerator LoadSceneSwitchCouroutine()
    {
        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();

        PreCharacterControl(interactionController, dualCharacterController);

        string newFollowerScene = SceneManager.GetActiveScene().name;

        yield return StartCoroutine(Fade(true));

        yield return StartCoroutine(LoadDestinationScene(unselectedScene));

        SetBoundaries();

        unselectedScene = newFollowerScene;

        SetCharacterActive(dualCharacterController.GetSelectedCharacter(), false);
        SetCharacterActive(dualCharacterController.GetUnselectedCharacter(), true);

        dualCharacterController.SetSelectedCharacterMobility(true);
        dualCharacterController.SetCameraTransitionTime(true);

        PostCharacterControl(interactionController, dualCharacterController);
    }

    public IEnumerator LoadSceneFromMenuCouroutine(string destinationScene)
    {
        yield return StartCoroutine(Fade(true));

        yield return StartCoroutine(LoadDestinationScene(destinationScene));

        InteractionController interactionController = PlayerManager.Instance.GetInteractionController();
        DualCharacterController dualCharacterController = PlayerManager.Instance.GetDualCharacterController();

        PreCharacterControl(interactionController, dualCharacterController);

        SetBoundaries();

        SetInScenePosition(dualCharacterController, -1, false);

        dualCharacterController.SetMobility(true);

        yield return StartCoroutine(Fade(false));

        PostCharacterControl(interactionController, dualCharacterController);
    }

    private void PreCharacterControl(InteractionController interactionController, DualCharacterController dualCharacterController)
    {
        interactionController.DestroyInteractions();
        interactionController.SetInteractivity(false);
        dualCharacterController.SetMobility(false);
        dualCharacterController.SetSwitchAvailability(false);
    }

    private void PostCharacterControl(InteractionController interactionController, DualCharacterController dualCharacterController)
    {
        interactionController.SetInteractivity(true);
        dualCharacterController.SetSwitchAvailability(true);
    }

    private IEnumerator LoadDestinationScene(string destinationScene)
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(destinationScene, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
    }

    private void SetBoundaries()
    {
        PolygonCollider2D boundaries = GameObject.Find(GlobalConstants.PathBoundaries).GetComponent<PolygonCollider2D>();
        GameObject[] vCams = GameObject.FindGameObjectsWithTag(GlobalConstants.TagVirtualCamera);
        foreach (GameObject vCam in vCams)
        {
            vCam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = boundaries;
        }
    }

    private void SetInScenePosition(DualCharacterController dualCharacterController, int destinationPassage, bool reverseLookingAt)
    {
        Transform inSceneTransform = GetInSceneTransform(destinationPassage);
        Tuple<bool, bool> inSceneLookingAt = GetInSceneLookingAt(inSceneTransform, reverseLookingAt);

        dualCharacterController.GetSelectedCharacter().transform.position = inSceneTransform.position;
        dualCharacterController.SetSelectedCharacterLookingAt(inSceneLookingAt);

        if (dualCharacterController.Grouped)
        {
            dualCharacterController.GetUnselectedCharacter().transform.position = GetUnselectedCharacterPosition(inSceneTransform.position, inSceneLookingAt);
            dualCharacterController.SetUnselectedCharacterLookingAt(inSceneLookingAt);
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

    private void SetCharacterActive(GameObject gameObject, bool active)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = active;
        gameObject.GetComponent<Collider2D>().enabled = active;
    }

    public void ResetFollowerInSceneData()
    {
        unselectedScene = null;
    }

    public GameObject PlayerUtils { get => playerUtils; }
}
