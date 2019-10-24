using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
     // Instance variables for obvious reasons
     public string Username;
     public string Password;
     public string Status;
     public int Score;

     // Keeps track of user logins time and length
     public List<SessionLog> logins = new List<SessionLog>();

     // Default constructors
     public User(string username)
     {
          this.Username = username;
          this.Password = "password"; // default password
          this.Status = "new";
          this.Score = 0;
     }

     // Overloaded constructor
     public User(string username, string pass, string status, int scre)
     {
          this.Username = username;
          this.Password = pass;
          this.Status = status;
          this.Score = scre;
     }

     // GETTER and SETTING methods 

     public string username
     {
          get
          {
               return this.Username;
          }

          set
          {
               this.Username = value;
          }
     }

     public string password
     {
          get
          {
               return this.Password;
          }
          set
          {
               this.Password = value;
          }
     }

     public string status
     {
          get
          {
               return this.Status;
          }
          set
          {
               this.Status = value;
          }
     }

     public int score
     {
          get
          {
               return this.Score;
          }
          set
          {
               this.Score = value;
          }
     }
}
