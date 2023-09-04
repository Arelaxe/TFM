using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class OpenPanel : Action
{
    [SerializeField]
    private GameObject panel;

    [Header("Initialization")]
    [SerializeField]
    private string tagFuse;
    [SerializeField]
    private string panelInteraction;
    [SerializeField]
    private Interactable panelInteractable;
    [SerializeField]
    private GameObject[] destroyables;
    [SerializeField]
    private List<int> array;
    [SerializeField]
    private Sprite onFuseImage;
    public override void Execute()
    {
        GameObject newPanel = Instantiate(panel);
        PlayerManager.Instance.GetInGameMenuController().AddAdditionalUI(newPanel);
        newPanel.GetComponent<Panel>().Init(tagFuse, panelInteraction, panelInteractable, array, onFuseImage, destroyables);
    }

}