using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescription : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TMP_Text itemName;

    [SerializeField]
    private TMP_Text itemDescription;

    public void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        itemImage.gameObject.SetActive(false);
        itemName.text = "";
        itemDescription.text = "";
    }

    public void SetDescription(Sprite sprite, string itemName, string itemDescription)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        this.itemName.text = itemName;
        this.itemDescription.text = itemDescription;
    }
}
