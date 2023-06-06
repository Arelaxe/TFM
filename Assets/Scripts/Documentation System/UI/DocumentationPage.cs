using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class DocumentationPage : MonoBehaviour, IEventSystemHandler
{
    [SerializeField]
    private DocumentItem documentItem;

    [SerializeField]
    private DocumentDescription documentDescription;

    [SerializeField]
    private RectTransform contentPanel;

    [SerializeField]
    private Scrollbar scrollbar;

    private int readingIndex;

    private List<DocumentItem> documents = new();

    public event Action<int, DocumentItem> OnDocumentDescriptionRequested;

    private void Awake()
    {
        documentDescription.ResetDescription();
    }

    public void LoadItems(Documentation documentation)
    {
        for (int i = 0; i < documentation.GetDocuments().Count; i++)
        {
            documents[i].SetData(documentation.GetDocuments()[i].Name);
        }
    }

    public void InitDocumentsUI(int documentationSize)
    {
        for (int i = 0; i < documentationSize; i++)
        {
            InitDocumentUI();
        }
    }

    public void InitDocumentUI()
    {
        DocumentItem document = Instantiate(documentItem, Vector3.zero, Quaternion.identity, contentPanel);
        document.transform.localPosition = new Vector3(document.transform.position.x, document.transform.position.y, 0);
        document.transform.localScale = new Vector3(1, 1, 1);
        documents.Add(document);

        document.OnDocumentSelect += HandleDocumentSelect;
        document.OnDocumentSubmit += HandleDocumentSubmit;
    }

    public void UpdateSelected(Item item, DocumentItem documentItem)
    {
        documentDescription.SetDescription(item.ItemImage, item.Description);
        documentDescription.SetNavigation(documentItem);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        documentDescription.ResetDescription();
        SelectFirstItemAvailable();
        scrollbar.value = 1;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SelectFirstItemAvailable()
    {
        if (documents.Count != 0)
        {
            documents[0].Select();
        }
    }

    public void SetScrollbar(int target)
    {
        if (documents.Count > 1)
        {
            scrollbar.value = 1 - ((float)target / (float)(documents.Count - 1));
        }
    }

    // Event handlers

    private void HandleDocumentSelect(DocumentItem document)
    {
        SetScrollbar(documents.IndexOf(document));
    }

    private void HandleDocumentSubmit(DocumentItem document)
    {
        OnDocumentDescriptionRequested?.Invoke(documents.IndexOf(document), document);
    }
}
