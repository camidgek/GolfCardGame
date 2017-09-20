using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

	[SerializeField]
	private int _state;
	[SerializeField]
	private int _cardSuit;
	[SerializeField]
	private int _cardValue;
    [SerializeField]
    private int _cardScore;
	[SerializeField]
	private bool _initialized = false;

	private Sprite _cardFace;
	private Sprite _cardBack;

	private GameObject _manager;

	public Card(){}
	public Card(int suit, int value)
	{
		_cardSuit = suit;
		_cardValue = value;
	}

	void Start()
	{
		_state = 0;
        _cardScore = 0;
		_manager = GameObject.FindGameObjectWithTag ("Manager");
	}

    // Graphics functions
	public void setupGraphics()
	{
		_cardBack = _manager.GetComponent<GameManager> ().getCardBack ();
		_cardFace = _manager.GetComponent<GameManager> ().getCardFace (_cardSuit, _cardValue);
        updateCard();
	}

    public void updateCard()
    {
        if (_state == 0)
        {
            GetComponent<Image>().sprite = _cardBack;
        }
        else if (_state == 1)
        {
            GetComponent<Image>().sprite = _cardFace;
        }
    }

    // Card Image functions
    public void flipCard()
	{
		if (_state == 0)
        {
            setFaceUp();
		} else if (_state == 1)
        {
            setFaceDown();
        }
    }

    public void setFaceUp()
    {
        _state = 1;
        _cardScore = _cardValue;
        GetComponent<Image>().sprite = _cardFace;
    }

    public void setFaceDown()
    {
        _state = 0;
        _cardScore = 0;
        GetComponent<Image>().sprite = _cardBack;
    }

    // Button functions
    public void buttonPlayer()
    {
        _manager.GetComponent<GameManager>().buttonCard("player", this);
    }

    public void buttonDraw()
    {
        _manager.GetComponent<GameManager>().buttonCard("draw", this);

    }

    public void buttonDiscard()
    {
        _manager.GetComponent<GameManager>().buttonCard("discard", this);
    }

    // Get & Set functions
    public int cardSuit
	{
		get { return _cardSuit; }
		set { _cardSuit = value; }
	}

	public int cardValue
	{
		get { return _cardValue; }
		set { _cardValue = value; }
	}

    public int cardScore
    {
        get { return _cardScore; }
        set { _cardScore = value; }
    }

	public int state
	{
		get { return _state; }
		set { _state = value; }
	}

	public bool initialized
	{
		get { return _initialized; }
		set { _initialized = value; }
	}
}
