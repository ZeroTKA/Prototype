using DG.Tweening;
using UnityEngine;

public class UIFadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public void FadeToBlack(float duration = 1.7f)
    {
        canvasGroup.DOFade(1f, duration).SetUpdate(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;
    }
    public void FadeBlackToClear(float duration = 1.7f)
    {
        canvasGroup.DOFade(0f, duration).SetUpdate(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
