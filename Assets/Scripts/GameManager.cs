using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Sprite[] cardFacesSpades;
    public Sprite[] cardFacesHearts;
    public Sprite[] cardFacesClubs;
    public Sprite[] cardFacesDiamonds;
    public Sprite cardBack;
    public GameObject[] cardsPlayer;
    public GameObject[] cardsOpponent;
    public GameObject cardDraw;
    public GameObject cardDiscard;
    public Text txtScore;

    private bool _init = false;
    public Deck deck = new Deck();

    public Card currentCard = new Card();

    public enum _states
    {
        Init,
        First_Flip,
        Init_Draw,
        Draw,
        Init_Place,
        Place,
        Init_Swap,
        Swap,
        End
    }
    private _states _state = _states.Init;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case _states.Init:
                deck.shuffle();
                initializeCards();
                _state = _states.First_Flip;
                break;

            case _states.First_Flip:
                enableButtons("player");
                break;

            case _states.Init_Draw:
                //refreshCards();
                disableButtons("player");
                enableButtons("draw");
                enableButtons("discard");
                _state = _states.Draw;
                break;

            case _states.Draw:
                break;

            case _states.Init_Place:
                enableButtons("player");
                disableButtons("draw");
                enableButtons("discard");
                _state = _states.Place;
                break;

            case _states.Place:
                break;

            case _states.Init_Swap:
                enableButtons("player");
                disableButtons("draw");
                disableButtons("discard");
                _state = _states.Swap;
                break;

            case _states.Swap:
                break;

            case _states.End:
                disableButtons("player");
                disableButtons("draw");
                disableButtons("discard");
                break;
        }
        checkComplete();
    }

    void initializeCards()
    {
        Card c = new Card();

        // Players Cards
        for (int i = 0; i < cardsPlayer.Length; i++)
        {
            c = deck.takeCard("draw");
            cardsPlayer[i].GetComponent<Card>().cardSuit = c.cardSuit;
            cardsPlayer[i].GetComponent<Card>().cardValue = c.cardValue;
            cardsPlayer[i].GetComponent<Card>().cardScore = c.cardValue;
            cardsPlayer[i].GetComponent<Card>().initialized = true;
        }

        // Deck Cards
        c = deck.takeCard("draw");
        cardDraw.GetComponent<Card>().cardSuit = c.cardSuit;
        cardDraw.GetComponent<Card>().cardValue = c.cardValue;
        cardDraw.GetComponent<Card>().initialized = true;

        c = deck.takeCard("draw");
        cardDiscard.GetComponent<Card>().cardSuit = c.cardSuit;
        cardDiscard.GetComponent<Card>().cardValue = c.cardValue;
        cardDiscard.GetComponent<Card>().initialized = true;
        deck.discard(c);

        refreshCards();
        cardDiscard.GetComponent<Card>().flipCard();

        if (!_init)
            _init = true;
    }

    void refreshCards()
    {
        foreach (GameObject c in cardsPlayer)
        {
            c.GetComponent<Card>().setupGraphics();
        }

        resetScores();
        cardDraw.GetComponent<Card>().setupGraphics();
        cardDiscard.GetComponent<Card>().setupGraphics();

        updateScore();
    }

    // Button functions
    void enableButtons(string group)
    {
        switch (group)
        {
            case "player":
                for (int i = 0; i < cardsPlayer.Length; i++)
                {
                    cardsPlayer[i].GetComponent<Button>().interactable = true;
                }
                break;
            case "draw":
                cardDraw.GetComponent<Button>().interactable = true;
                break;
            case "discard":
                cardDiscard.GetComponent<Button>().interactable = true;
                break;
        }
    }

    void disableButtons(string group)
    {
        switch (group)
        {
            case "player":
                for (int i = 0; i < cardsPlayer.Length; i++)
                {
                    cardsPlayer[i].GetComponent<Button>().interactable = false;
                }
                break;
            case "draw":
                cardDraw.GetComponent<Button>().interactable = false;
                break;
            case "discard":
                cardDiscard.GetComponent<Button>().interactable = false;
                break;
        }
    }

    public void buttonCard(string type, Card card)
    {
        switch(type)
        {
            case "player":
                switch (_state)
                {
                    case _states.First_Flip:
                        card.GetComponent<Card>().flipCard();
                        refreshCards();
                        _state = _states.Init_Draw;
                        break;
                    case _states.Place:
                    case _states.Swap:
                        cardDraw.GetComponent<Card>().setFaceDown();
                        card.GetComponent<Card>().setFaceUp();
                        placeCard(card);
                        _state = _states.Init_Draw;
                        break;
                }
                break;
            case "draw":
                switch (_state)
                {
                    case _states.Draw:
                        currentCard = card;
                        card.GetComponent<Card>().flipCard();
                        _state = _states.Init_Place;
                        break;
                }
                break;
            case "discard":
                switch (_state)
                {
                    case _states.Draw:
                        currentCard = card;
                        _state = _states.Init_Swap;
                        break;
                    case GameManager._states.Place:
                        cardDraw.GetComponent<Card>().setFaceDown();
                        discardCard(card);
                        _state = _states.Init_Draw;
                        break;
                }
                break;
        }
    }

    // Card functions
    public void placeCard(Card destination)
    {
        ///*
        switch(_state)
        {
            // From draw to board
            case _states.Place:
                deck.discard(cardDiscard.GetComponent<Card>());

                cardDiscard.GetComponent<Card>().cardSuit = destination.GetComponent<Card>().cardSuit;
                cardDiscard.GetComponent<Card>().cardValue = destination.GetComponent<Card>().cardValue;

                destination.GetComponent<Card>().cardSuit = currentCard.cardSuit;
                destination.GetComponent<Card>().cardValue = currentCard.cardValue;

                currentCard = deck.takeCard("draw");
                cardDraw.GetComponent<Card>().cardSuit = currentCard.cardSuit;
                cardDraw.GetComponent<Card>().cardValue = currentCard.cardValue;
                break;
            
            // From discard to board
            case _states.Swap:
                int suit = currentCard.cardSuit;
                int value = currentCard.cardValue;

                cardDiscard.GetComponent<Card>().cardSuit = destination.GetComponent<Card>().cardSuit;
                cardDiscard.GetComponent<Card>().cardValue = destination.GetComponent<Card>().cardValue;

                destination.GetComponent<Card>().cardSuit = suit;
                destination.GetComponent<Card>().cardValue = value;

                cardDraw.GetComponent<Card>().setFaceDown();
                break;
         }
         //*/
        
        refreshCards();
    }

    public void discardCard(Card destination)
    {
        deck.discard(cardDiscard.GetComponent<Card>());
        cardDiscard.GetComponent<Card>().cardSuit = currentCard.cardSuit;
        cardDiscard.GetComponent<Card>().cardValue = currentCard.cardValue;

        currentCard = deck.takeCard("draw");
        cardDraw.GetComponent<Card>().cardSuit = currentCard.cardSuit;
        cardDraw.GetComponent<Card>().cardValue = currentCard.cardValue;

        refreshCards();
    }

    // Board functions
    public void checkComplete()
    {
        foreach (GameObject card in cardsPlayer)
        {
            if (card.GetComponent<Card>().state == 0)
            {
                return;
            }
        }
        updateScore();
        _state = _states.End;
    }

    public void updateScore()
    {
        int score = 0;
        checkScore();
        foreach (GameObject card in cardsPlayer)
        {
            if (card.GetComponent<Card>().state == 1)
            {
                score = score + card.GetComponent<Card>().cardScore;
            }
        }
        txtScore.GetComponent<Text>().text = score.ToString();
        resetScores();
    }

    void resetScores()
    {
        foreach (GameObject card in cardsPlayer)
        {
            if (card.GetComponent<Card>().cardValue == 11 || card.GetComponent<Card>().cardValue == 12)
                card.GetComponent<Card>().cardScore = 10;
            else if (card.GetComponent<Card>().cardValue == 13)
                card.GetComponent<Card>().cardScore = 0;
            else
                card.GetComponent<Card>().cardScore = card.GetComponent<Card>().cardValue;
        }
    }

    public void checkScore()
    {
        int start;
        // 3 in a row
        for (int row = 0; row < 3; row++)
        {
            // Row 0 = 0,1,2 | Row 1 = 3,4,5 | Row 2 = 6,7,8
            start = row * 3;

            // For cards in row...
            for (int i = start; i < start + 3; i++)
            {
                // Return if not face up
                if (cardsPlayer[i].GetComponent<Card>().state == 0)
                    break;
            }

            // If all cards are face up...
            // and the same value..
            if (cardsPlayer[start].GetComponent<Card>().cardValue == cardsPlayer[start + 1].GetComponent<Card>().cardValue &&
                cardsPlayer[start + 1].GetComponent<Card>().cardValue == cardsPlayer[start + 2].GetComponent<Card>().cardValue)
            {
                // Their scores cancel
                cardsPlayer[start].GetComponent<Card>().cardScore = 0;
                cardsPlayer[start + 1].GetComponent<Card>().cardScore = 0;
                cardsPlayer[start + 2].GetComponent<Card>().cardScore = 0;
            }
        }

        // 3 in a column
        for (int col = 0; col < 3; col++)
        {
            // Col 0 = 0,3,6 | Col 1 = 1,4,7 | Col 2 = 2,5,8
            start = col;

            // For cards in column...
            for (int i = start; i < 9; i = i + 3)
            {
                // Return if not face up
                if (cardsPlayer[i].GetComponent<Card>().state == 0)
                    break;
            }

            // If all cards are face up...
            // and the same value..
            if (cardsPlayer[start].GetComponent<Card>().cardValue == cardsPlayer[start + 3].GetComponent<Card>().cardValue &&
                cardsPlayer[start + 3].GetComponent<Card>().cardValue == cardsPlayer[start + 6].GetComponent<Card>().cardValue)
            {
                // Their scores cancel
                cardsPlayer[start].GetComponent<Card>().cardScore = 0;
                cardsPlayer[start + 3].GetComponent<Card>().cardScore = 0;
                cardsPlayer[start + 6].GetComponent<Card>().cardScore = 0;
            }
        }

        // Pairs in row
        for (int row = 0; row < 3; row++)
        {
            start = row * 3;
            if (cardsPlayer[start].GetComponent<Card>().cardValue == cardsPlayer[start + 1].GetComponent<Card>().cardValue &&
                cardsPlayer[start].GetComponent<Card>().cardScore == cardsPlayer[start + 1].GetComponent<Card>().cardScore)
            {
                cardsPlayer[start].GetComponent<Card>().cardScore = 0;
                cardsPlayer[start + 1].GetComponent<Card>().cardScore = 0;
            }
            if (cardsPlayer[start + 1].GetComponent<Card>().cardValue == cardsPlayer[start + 2].GetComponent<Card>().cardValue &&
                cardsPlayer[start + 1].GetComponent<Card>().cardScore == cardsPlayer[start + 2].GetComponent<Card>().cardScore)
            {
                cardsPlayer[start + 1].GetComponent<Card>().cardScore = 0;
                cardsPlayer[start + 2].GetComponent<Card>().cardScore = 0;
            }
        }

        // Pairs in column
        for (int col = 0; col < 3; col++)
        {
            start = col;
            if (cardsPlayer[start].GetComponent<Card>().cardValue == cardsPlayer[start + 3].GetComponent<Card>().cardValue &&
                cardsPlayer[start].GetComponent<Card>().cardScore == cardsPlayer[start + 3].GetComponent<Card>().cardScore)
            {
                cardsPlayer[start].GetComponent<Card>().cardScore = 0;
                cardsPlayer[start + 3].GetComponent<Card>().cardScore = 0;
            }
            if (cardsPlayer[start + 3].GetComponent<Card>().cardValue == cardsPlayer[start + 6].GetComponent<Card>().cardValue &&
                cardsPlayer[start + 3].GetComponent<Card>().cardScore == cardsPlayer[start + 6].GetComponent<Card>().cardScore)
            {
                cardsPlayer[start + 3].GetComponent<Card>().cardScore = 0;
                cardsPlayer[start + 6].GetComponent<Card>().cardScore = 0;
            }
        }
    }

    // Get & Set functions
    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int suit, int value)
    {
        switch (suit)
        {
            default:
            case 0:
                return cardFacesSpades[value - 1];
            case 1:
                return cardFacesHearts[value - 1];
            case 2:
                return cardFacesClubs[value - 1];
            case 3:
                return cardFacesDiamonds[value - 1];
        }
    }

    public _states state
        {
            get { return _state; }
            set { _state = value; }
        }
}
