using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private int amountOfCards;

    [SerializeField]
    private List<UpgradeCard> cards = new List<UpgradeCard>();

    public void EnableCards()
    {
        List<UpgradeCard> tempCards = new List<UpgradeCard>();
        for (int i = 0; i < amountOfCards; i++)
        {
            tempCards.Add(PickRandomCard());
        }

        tempCards.ForEach(card => card.gameObject.SetActive(true));

        foreach (UpgradeCard card in tempCards) { 
            card.gameObject.SetActive(true);
        }
    }

    public UpgradeCard PickRandomCard()
    {
        float totalWeight = cards.Sum(card => card.Chance);

        float randomValue = Random.Range(0, totalWeight);

        foreach (var card in cards)
        {
            if (randomValue < card.Chance)
            {
                return card;
            }
            randomValue -= card.Chance;
        }

        return null;
    }
}