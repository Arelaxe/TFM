using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DocumentDescription : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TMP_Text title;

    [SerializeField]
    private TMP_Text description;

    [SerializeField]
    private Scrollbar scrollbar;

    public void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        image.gameObject.SetActive(false);
        title.text = "";
        description.text = "";
        
        scrollbar.value = 1;
    }

    public void SetDescription(Sprite sprite, string title, string description)
    {
        image.gameObject.SetActive(true);
        image.sprite = sprite;
        this.title.text = title;
        this.description.text = description;
        
        scrollbar.value = 1;
        scrollbar.Select();
    }

    public void SetNavigation(DocumentElement element)
    {
        Navigation navToSelected = new();
        navToSelected.mode = Navigation.Mode.Explicit;
        navToSelected.selectOnLeft = element;
        scrollbar.navigation = navToSelected;
    }
}
