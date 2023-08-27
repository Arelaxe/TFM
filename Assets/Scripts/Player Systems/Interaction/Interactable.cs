using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicObject))]
public class Interactable : MonoBehaviour
{
    [SerializeField]
    private Interaction[] interactions;
    public Interaction[] Interactions { get => interactions; }

    private static readonly string KEY_INTERACTIONS = "Interactable";

    private void Awake()
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
        List<InteractionData> interactionDatas = new();
        foreach (Interaction interaction in interactions)
        {
            interactionDatas.Add(new(interaction.Name, interaction.IsAvailable, interaction.IsBlocked, interaction.TimesExecuted));
        }
        objectState.extendedData[KEY_INTERACTIONS] = interactionDatas;
    }

    private void LoadObjectState(ObjectState objectState)
    {
        List<InteractionData> interactionDatas = PersistenceUtils.GetList<InteractionData>(objectState.extendedData[KEY_INTERACTIONS]);
        for (int i = 0; i < interactions.Length; i++)
        {
            interactions[i].SetAvailable(interactionDatas[i].available);
            interactions[i].SetBlocked(interactionDatas[i].blocked);
            interactions[i].SetExecuted(interactionDatas[i].timesExecuted);
        }
    }

    public Interaction GetInteraction(string name)
    {
        Interaction result = null;

        foreach (Interaction interaction in interactions)
        {
            if (interaction.Name == name)
            {
                result = interaction;
                break;
            }
        }

        return result;
    }
}
