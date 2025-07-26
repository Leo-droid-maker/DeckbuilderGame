using System.Collections.Generic;
using LeoDroidMakerObjects;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    private int currenIndex = 0;
    public int maxHandSize = 7;

    public void Start()
    {
        Card[] cards = Resources.LoadAll<Card>("Cards");

        allCards.AddRange(cards);

        HandManager hand = FindFirstObjectByType<HandManager>();
        for (int i = 0; i < maxHandSize; i++)
        {
            DrawCard(handManager: hand);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (handManager.cardsInHand.Count == maxHandSize)
            return;
        if (allCards.Count == 0)
            return;
        Card nextCard = allCards[currenIndex];
        handManager.AddCardToHand(nextCard);
        allCards.RemoveAt(currenIndex);
        currenIndex = (currenIndex + 1) % allCards.Count;
    }
}
