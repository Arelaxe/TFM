using UnityEngine;
using UnityEngine.UI;

public class DocumentationController : PageController
{
    [SerializeField]
    private DocumentationPage page;

    [SerializeField]
    private Documentation documentation;

    private Item selectedDocument;

    public override Page Page => page;

    public override string MenuAction => PlayerConstants.ActionDocumentation;

    [SerializeField]
    private Image newDoc;

    protected override void InitPage()
    {
        page.InitUIElementList(documentation.GetItems().Count);
        page.OnDescriptionRequested += HandleDescriptionRequest;
    }

    public override void LoadData()
    {
        page.LoadData(documentation);
    }

    public void Clear()
    {
        documentation.Clear();
        page.ClearData();
    }

    public void Add(Item item, bool initUI = true)
    {
        documentation.AddItem(item);
        if (initUI)
        {
            page.InitUIElement();
        }

        ShowNewDocIcon(true);
    }

    public void ShowNewDocIcon(bool show)
    {
        newDoc.enabled = show;
    }

    // Page Event Handlers

    private void HandleDescriptionRequest(int documentIndex, DocumentElement documentItem)
    {
        Item document = null;
        if (documentIndex < documentation.GetItems().Count)
        {
            document = documentation.GetItems()[documentIndex];
        }

        selectedDocument = document;
        page.UpdateSelected(document, documentItem);
    }

    public Documentation Documents { get => documentation; }
    public Item SelectedDocument { get => selectedDocument; }
}
