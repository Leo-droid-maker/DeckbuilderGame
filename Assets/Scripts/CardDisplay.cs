using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LeoDroidMakerObjects;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public Image[] typeImages;

    private Color[] cardColors = {
        Color.red, //Fire
        new Color(0.8f, 0.52f, 0.24f), //Earth
        Color.blue, //Water
        new Color(0.23f, 0.06f, 0.21f), //Dark
        Color.yellow, //Light
        Color.cyan //Air
    };

    private readonly Dictionary<Card.CardType, Color> typeColors = new() {
    { Card.CardType.Fire, new Color(1f, 0.41f, 0.48f, 1f) },
    { Card.CardType.Earth, new Color(0.8f, 0.52f, 0.24f) },
    { Card.CardType.Water, new Color(0.41f, 0.48f, 1f, 1f) },
    { Card.CardType.Dark, new Color(0.47f, 0.0f, 0.40f) },
    { Card.CardType.Light, new Color(1f, 0.41f, 0.048f, 1f) },
    { Card.CardType.Air, new Color(0.41f, 1f, 1f, 1f) }
};

    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        cardImage.color = cardColors[(int)cardData.cardType[0]];

        nameText.text = cardData.cardName;
        healthText.text = cardData.health.ToString();
        damageText.text = $"{cardData.damageMin}/{cardData.damageMax}";

        for (int i = 0; i < typeImages.Length; i++)
        {
            if (i < cardData.cardType.Count)
            {
                typeImages[i].gameObject.SetActive(true);
                typeImages[i].color = typeColors[cardData.cardType[i]];
            }
            else
            {
                typeImages[i].gameObject.SetActive(false);
            }
        }
    }
}
