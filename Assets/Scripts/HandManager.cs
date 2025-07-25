using UnityEngine;
using LeoDroidMakerObjects;
using System.Collections.Generic;
using System;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject cardPrefab; //Assing card prefab in inspector
    public Transform handTransform; //Root of the hand position
    public float fanSpread = -7.5f;
    public float cardSpacing = 150f;
    public float verticalSpacing = 100f;
    public List<GameObject> cardsInHand = new List<GameObject>(); //Hold a list of the cards objects in our hand
    void Start()
    {

    }

    public void AddCardToHand(Card cardData)
    {
        //Instantiate a card
        GameObject newCard = Instantiate(original: cardPrefab, position: handTransform.position, rotation: Quaternion.identity, parent: handTransform);
        cardsInHand.Add(newCard);

        newCard.GetComponent<CardDisplay>().cardData = cardData;

        UpdateHandVisuals();
    }

    public void Update()
    {
        UpdateHandVisuals();
    }

    private void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(x: 0f, y: 0f, z: rotationAngle);

            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (cardCount > 1)
            ? (2f * i / (cardCount - 1) - 1f)
            : 0f;

            float verticalOffset = (cardCount > 1)
            ? verticalSpacing * (1 - normalizedPosition * normalizedPosition)
            : 0f;

            cardsInHand[i].transform.localPosition = new Vector3(x: horizontalOffset, y: verticalOffset, z: 0f);
        }
    }
}
