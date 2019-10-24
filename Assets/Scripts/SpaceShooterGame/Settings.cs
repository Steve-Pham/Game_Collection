using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{

     private static Level[] lvls = new Level[3];
     private static int[] eScores = new int[5];
     private static int[] colors = new int[5];

 
     // This is used to keep the info on the levels from the start menu
     public void setLevels(Level bronze, Level silver, Level gold)
     {
          lvls[0] = bronze;
          lvls[1] = silver;
          lvls[2] = gold;
     }

     // Function to determine the current level
     public static Level getLevel(int i)
     {
          return lvls[i];
     }

     // Function to obtain score per kill on enemies
     public void setScores(int one, int two, int three, int four, int five)
     {
          eScores[0] = one;
          eScores[1] = two;
          eScores[2] = three;
          eScores[3] = four;
          eScores[4] = five;
     }

     // Function to set color of enemies
     public void setColors(int one, int two, int three, int four, int five)
     {
          colors[0] = one;
          colors[1] = two;
          colors[2] = three;
          colors[3] = four;
          colors[4] = five;
     }

     // Function to get color of enemy
     public static int getColor(int i)
     {
          return colors[i];
     }


     //Functino to get score of enemy
     public static int getScore(int i)
     {
          return eScores[i];
     }
}
