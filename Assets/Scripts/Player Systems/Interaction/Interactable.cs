using UnityEngine;

[RequireComponent(typeof(DynamicObject))]
public class Interactable : MonoBehaviour
{
    [SerializeField]
    private Interaction[] interactions;
    public Interaction[] Interactions { get => interactions; }

    private static readonly string KEY_INTERACTIONS = "interactions";

    private void Start()
    {
        InitDynamicObject();
    }

    private void InitDynamicObject()
    {
        DynamicObject dynamicObject = GetComponent<DynamicObject>();
        dynamicObject.OnPrepareToSave += PrepareToSaveObjectState;
        dynamicObject.OnLoadObjectState += LoadObjectState;
    }

    private void PrepareToSaveObjectState(ObjectState objectState)
    {
        objectState.extendedData[KEY_INTERACTIONS] = interactions;
    }

    private void LoadObjectState(ObjectState objectState)
    {
        Interaction[] savedInteractions = (Interaction[]) objectState.extendedData[KEY_INTERACTIONS];
        for (int i = 0; i < interactions.Length; i++)
        {
            interactions[i].SetAvailable(savedInteractions[i].IsAvailable);
        }
    }
}
