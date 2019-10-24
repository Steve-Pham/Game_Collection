using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
     public static bool halt = false;

     [SerializeField]
     private int stateofcard; // this is the state of the card
     [SerializeField]
     private int cardValue; // This is the value of the card
     [SerializeField]
     private bool initialized = false;

     private Sprite cardBack;
     private Sprite cardFace;

     private GameObject gameManager; 

     // This is to initialize the state of the card
     void Start()
     {
          stateofcard = 1;
          gameManager = GameObject.FindGameObjectWithTag("Manager");
     }

     // This is used every round to set up the card
     public void roundlySetup()
     {
          cardBack = gameManager.GetComponent<CardController>().getBack();
          cardFace = gameManager.GetComponent<CardController>().getFace(cardValue);

          flipCard();
     }

     // This method is get the state of the card
     public void flipCard()
     {
          if (stateofcard == 0)
          {
               stateofcard = 1;
          }
          else if (stateofcard == 1)
          {
               stateofcard = 0;
          }


          if (stateofcard == 0 && !halt)
          {
               GetComponent<Image>().sprite = cardBack;
          }
          else if (stateofcard == 1 && !halt)
          {
               GetComponent<Image>().sprite = cardFace;
          }

     }

     // GETTER/SETTER methods to get variable names
     public int _cardValue
     {
          get
          {
               return cardValue;
          }
          set
          {
               cardValue = value; 
          }
     }

     public int _stateofcard
     {
          get
          {
               return stateofcard;
          }
          set
          {
               stateofcard = value;     
          }
     }

     public bool _initialized
     {
          get
          {
               return initialized;
          }
          set
          {
               initialized = value; 
          }
     }


     // This is the to call the coroutine, so the game doesnt go too fast
     public void sktfaker()
     {
          StartCoroutine(pause());
     }


     // This method is needed to start the coroutine
     IEnumerator pause()
     {
          yield return new WaitForSeconds(1);

          if (stateofcard == 0)
          {
               GetComponent<Image>().sprite = cardBack;
          }
          else if (stateofcard == 1)
          {
               GetComponent<Image>().sprite = cardFace;
          }
          halt = false; 
     }
}
