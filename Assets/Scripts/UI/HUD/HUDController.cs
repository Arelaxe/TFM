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

    public void EnableHUD()
    {
        hud.SetActive(true);
    }

    public void DisableHUD()
    {
        hud.SetActive(false);
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
