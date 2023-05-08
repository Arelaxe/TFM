using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private Interaction[] interactions;
    public Interaction[] Interactions { get => interactions; }
}
