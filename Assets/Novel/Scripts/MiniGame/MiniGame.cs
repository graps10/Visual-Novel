using System;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class MiniGame : MonoBehaviour
{
    [SerializeField] private RectTransform cardContainer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] cardSprites;

    private List<Card> cards = new List<Card>();
    private Card firstSelected, secondSelected;
    private bool canSelect = true;
    private Action<bool> onGameCompleted;
    private int matchedPairs;
    private int totalPairs;

    public void Initialize(int pairsCount, Action<bool> completionCallback)
    {
        onGameCompleted = completionCallback;
        totalPairs = Mathf.Min(pairsCount, cardSprites.Length);
        matchedPairs = 0;
        SetupCards();
    }

    private void SetupCards()
    {
        // Clear existing cards
        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);
        cards.Clear();

        // Create deck
        List<Sprite> deck = new List<Sprite>();
        for (int i = 0; i < totalPairs; i++)
        {
            deck.Add(cardSprites[i]);
            deck.Add(cardSprites[i]); // Add pair
        }

        deck = Shuffle(deck);

        // Create cards
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardContainer);
            Card card = newCard.GetComponent<Card>();
            card.Initialize(deck[i], OnCardSelected, CanSelectCard);
            cards.Add(card);
        }
    }

    private List<Sprite> Shuffle(List<Sprite> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Sprite temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        return deck;
    }

    private bool CanSelectCard() => canSelect;
    private async void OnCardSelected(Card card)
    {
        if (!canSelect || card.IsMatched || card == firstSelected) return;

        if (firstSelected == null)
            firstSelected = card;
        else
        {
            canSelect = false;
            secondSelected = card;

            await UniTask.Delay(1000);

            if (firstSelected.CardSprite == secondSelected.CardSprite)
            {
                firstSelected.SetMatched();
                secondSelected.SetMatched();
                matchedPairs++;

                if (matchedPairs >= totalPairs)
                {
                    await UniTask.Delay(500);
                    onGameCompleted?.Invoke(true);
                }
            }
            else
            {
                await UniTask.WhenAll(
                    firstSelected.FlipBack(),
                    secondSelected.FlipBack()
                );
            }

            firstSelected = null;
            secondSelected = null;
            canSelect = true;
        }
    }
}