using UnityEngine;
using System;

[Serializable]
public class InteractableUpdate
{
    [SerializeField]
    public Interactable interactable;

    [SerializeField]
    public int[] availableInteractions;

    [SerializeField]
    public int[] notAvailableInteractions;
}
