using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField]
    private List<Page> pages;

    private PlayerInput input;
    private bool switchPageAvailable = true;

    private int openPage = -1;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        InitPageEvents();
    }

    private void Update()
    {
        InputSystemUtils.ControlCursor(input);
    }

    private void InitPageEvents()
    {
        foreach (var page in pages)
        {
            page.OnShow += HandleShow;
            page.OnHide += HandleHide;
        }
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

    public bool SwitchPageAvailable { get => switchPageAvailable; }
    public void SetSwitchPageAvailability(bool switchPageAvailable)
    {
        this.switchPageAvailable = switchPageAvailable;
    }
}
