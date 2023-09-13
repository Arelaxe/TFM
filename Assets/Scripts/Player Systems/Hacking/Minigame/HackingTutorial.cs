using UnityEngine;
using UnityEngine.UI;

public class HackingTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject[] panels;

    [SerializeField]
    private Button previousButton;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Color32 nextColor;

    [SerializeField]
    private Color32 endTutorialColor;

    private int currentPanelIndex = 0;

    private void OnEnable()
    {
        nextButton.Select();
    }

    public void NextPanel()
    {
        if (currentPanelIndex != panels.Length - 1)
        {
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            panels[currentPanelIndex].SetActive(true);

            previousButton.interactable = true;

            if (currentPanelIndex == panels.Length - 1)
            {
                nextButton.GetComponent<Image>().color = endTutorialColor;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void PreviousPanel()
    {
        if (currentPanelIndex != 0)
        {
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex--;
            panels[currentPanelIndex].SetActive(true);

            nextButton.GetComponent<Image>().color = nextColor;

            if (currentPanelIndex == 0)
            {
                previousButton.interactable = false;
                nextButton.Select();
            }
        }
    }
}
