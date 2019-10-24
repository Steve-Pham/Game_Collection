using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerMenu : MonoBehaviour
{
     // Audio Instance variables for the Winner music
     public AudioClip MusicClip;
     public AudioSource MusicSource;

     public Text scoretext; // text for final score
     public Text timeText;  // text for final time


     /** This method is initialize the frame, and give the score, time and play the audio
      */ 
     void Start()
     {
          MusicSource.clip = MusicClip;
          MusicSource.Play();
          scoretext.text = "Your Score was: " + Score.score;
          timeText.text = "Total Time Elapsed: " + Timer.totalTime() + "s";
     }


     // This function is used to determine what the buttons do
     // If manager == 0, then load the game (play again)
     // If manager == 1, then quit the game (end)
     public void MenuButtons(int manager)
     {
          if (manager == 0)
          {
               SceneManager.LoadScene("Game");
          }
          else if (manager == 1)
          {
               SceneManager.LoadScene("MainMenuCard");
          }
          else
          {
               Application.Quit();
          }
     }
}
