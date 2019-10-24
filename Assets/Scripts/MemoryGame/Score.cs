using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
     public static int score; // This variable is used to keep the score of the game, it is initialized to 1000 at the start of the game

     // This method is used decrease a score when there wrong match
     public static void decScore()
     {
          score -= 40; 
     }

}
