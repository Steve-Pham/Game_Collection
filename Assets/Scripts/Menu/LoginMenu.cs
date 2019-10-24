using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
     // Keep trackof the Canvas' 
     public Canvas LoginMenuCanvas, GameMenuCanvas, ChangePasswordCanvas;
     
     // Username and password fields
     public Text userField, passField; 

     // Notify the user of the status of the login!
     public Text status;

     // This is to count the number of login violations, if they exceed 3
     Dictionary<string, int> countViolations = new Dictionary<string, int>();

     // Start is called before the first frame update
     void Start()
     {
          // Check if user that is logged in is not false 
          if (Users.useratm != null)
          {
               LoginMenuCanvas.gameObject.SetActive(false);
               GameMenuCanvas.gameObject.SetActive(true);
          }

     }

     // Check Login of the user (if they are blocked, or password is wrong, or too many login attempts!)
     public void CheckLogin()
     {
          // Check if user input field has text
          if (Users.containUser(userField.text))
          {
               User userattempt = Users.getUser(userField.text);
               // If user is blocked, notify them !
               if (userattempt.Status == "blocked")
               {
                    status.text = "User is blocked atm!";
               }
               // If user password is correct 
               else if (userattempt.Password == passField.text)
               {
                    Users.useratm = Users.getUser(userField.text);
                    Users.useratmStartTime = Time.time;
                    LoginMenuCanvas.gameObject.SetActive(false);
                    // If user status is "new", then change the password!
                    if (Users.useratm.Status == "new")
                    {
                         ChangePasswordCanvas.gameObject.SetActive(true);
                         //text
                         Users.useratm.Status = "normal";
                         FileMenu.canHistory.Push(GameMenuCanvas);
                    }
                    else
                    {
                         GameMenuCanvas.gameObject.SetActive(true);
                    }
               }
               else
               {
                    if (userField.text != "admin")
                    {
                         int attemptsleft = addVio(userField.text);
                         // if login attempts exceed too many attempts 
                         if (attemptsleft == 0)
                         {
                              status.text = "Too many attempts!";
                              Users.blockUser(userattempt.Username);
                         }
                         // Invalid password
                         else
                         {
                              status.text = "Invalid password!, User will be blocked after " + attemptsleft + " attempts!";
                         }
                    }
                    else
                    {
                         status.text = "Invalid password!";
                    }
               }
          }
          else
          {
               status.text = "This user does not exist!";
          }
     }
     // This function is to add violations as there is wrong password attempts
     public int addVio(string user)
     {
          if (countViolations.ContainsKey(user))
          {
               countViolations[user]--;
          }
          else
          {
               countViolations[user] = 2;
          }
          return countViolations[user];
     }

     // This fucntion to end the game 
     public void Endgame()
     {
          Application.Quit();
     }
}
