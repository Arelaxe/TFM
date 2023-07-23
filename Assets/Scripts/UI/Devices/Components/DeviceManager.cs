using System.Collections;
using UnityEngine;
using TMPro;

public class DeviceManager : MonoBehaviour
{
    [Header("User Interface")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject restrictionPanel;
    private bool restrictionPanelActivated;

    private void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = GlobalConstants.SortingLayerForeground;
    }

    public void ShowRestrictionPanel(int accessLevelRequired)
    {
        restrictionPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Acceso restringido:\nNivel " + accessLevelRequired + " de acceso requerido";
        restrictionPanel.GetComponent<CanvasGroup>().alpha = 1; ;
        
        StartCoroutine(FadeRestrictionPanel());
    }

    private IEnumerator FadeRestrictionPanel()
    {
        restrictionPanelActivated = true;
        yield return new WaitForSeconds(1.5f);
        restrictionPanelActivated = false;

        CanvasGroup canvasGroup = restrictionPanel.GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.05f;
            yield return new WaitForSeconds(0.1f);
            if (restrictionPanelActivated) break;
        }
    }
}
