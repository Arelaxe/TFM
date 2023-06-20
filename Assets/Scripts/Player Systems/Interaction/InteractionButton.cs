using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionButton : MonoBehaviour
{
    [SerializeField]
    private GameObject typeConstraint;
    [SerializeField]
    private GameObject itemConstraint;

    [Space]
    [SerializeField]
    private Sprite hackingIcon;
    [SerializeField]
    private Sprite spiritualIcon;

    public void SetData(Interaction interaction)
    {
        Button tempButton = GetComponent<Button>();
        tempButton.GetComponentInChildren<TextMeshProUGUI>().text = interaction.Name;

        if (!interaction.Type.Equals(Interaction.ActionType.Normal))
        {
            typeConstraint.SetActive(true);
            Image typeImage = typeConstraint.GetComponent<Image>();
            if (interaction.Type.Equals(Interaction.ActionType.Hacking))
            {
                typeImage.sprite = hackingIcon;
            }
            else if (interaction.Type.Equals(Interaction.ActionType.Spiritual))
            {
                typeImage.sprite = spiritualIcon;
            }
        }
        
        if (interaction.RequiredItem != null)
        {
            itemConstraint.SetActive(true);
            Image itemImage = itemConstraint.GetComponent<Image>();
            itemImage.sprite = interaction.RequiredItem.ItemImage;
        }
    }

}
