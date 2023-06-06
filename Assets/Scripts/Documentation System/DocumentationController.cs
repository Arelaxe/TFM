using UnityEngine.InputSystem;
using UnityEngine;

public class DocumentationController : MonoBehaviour
{
    private PlayerInput input;
    private InputAction documentationAction;

    [SerializeField]
    private DocumentationPage page;

    [SerializeField]
    private Documentation documentation;

    public void Start()
    {
        InitInputActions();
        InitPage();
    }

    void Update()
    {
        if (documentationAction.triggered)
        {
            DualCharacterController playerController = PlayerManager.Instance.GetDualCharacterController();
            InteractionController interactionController = PlayerManager.Instance.GetInteractionController();

            if (!page.isActiveAndEnabled)
            {
                InventoryController inventoryController = PlayerManager.Instance.GetInventoryController();
                if (inventoryController.IsActive())
                {
                    inventoryController.Hide();
                }

                playerController.SetMobility(false);
                interactionController.SetInteractivity(false);
                interactionController.DestroyInteractions();

                page.LoadItems(documentation);
                page.Show();
            }
            else
            {
                playerController.SetMobility(true);
                interactionController.SetInteractivity(true);
                page.Hide();
            }
        }
    }

    private void InitInputActions()
    {
        input = GetComponent<PlayerInput>();
        documentationAction = input.actions[PlayerConstants.ActionDocumentation];
    }

    private void InitPage()
    {
        page.InitDocumentsUI(documentation.GetDocuments().Count);
        page.OnDocumentDescriptionRequested += HandleDescriptionRequest;
    }

    public void AddDocument(Item item)
    {
        documentation.AddDocument(item);
        page.InitDocumentUI();
    }

    public bool IsActive()
    {
        return page.isActiveAndEnabled;
    }

    public void Hide()
    {
        page.Hide();
    }

    // Page Event Handlers

    private void HandleDescriptionRequest(int documentIndex, DocumentItem documentItem)
    {
        Item document = null;
        if (documentIndex < documentation.GetDocuments().Count)
        {
            document = documentation.GetDocuments()[documentIndex];
        }
        page.UpdateSelected(document, documentItem);
    }
}
