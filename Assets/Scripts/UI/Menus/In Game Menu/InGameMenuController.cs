using System.Collections.Generic;
using UnityEngine;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField]
    private List<Page> pages;

    private int openPage = -1;

    void Start()
    {
        InitPageEvents();
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
}
