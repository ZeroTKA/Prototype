using DG.Tweening;
using UnityEngine;

public class UIFadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("[UIFadeScript] canvasGroup is null. It'll break for sure.");
        }
    }
    public void FadeToBlack(float duration = 1.7f)
    {
        if (canvasGroup == null)
        {
            Debug.LogError("[UIFadeScript] canvasGroup is null. Can't Fade to black.");
            return;
        }
        canvasGroup.DOFade(1f, duration).SetUpdate(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;
    }
    public void FadeBlackToClear(float duration = 1.7f)
    {
        if (canvasGroup == null)
        {
            Debug.LogError("[UIFadeScript] canvasGroup is null. Can't fade black to clear.");
            return;
        }
        canvasGroup.DOFade(0f, duration).SetUpdate(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
