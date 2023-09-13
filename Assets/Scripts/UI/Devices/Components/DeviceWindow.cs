using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeviceWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [Header("Document Info")]
    [SerializeField]
    private Image docImage;

    [SerializeField]
    private TextMeshProUGUI docText;

    private DeviceWindowIcon previous;

    public void Open(string title, Item document, DeviceWindowIcon previous)
    {
        this.previous = previous;
        this.title.text = title;
        if (document)
        {
            if (docImage)
            {
                docImage.sprite = document.ItemImage;
            }
            if (docText)
            {
                docText.text = document.Description;
            }
        }
        gameObject.SetActive(true);
    }

    public void Close()
    {
        previous.Select();
        gameObject.SetActive(false);
    }
}
