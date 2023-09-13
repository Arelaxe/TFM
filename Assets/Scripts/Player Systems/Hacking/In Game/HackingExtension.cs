using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class HackingExtension : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI documentTitle;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private GameObject tutorialBox;

    private GameObject interactable;
    private string scene;
    private Item item;
    private Item document;

    private void OnEnable()
    {
        string tutorialHacking = SceneLoadManager.Instance.GetKeyAction(KeyActions.TutorialHacking);
        tutorialBox.GetComponent<CanvasGroup>().alpha = tutorialHacking == null ? 1 : 0;
    }

    public void SetItem(Item item)
    {
        this.item = item;
        itemImage.enabled = true;
        itemImage.sprite = item.ItemImage;
    }

    public void SetDocument(Item document)
    {
        this.document = document;

        documentTitle.text = document.Name;
    }

    public void ResetData(string scene, GameObject interactable)
    {
        this.interactable = interactable;
        this.scene = scene;
        item = null;
        document = null;

        itemImage.sprite = null;
        itemImage.enabled = false;
        documentTitle.text = "";
    }

    public void SelectContinueButton()
    {
        continueButton.Select();
    }

    public void OpenMiniGame()
    {
        Dictionary<string, Object> hackingData = new();
        hackingData.Add(HackingManager.HackingInteractable, interactable);
        hackingData.Add(HackingManager.HackingItem, item);
        hackingData.Add(HackingManager.HackingDocument, document);

        PlayerManager.Instance.GetInGameMenuController().Hide();

        SceneLoadManager.Instance.LoadAdditiveScene(scene, hackingData);
    }
}
