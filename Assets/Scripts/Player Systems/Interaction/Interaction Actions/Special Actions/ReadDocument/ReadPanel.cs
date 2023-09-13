using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI documentContent;

    [SerializeField]
    private Image documentImage;

    [SerializeField]
    private Button closeBtn;

    private void Start()
    {
        closeBtn.Select();
    }

    public void Init(Item document)
    {
        documentContent.text = document.Description;
        if (document.ItemImage)
        {
            documentImage.sprite = document.ItemImage;
        }
    }

    public void Close()
    {
        PlayerManager.Instance.GetInGameMenuController().DestroyAdditionalUI();
    }
}
