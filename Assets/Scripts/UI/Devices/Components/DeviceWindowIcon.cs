using UnityEngine;
using TMPro;

public class DeviceWindowIcon : DeviceIcon
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private GameObject window;

    [Space]
    [SerializeField]
    private Item document;

    public override void Execute()
    {
        HackingAction hackingAction = (HackingAction) SceneLoadManager.Instance.ObjectsData[GlobalConstants.HackingAction];
        HackingAction.HackingStatus status = hackingAction != null ? hackingAction.status : HackingAction.HackingStatus.Failed;

        if (((int)status) < ((int)accessLevel))
        {
            GetManager().ShowRestrictionPanel((int)accessLevel);
        }
        else
        {
            window.GetComponent<DeviceWindow>().Open(title.text, document);
            if (document)
            {
                PlayerManager.Instance.GetDocumentationController().Add(document, false);
            }
        }
    }

}
