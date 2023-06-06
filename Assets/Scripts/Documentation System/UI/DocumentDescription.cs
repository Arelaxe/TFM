using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DocumentDescription : MonoBehaviour
{
    [SerializeField]
    private Image documentImage;

    [SerializeField]
    private TMP_Text documentDescription;

    [SerializeField]
    private Scrollbar scrollbar;

    public void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        documentImage.gameObject.SetActive(false);
        documentDescription.text = "";
        
        scrollbar.value = 1;
    }

    public void SetDescription(Sprite sprite, string documentDescription)
    {
        documentImage.gameObject.SetActive(true);
        documentImage.sprite = sprite;
        this.documentDescription.text = documentDescription;
        
        scrollbar.value = 1;
        scrollbar.Select();
    }

    public void SetNavigation(DocumentItem documentItem)
    {
        Navigation navToSelected = new();
        navToSelected.mode = Navigation.Mode.Explicit;
        navToSelected.selectOnLeft = documentItem;
        scrollbar.navigation = navToSelected;
    }
}
