using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Naninovel;
using System;

public class Card : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Button cardButton;

    private Sprite frontSprite;
    private bool isFlipped = false;
    private bool isMatched = false;
    private Action<Card> onCardSelected;
    private Func<bool> canSelectCard;

    public Sprite CardSprite => frontSprite;
    public bool IsMatched => isMatched;

    void OnClick()
    {
        if (!isFlipped && !isMatched && canSelectCard.Invoke())
            FlipCard().Forget();
    }

    public void Initialize(Sprite front, Action<Card> selectCallback, Func<bool> canSelect)
    {
        frontSprite = front;
        cardImage.sprite = backSprite;

        isFlipped = false;
        isMatched = false;

        onCardSelected = selectCallback;
        canSelectCard = canSelect;

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(() => OnClick());
    }

    public async UniTask FlipCard()
    {
        if (isFlipped || isMatched) return;

        isFlipped = true;
        onCardSelected?.Invoke(this);

        await FlipAnimation(frontSprite);
    }

    public async UniTask FlipBack()
    {
        if (!isFlipped || isMatched) return;

        await FlipAnimation(backSprite);
        isFlipped = false;
    }

    public void SetMatched()
    {
        isMatched = true;
        cardButton.interactable = false;
    }

    private async UniTask FlipAnimation(Sprite targetSprite)
    {
        await cardImage.transform.DOScaleX(0, 0.2f).AsyncWaitForCompletion();
        cardImage.sprite = targetSprite;
        await cardImage.transform.DOScaleX(1, 0.2f).AsyncWaitForCompletion();
    }
}