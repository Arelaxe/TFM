using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;

public class HackingManager : MonoBehaviour
{
    private PlayerInput input;
    private InputAction startAction;

    [Header("Energy Flow")]
    [SerializeField]
    private GameObject energyFlowPrefab;
    [SerializeField]
    private Transform playerStart;

    private GameObject energyFlow;

    [Header("Energy Points")]
    [SerializeField]
    private GameObject energyPointPrefab;
    [SerializeField]
    private GameObject energyPointsParent;

    private List<GameObject> energyPoints = new();

    [Header("User Interface")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject status;
    [SerializeField]
    private GameObject controls;
    [SerializeField]
    private EnergyContainer energyContainer;
    [SerializeField]
    private Color32 failedColor;
    [SerializeField]
    private Color32 successColor;
    [SerializeField]
    private Color32 maximumColor;

    [Header("Camera")]
    [SerializeField]
    private PolygonCollider2D bounds;

    private bool firstRun;

    private void Start()
    {
        InitUI();
        InitInputActions();
        InitPlayer();
        InitPoints();
    }

    private void Update()
    {
        if (startAction.triggered && CanRun())
        {
            Run();
        }
    }

    private void InitUI()
    {
        canvas.worldCamera = Camera.main;
        StartCoroutine(FadeCanvasGroup(status.GetComponent<CanvasGroup>(), false));
    }

    private void InitInputActions()
    {
        input = GetComponent<PlayerInput>();
        startAction = input.actions[PlayerConstants.ActionInteract];
    }

    private void InitPlayer()
    {
        energyFlow = Instantiate(energyFlowPrefab, playerStart.position, Quaternion.identity, transform);

        CinemachineVirtualCamera vcam = PlayerManager.Instance.GetDualCharacterController().GetMinigameCamera();
        vcam.Follow = energyFlow.transform;
        vcam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = bounds;
    }

    private void InitPoints()
    {
        for (int i = 0; i < energyPointsParent.transform.childCount; i++)
        {
            GameObject energyPoint = Instantiate(energyPointPrefab, energyPointsParent.transform.GetChild(i).transform.position, Quaternion.identity, transform);
            energyPoint.name = energyPoint.name.Replace("(Clone)", "") + "_" + i;
            energyPoints.Add(energyPoint);
        }
    }

    private void Run()
    {
        energyFlow.GetComponent<EnergyFlowController>().CanMove = true;

        if (!firstRun)
        {
            StartCoroutine(FadeCanvasGroup(status.GetComponent<CanvasGroup>()));
            firstRun = true;
        }

        StartCoroutine(FadeCanvasGroup(controls.GetComponent<CanvasGroup>()));
    }

    private bool CanRun()
    {
        bool canRun = !energyFlow.GetComponent<EnergyFlowController>().CanMove;
        if (!firstRun)
        {
            canRun = canRun && status.GetComponent<CanvasGroup>().alpha == 1;
        }
        else
        {
            canRun = canRun && controls.GetComponent<CanvasGroup>().alpha == 1;
        }
        return canRun;
    }

    public void Fail()
    {
        Destroy(energyFlow);
        StartCoroutine(End());
    }

    public IEnumerator End()
    {
        if (energyFlow)
        {
            EnergyFlowController flow = energyFlow.GetComponent<EnergyFlowController>();
            flow.Stop();

            yield return StartCoroutine(energyContainer.AddEnergy(flow.Energy));
        }

        switch (energyContainer.GetEnergyLevel())
        {
            case 1:
                status.GetComponentInChildren<TextMeshProUGUI>().text = "Hackeo fallido";
                status.GetComponentInChildren<Image>().color = failedColor;
                break;
            case 2:
                status.GetComponentInChildren<TextMeshProUGUI>().text = "Hackeo completado";
                status.GetComponentInChildren<Image>().color = successColor;
                break;
            case 3:
                status.GetComponentInChildren<TextMeshProUGUI>().text = "Hackeo máximo";
                status.GetComponentInChildren<Image>().color = maximumColor;
                break;
            default:
                break;
        }

        yield return StartCoroutine(FadeCanvasGroup(status.GetComponent<CanvasGroup>(), false));

        yield return new WaitForSeconds(1.5f);
        SceneLoadManager.Instance.ReturnFromMinigameScene();
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup group, bool hide = true)
    {
        if (hide)
        {
            while (group.alpha > 0)
            {
                group.alpha -= 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            while (group.alpha < 1)
            {
                group.alpha += 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        
    }
}
