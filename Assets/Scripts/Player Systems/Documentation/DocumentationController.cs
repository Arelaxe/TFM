using UnityEngine;

public class DocumentationController : PageController
{
    [SerializeField]
    private DocumentationPage page;

    [SerializeField]
    private Documentation documentation;

    public override Page Page => page;

    public override string MenuAction => PlayerConstants.ActionDocumentation;

    protected override void InitPage()
    {
        page.InitUIElementList(documentation.GetItems().Count);
        page.OnDescriptionRequested += HandleDescriptionRequest;
    }

    protected override void LoadData()
    {
        page.LoadData(documentation);
    }

    public void Clear()
    {
        documentation.Clear();
    }

    public void Add(Item item, bool initUI = true)
    {
        documentation.AddItem(item);
        if (initUI)
        {
            page.InitUIElement();
        }
    }

    // Page Event Handlers

    private void HandleDescriptionRequest(int documentIndex, DocumentElement documentItem)
    {
        Item document = null;
        if (documentIndex < documentation.GetItems().Count)
        {
            document = documentation.GetItems()[documentIndex];
        }
        page.UpdateSelected(document, documentItem);
    }

    public Documentation Documents { get => documentation; }
}
