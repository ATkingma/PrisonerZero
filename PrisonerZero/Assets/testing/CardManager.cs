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
        if (amountOfCards > cards.Count)
        {
            Debug.LogError("Not enough unique cards to fulfill the request.");
            return;
        }

        List<UpgradeCard> availableCards = new List<UpgradeCard>(cards);
        List<UpgradeCard> tempCards = new List<UpgradeCard>();

        for (int i = 0; i < amountOfCards; i++)
        {
            UpgradeCard pickedCard = PickRandomCard(availableCards);
            if (pickedCard != null)
            {
                tempCards.Add(pickedCard);
            }
        }

        tempCards.ForEach(card => card.gameObject.SetActive(true));
    }

    public UpgradeCard PickRandomCard(List<UpgradeCard> availableCards)
    {
        if (availableCards.Count == 0)
        {
            return null;
        }

        float totalWeight = availableCards.Sum(card => card.Chance);
        float randomValue = Random.Range(0, totalWeight);

        foreach (var card in availableCards)
        {
            if (randomValue < card.Chance)
            {
                availableCards.Remove(card);
                return card;
            }
            randomValue -= card.Chance;
        }

        return null;
    }
}