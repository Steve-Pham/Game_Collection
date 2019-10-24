using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameLog
{
     // These are common instance variables that need to be shared amongst games
     public string Username;
     public string Date;
     public string Score;
     public string HighestLevel; 

     // Simple overloaded constructors
     public GameLog(string u, string d, string s, string h)
     {
          this.Username = u;
          this.Date = d;
          this.Score = s;
          this.HighestLevel = h;
     }
}