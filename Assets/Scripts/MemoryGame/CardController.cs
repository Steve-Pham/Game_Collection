using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardController : MonoBehaviour
{
     // These are the public variables needed 
     public Sprite[] faces;
     public Sprite back;
     public GameObject[] deck;
     public Text numMatches;

     public Text scoretext; 

     // Private variables
     private bool initialize = false;
     private int totalmatches = 13;

     // Audio sources for clicking, matched sounds, and non-matched sounds
     public AudioClip MusicClip;
     public AudioSource MusicSource;

     public AudioClip MusicClip2;
     public AudioSource MusicSource2;

     // This is used at the start of the game to start the timer and initialize the score of the game
     void Start()
     {
          Timer.time1 = Time.time; 
          Score.score = 1000;
          MusicSource.clip = MusicClip;
          MusicSource2.clip = MusicClip2;
     }


     // Update is called once per frame to setup cards and check if there is a match
     void Update()
    {
        if (!initialize)
        {
          setupCards(); 
        }
        
        if (Input.GetMouseButtonUp(0))
        {
               checkCards();
        }
    }

     // This method is used to setup the cards at the beginning of the game, and so on when there are matched cards or not picked
     public void setupCards()
     {
          for (int i = 0; i < 2; i++)
          {
               for (int j = 1; j < 14; j++)
               {
                    bool test = false;
                    int choice = 0;
                    while (!test)
                    {
                         choice = Random.Range(0, deck.Length);
                         test = !(deck[choice].GetComponent<Card>()._initialized); 
                    }
                    deck[choice].GetComponent<Card>()._cardValue = j;
                    deck[choice].GetComponent<Card>()._initialized = true;
               }
          }

          foreach (GameObject c in deck)
          {
               c.GetComponent<Card>().roundlySetup();
          }


          if (!initialize)
          {
               initialize = true;
          }
     }

     // Get methods for the face and back of a card
     public Sprite getBack()
     {
          return back; 
     }

     public Sprite getFace(int n)
     {
          return faces[n-1];
     }

     // This method is used to check cards using an arrayList
     public void checkCards()
     {
          List<int> list = new List<int>();

          for (int i = 0; i < deck.Length; i++)
          {
               if (deck[i] != null && deck[i].GetComponent<Card>()._stateofcard == 1)
               {
                    list.Add(i);
               }
          }

          if (list.Count == 2)
          {
               compareCards(list);
          }
     }

     // This method is used to compare the cards, to see whether they are equal or not
     // This method also takes care of the score, whether to decrease it depending on a match or not
     public void compareCards(List<int> list)
     {
          Card.halt = true;

          int n = 0;

          if (deck[list[0]].GetComponent<Card>()._cardValue == deck[list[1]].GetComponent<Card>()._cardValue)
          {
               MusicSource.Play();
               n = 2;
               totalmatches = totalmatches - 1;
               numMatches.text = "Number of Matches: " + totalmatches;
               if (totalmatches == 0)
               {
                    Timer.time2 = Time.time;
                    if (Users.useratm == null)
                    {
                         MainMenu.logs.Add(new GameLog("admin", System.DateTime.Now.ToString(), Score.score.ToString(), "n/a"));
                    }
                    else
                    {
                         MainMenu.logs.Add(new GameLog(Users.useratm.username, System.DateTime.Now.ToString(), Score.score.ToString(), "n/a"));
                    }
                    MainMenu.SaveGameData();

                    SceneManager.LoadScene("Winner");
               }
               StartCoroutine(Pause(2, deck[list[0]], deck[list[1]]));

          }

          if (deck[list[0]].GetComponent<Card>()._cardValue != deck[list[1]].GetComponent<Card>()._cardValue)
          {
               MusicSource2.Play();
               Score.decScore(); 
               scoretext.text = "Score: " + Score.score; 
               if (Score.score == 0)
               {
                    Timer.time2 = Time.time;
                    SceneManager.LoadScene("Lose");
               }
          }

          for (int i = 0; i < list.Count; i++)
          {
               deck[list[i]].GetComponent<Card>()._stateofcard = n;
               deck[list[i]].GetComponent<Card>().sktfaker();
          }

     }

     // This is needed to cause a delay when there is a set of matched cards, so they don't disappear right away
     IEnumerator Pause(int time, GameObject card1, GameObject card2)
     {
          yield return new WaitForSeconds(time);
          Destroy(card1);
          Destroy(card2);
     }

}
