using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput input;

    [SerializeField]
    private List<Page> pages;

    private int openPage = -1;

    void Start()
    {
        InitInputActions();
        InitPageEvents();
    }

    private void InitInputActions()
    {
        InputAction quickSaveAction = input.actions[PlayerConstants.ActionQuickSave];
        quickSaveAction.performed += (ctx) => { QuickSave(); };
    }

    private void InitPageEvents()
    {
        foreach (var page in pages)
        {
            page.OnShow += HandleShow;
            page.OnHide += HandleHide;
        }
    }

    private void QuickSave()
    {
        PersistenceUtils.Save();
    }

    // Event handlers

    private void HandleShow(Page page)
    {
        if (openPage != -1)
        {
            pages[openPage].Hide();
        }
        openPage = pages.IndexOf(page);
    }

    private void HandleHide(Page page)
    {
        if (openPage == pages.IndexOf(page))
        {
            openPage = -1;
        }
    }
}
