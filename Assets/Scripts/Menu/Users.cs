using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Users : MonoBehaviour
{

     public static Dictionary<string, User> users = new Dictionary<string, User>();

     public static User useratm = null; // Keep track of the current user 

     public static float useratmStartTime;

     // Start is called before the first frame update
     void Start()
     {
          if (useratm == null)
          {
               // Get file path to data stored in the JSON file
               string filepath = Application.streamingAssetsPath + "/userData.json";

               // read info of the json file
               string jsonString = File.ReadAllText(filepath);

               // store json data into the userData list
               userData data = JsonUtility.FromJson<userData>(jsonString);

               // Get all the users from the json file and get their 
               foreach (User user in data.Users)
               {
                    users.Add(user.Username, user); // add user to the list
                    //Debug.Log(user.Username);
                    foreach (SessionLog session in user.logins)
                    {
                         //Debug.Log("Length: " + session.Lengthofsess + "\t Time: " + session.Timeofsess);
                    }
               }
               dumpTKL();
          }
     }

     // Obtain the user info 
     public static User getUser(string username)
     {
          return users[username];
     }

     // Add user info to the json file
     public static void addUser(string username)
     {
          users.Add(username, new User(username));
          dumpTKL();
     }

     // Delete user info
     public static void deleteUser(string username)
     {
          users.Remove(username);
          dumpTKL();
     }

     // See if json file has the specified user
     public static Boolean containUser(string username)
     {
          return users.ContainsKey(username);
     }

     // Release the user 
     public static void releaseUser(string username)
     {
          users[username].Status = "normal";
          dumpTKL();
     }

     // Block the user if they log in too much
     public static void blockUser(string username)
     {
          users[username].Status = "blocked";
          dumpTKL();
     }

     // Change password
     public static void changePassword(string npass)
     {
          if (useratm != null)
          {
               users[useratm.Username].Password = npass;
          }
          dumpTKL();
     }

     // This function is to modify or add users to the JSON file
     public static void dumpTKL()
     {
          userData nUsers = new userData();
          // File path to user data
          string filepath = Application.streamingAssetsPath + "/userData.json";

          // Add user modifications or new users to the JSON file
          foreach (User user in users.Values)
          {
               nUsers.Add(user);
          }
          //Debug.Log(JsonUtility.ToJson(nUsers));
          File.WriteAllText(filepath, JsonUtility.ToJson(nUsers));
     }

     // Get the users
     public static Dictionary<string, User> getUsers()
     {
          return users;
     }

     // Get the current user 
     public static User currentUser
     {
          get
          {
               return useratm;
          }
          set
          {
               useratm = value;
          }
     }
}


[System.Serializable]
public class SessionLog
{
     public string Lengthofsess; // This variable keeps track of the time length of the session

     public string Timeofsess; // This variable keeps track of when the user logged


     // Default constructor
     public SessionLog()
     {
          this.Lengthofsess = System.String.Format("{0:0.00}s", UnityEngine.Time.time - Users.useratmStartTime);
          this.Timeofsess = System.DateTime.Now.ToString(); // get current time
     }
}


[System.Serializable]
public class userData
{
     // Keep list of the user data
     public List<User> Users = new List<User>();

     // Add user to the current list, override List add method
     public void Add(User userinfo)
     {
          Users.Add(userinfo);
     }
}




