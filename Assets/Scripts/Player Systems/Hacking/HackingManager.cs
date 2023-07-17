using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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
    private CinemachineVirtualCamera vcam;
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
    
    // Status
    private bool firstRun;
    private bool failed;

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
        StartCoroutine(FadeCanvasGroup(status.GetComponent<CanvasGroup>(), false));
    }

    private void InitInputActions()
    {
        input = GetComponent<PlayerInput>();
        startAction = input.actions[PlayerConstants.ActionInteract];
    }

    private void InitPlayer()
    {
        energyFlow = Instantiate(energyFlowPrefab, playerStart.position, Quaternion.identity);
        vcam.Follow = energyFlow.transform;
    }

    private void InitPoints()
    {
        for (int i = 0; i < energyPointsParent.transform.childCount; i++)
        {
            GameObject energyPoint = Instantiate(energyPointPrefab, energyPointsParent.transform.GetChild(i).transform.position, Quaternion.identity);
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

    public void Restart()
    {
        energyFlow.transform.position = playerStart.position;
        foreach (GameObject point in energyPoints)
        {
            Destroy(point);
        }
        energyPoints.Clear();
        InitPoints();

        StartCoroutine(FadeCanvasGroup(controls.GetComponent<CanvasGroup>(), false));
    }

    public IEnumerator End()
    {
        EnergyFlowController flow = energyFlow.GetComponent<EnergyFlowController>();
        flow.Stop();

        yield return StartCoroutine(energyContainer.AddEnergy(flow.Energy));

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

        StartCoroutine(FadeCanvasGroup(status.GetComponent<CanvasGroup>(), false));
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
