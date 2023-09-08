using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndMenuController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup titleGroup;

    [SerializeField]
    private CanvasGroup buttonGroup;

    [SerializeField]
    private Button quit;

    private void Start()
    {
        PlayerManager.Instance.GetDualCharacterController().SetMobility(false);
        StartCoroutine(InitUI());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator InitUI()
    {
        yield return ShowCanvasGroup(titleGroup, 0.02f);
        yield return ShowCanvasGroup(buttonGroup, 0.01f);
        quit.Select();
    }

    public IEnumerator ShowCanvasGroup(CanvasGroup group, float delay)
    {
        while (group.alpha < 1)
        {
            group.alpha += 0.01f;
            yield return new WaitForSeconds(delay);
        }

    }
}
