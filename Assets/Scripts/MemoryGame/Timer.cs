using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
   
     // Instance variables used to calculate the total time elasped
     public static float time1;
     public static float time2;

     // This method is used to calculate the total time elasped within the game from start to finish
     public static float totalTime()
     {
          return time2 - time1;
     }

  
}
