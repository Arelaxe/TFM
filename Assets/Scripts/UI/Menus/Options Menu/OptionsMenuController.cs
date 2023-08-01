using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField]
    private Color32 normalColor;
    [SerializeField]
    private Color32 selectedColor;

    private GameObject currentContent;
    private Button currentButton;

    private void OnDisable()
    {
        TryDisableCurrentContent();
        TryDisableCurrentButton();
    }

    public void ShowContent(GameObject content)
    {
        TryDisableCurrentContent();

        content.SetActive(true);
        currentContent = content;
    }

    public void SetSelectColor(Button button)
    {
        TryDisableCurrentButton();

        ColorBlock cb = button.colors;
        cb.normalColor = selectedColor;
        button.colors = cb;
        currentButton = button;
    }

    private void TryDisableCurrentContent()
    {
        if (currentContent)
        {
            currentContent.SetActive(false);
        }
    }

    private void TryDisableCurrentButton()
    {
        if (currentButton)
        {
            ColorBlock currentBlock = currentButton.colors;
            currentBlock.normalColor = normalColor;
            currentButton.colors = currentBlock;
        }
    }
}
