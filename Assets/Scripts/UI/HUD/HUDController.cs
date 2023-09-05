using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private GameObject hud;

    [Header("Character Icons")]
    [SerializeField]
    private Image character1Icon;
    [SerializeField]
    private Image character2Icon;
    [Header("Group Icons")]
    [SerializeField]
    private Image groupChain;
    [SerializeField]
    private Sprite chainSprite;
    [SerializeField]
    private Sprite brokenChainSprite;
    [Header("Buttons")]
    [SerializeField]
    private Button inventoryButton;
    [SerializeField]
    private Button documentationButton;

    public void ShowHUD()
    {
        hud.SetActive(true);
    }

    public void HideHUD()
    {
        hud.SetActive(false);
    }

    public void EnableHUD()
    {
        inventoryButton.interactable = true;
        documentationButton.interactable = true;
    }

    public void DisableHUD()
    {
        inventoryButton.interactable = false;
        documentationButton.interactable = false;
    }

    public void SwitchCharacterIcon()
    {
        Color newColor = character1Icon.color;

        if (PlayerManager.Instance.SelectedCharacterOne)
        {
            newColor.a = 1;
            character1Icon.color = newColor;
            newColor.a = 0.5f;
            character2Icon.color = newColor;
        }
        else
        {
            newColor.a = 1;
            character2Icon.color = newColor;
            newColor.a = 0.5f;
            character1Icon.color = newColor;
        }
    }

    public void SwitchGroupIcon()
    {
        if (PlayerManager.Instance.Grouped)
        {
            groupChain.sprite = chainSprite;
        }
        else
        {
            groupChain.sprite = brokenChainSprite;
        }
    }
}
