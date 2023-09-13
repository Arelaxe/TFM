using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescription : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TMP_Text title;

    [SerializeField]
    private TMP_Text description;

    public void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        image.gameObject.SetActive(false);
        title.text = "";
        description.text = "";
    }

    public void SetDescription(Sprite sprite, string title, string description)
    {
        image.gameObject.SetActive(true);
        image.sprite = sprite;
        this.title.text = title;
        this.description.text = description;
    }
}
