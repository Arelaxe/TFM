using UnityEngine;

public class ReadAction : Action
{
    [SerializeField]
    private GameObject readDocument;
    [Header("Initialization")]
    [SerializeField]
    private Item document;

    public override void Execute()
    {
        DocumentationController documentationController = PlayerManager.Instance.GetDocumentationController();
        if (!documentationController.Documents.GetItems().Contains(document))
        {
            PlayerManager.Instance.GetDocumentationController().Add(document);
            SoundManager.Instance.PlayPickupDocument();
        }

        GameObject newReadDocument = Instantiate(readDocument);
        newReadDocument.GetComponent<ReadPanel>().Init(document);
        PlayerManager.Instance.GetInGameMenuController().AddAdditionalUI(newReadDocument);
    }
}
