using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class QuestNotificationMark : MonoBehaviour
{
    [SerializeField] private Image markImage;
    [SerializeField] private float showDuration = 0.3f;
    [SerializeField] private float hideDuration = 0.2f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;

    private bool isVisible;
    private Sequence animationSequence;

    private void Awake()
    {
        if (markImage != null)
            markImage.gameObject.SetActive(false);
    }

    public void ShowMark()
    {
        if (isVisible || markImage == null) return;

        animationSequence?.Kill();
        animationSequence = DOTween.Sequence();

        isVisible = true;
        markImage.gameObject.SetActive(true);

        animationSequence.Append(
            markImage.transform.DOScale(1f, showDuration)
                .SetEase(showEase)
                .OnKill(() => markImage.transform.localScale = Vector3.one)
        );
    }

    public void HideMark()
    {
        if (!isVisible || markImage == null) return;

        animationSequence?.Kill();
        animationSequence = DOTween.Sequence();

        animationSequence.Append(
            markImage.transform.DOScale(0f, hideDuration)
                .SetEase(hideEase)
                .OnComplete(() =>
                {
                    markImage.gameObject.SetActive(false);
                    isVisible = false;
                })
        );
    }

    private void OnDestroy()
    {
        animationSequence?.Kill();
    }
}
