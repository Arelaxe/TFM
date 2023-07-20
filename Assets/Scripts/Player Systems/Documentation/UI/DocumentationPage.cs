using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocumentationPage : Page
{
    [SerializeField]
    private DocumentElement element;

    [SerializeField]
    private DocumentDescription description;

    [SerializeField]
    private Scrollbar scrollbar;

    private List<DocumentElement> elementList = new();

    public event Action<int, DocumentElement> OnDescriptionRequested;

    private void Awake()
    {
        description.ResetDescription();
    }

    public void LoadData(Documentation documentation)
    {
        for (int i = 0; i < documentation.GetItems().Count; i++)
        {
            elementList[i].SetData(documentation.GetItems()[i].Name);
        }
    }

    public void InitUIElementList(int documentationSize)
    {
        for (int i = 0; i < documentationSize; i++)
        {
            InitUIElement();
        }
    }

    public void InitUIElement()
    {
        DocumentElement document = Instantiate(element, Vector3.zero, Quaternion.identity, contentPanel);
        document.transform.localPosition = new Vector3(document.transform.position.x, document.transform.position.y, 0);
        document.transform.localScale = new Vector3(1, 1, 1);
        elementList.Add(document);

        document.OnElementSelect += HandleSelect;
        document.OnElementSubmit += HandleSubmit;
    }

    public void UpdateSelected(Item item, DocumentElement documentItem)
    {
        description.SetDescription(item.ItemImage, item.Name, item.Description);
        description.SetNavigation(documentItem);

        HackingExtension extension = PlayerManager.Instance.GetInGameMenuController().GetHackingExtension();
        if (extension)
        {
            extension.SetDocument(item);
        }
    }

    public override void Show()
    {
        base.Show();
        description.ResetDescription();
        SelectFirstAvailable();
        scrollbar.value = 1;
    }

    public void SelectFirstAvailable()
    {
        if (elementList.Count != 0)
        {
            elementList[0].Select();
        }
    }

    public void SetScrollbar(int target)
    {
        if (elementList.Count > 1)
        {
            scrollbar.value = 1 - ((float)target / (float)(elementList.Count - 1));
        }
    }

    // Event handlers

    private void HandleSelect(DocumentElement document)
    {
        SetScrollbar(elementList.IndexOf(document));
        description.SetNavigation(document);
    }

    private void HandleSubmit(DocumentElement document)
    {
        OnDescriptionRequested?.Invoke(elementList.IndexOf(document), document);
    }
}
