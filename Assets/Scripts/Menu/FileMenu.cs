using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileMenu : MonoBehaviour
{
     // ALL THE CANVAS' IN THE FILE MENU
     public Canvas FileMenuCanvas, GameMenuCanvas, AccountsCanvas, CreateUserCanvas;
     public Canvas DeleteUserCanvas, ChangePasswordCanvas, ReleaseBlockCanvas, ConfigurationCanvas, HistoryCanvas;

     // Change Password field menu
     public InputField changePasswordfield;
     public Text PasswordStatus;

     // Create user field menu
     public InputField createUserField;
     public Text createUserStatus;

     // Delete user list menu
     public Dropdown uList;
     public Text deleteUserStatus;

     // Blocked people list
     public Dropdown blockList;
     public Text releaseBlockStatus;

     // Configuration Settings menu
     public Sprite[] backgroundImages;
     public Image currentBackground;
     public Dropdown BackgroundDropdown;
     public Dropdown AudioDropDown;
     public Dropdown AudioVolumeDropDown;
     public AudioClip[] BackgroundMusic;
     public AudioSource currentSong;
     public Image previewImage;

     // History Menu
     public Dropdown HistoryDropDown;

     // Need these buttons for admin menu
     public Button createUserButton;
     public Button deleteUserButton;
     public Button releaseBlocksButton;

     // These are the volumes 
     private float[] Volumes = { 1.0f, 0.5f, 0.1f }; // volume of audio


     // This is used to keep track of the canvas' so we don't have to hard code many exit/back fucntions
     public static Stack<Canvas> canHistory = new Stack<Canvas>(); 

     // Play the default song
     void Start()
     {
          currentSong.Play();
     }

     // THis is to check drop down menus for background and songs
     void Update()
     {
          previewImage.sprite = backgroundImages[BackgroundDropdown.value];
          
          // If current background image is not the same preview
          if (!backgroundImages[BackgroundDropdown.value].Equals(previewImage.sprite))
          {
               previewImage.sprite = backgroundImages[BackgroundDropdown.value];
          }

          // If current audio is not the same as drop down
          AudioClip tmp = currentSong.clip;

          currentSong.clip = BackgroundMusic[AudioDropDown.value];
          currentSong.volume = Volumes[AudioVolumeDropDown.value];

          if (tmp != currentSong.clip)
               currentSong.Play();
     }

     // This for the account menu
     public void AccountMenu()
     {
          canHistory.Push(FileMenuCanvas); // push FileMenucanvas into the stack
          InvisibleCanvas();
          AccountsCanvas.gameObject.SetActive(true);

          // Check if admin is logged, if he is, then enable the create user, delete user, and release user buttons
          if ((Users.useratm != null) && !(Users.useratm.Username == "admin"))
          {
               // If the user is not an admin, disable create user, delete user, and release blocks
               createUserButton.gameObject.SetActive(false); 
               deleteUserButton.gameObject.SetActive(false);
               releaseBlocksButton.gameObject.SetActive(false);
}
     }

     // Change password menu is enabled by click of a button
     public void changePasswordMenu()
     {
          canHistory.Push(AccountsCanvas);
          InvisibleCanvas();
          ChangePasswordCanvas.gameObject.SetActive(true);
     }

     // Create user menu is enbaled by a click of a button
     public void createUserMenu()
     {
          canHistory.Push(AccountsCanvas);
          InvisibleCanvas();
          CreateUserCanvas.gameObject.SetActive(true);
     }

     // Delete user menu is enabled by click of a button
     public void deleteUserMenu()
     {
          canHistory.Push(AccountsCanvas);
          InvisibleCanvas();
          DeleteUserCanvas.gameObject.SetActive(true);

          // Clear current options and get the current users stored in the users list
          uList.options.Clear();
          foreach (User user in Users.getUsers().Values)
          {
               uList.options.Add(new Dropdown.OptionData()
               {
                    text = user.Username
               });
          }

     }

     // Release blocks menu is enabled by a click of a button
     // This menu is to unblock user that has invalid logins
     public void releaseBlocksMenu()
     {
          canHistory.Push(AccountsCanvas);
          InvisibleCanvas();
          ReleaseBlockCanvas.gameObject.SetActive(true);

          blockList.options.Clear();
          foreach (User user in Users.getUsers().Values)
          {
               if (user.Status == "blocked")
               {
                    blockList.options.Add(new Dropdown.OptionData()
                    {
                         text = user.Username
                    });
               }
          }

     }

     // Enable the history menu
     public void HistoryMenu()
     {
          canHistory.Push(FileMenuCanvas);
          InvisibleCanvas();

          HistoryDropDown.ClearOptions();

          // If user is admin, open admin history, else open the user history
          if (Users.useratm == null || Users.useratm.Username == "admin")
          {
               AdminHistory();
          }
          else
          {
               UserHistory();
          }

          HistoryCanvas.gameObject.SetActive(true);
          HistoryDropDown.Show();
     }

     // This is to show the user history through the use of a drop down menu
     public void UserHistory()
     {
          string titles = string.Format("{0,-20}{1,-30}{2,-20}", "Username", "Date", "Duration");
          HistoryDropDown.options.Add(new Dropdown.OptionData()
          {
               text = titles
          });

          foreach (SessionLog session in Users.useratm.logins)
          {
               string Log = string.Format("{0,-20}{1,-30}{2,-20}", Users.useratm.Username, session.Timeofsess, session.Lengthofsess);
               HistoryDropDown.options.Add(new Dropdown.OptionData()
               {
                    text = Log
               });
          }

     }

     // This is to show the admin history and user history through the use of a drop down
     public void AdminHistory()
     {
          string title = string.Format("{0,-20}{1,-30}{2,-20}{3,-20}", "Username", "Date", "Duration", "Status");
          HistoryDropDown.options.Add(new Dropdown.OptionData()
          {
               text = title
          });

          foreach (User user in Users.getUsers().Values)
          {
               foreach (SessionLog session in user.logins)
               {
                    string Log = string.Format("{0,-20}{1,-30}{2,-20}{3,-20}", user.username, session.Timeofsess, session.Lengthofsess, user.Status);
                    HistoryDropDown.options.Add(new Dropdown.OptionData()
                    {
                         text = Log
                    });
               }
          }
     }

     // This is the changepass button for a user or admin to change the password 
     public void changePass()
     {
          // If the password field is not empty, then change the password
          if (changePasswordfield.text != "")
          {
               Users.changePassword(changePasswordfield.text);
               PasswordStatus.text = "Password Changed!";
          }
          else
          {
               PasswordStatus.text = "Password is invalid!";
          }
     }

     // This is the create user button to sucessfully create a user
     public void createUser()
     {
          if (Users.containUser(createUserField.text))
          {
               createUserStatus.text = "User exists!";
          }
          else if (createUserField.text == "")
          {
               createUserStatus.text = "Username must have atleast one character!";
          }
          else
          {
               Users.addUser(createUserField.text);
               createUserStatus.text = "User was successfully created with password as password";
          }
     }

     // This is to successfully delete a user using a button
     public void deleteUser()
     {
          string listeduser = uList.options[uList.value].text;
          if (listeduser == "admin")
          {
               deleteUserStatus.text = "You can't delete the admin!";
          }
          else
          {
               Users.deleteUser(listeduser);
               deleteUserStatus.text = "Deleted the user: " + listeduser;
               uList.options.Remove(uList.options[uList.value]);
               uList.value = 0;
          }
     }

     // Change the background button so user can preview button before hand
     public void ChangeBackgroundButton()
     {
          currentBackground.sprite = backgroundImages[BackgroundDropdown.value];
     }

     // This is for the config menu button to open this menu
     public void ConfigMenu()
     {
          canHistory.Push(FileMenuCanvas);
          InvisibleCanvas();
          ConfigurationCanvas.gameObject.SetActive(true);
     }

     // This is to successfully release a user
     public void releaseBlocks()
     {
          Users.releaseUser(blockList.options[blockList.value].text);
          releaseBlockStatus.text = "User is unblocked!";
          blockList.options.Remove(blockList.options[blockList.value]);
          blockList.value = 0;
     }

     // Back button for all menus
     public void BackButton()
     {
          InvisibleCanvas();
          canHistory.Pop().gameObject.SetActive(true);
     }


     // Hide all canvas' when necessary
     public void InvisibleCanvas()
     {
          FileMenuCanvas.gameObject.SetActive(false);
          GameMenuCanvas.gameObject.SetActive(false);
          AccountsCanvas.gameObject.SetActive(false);
          CreateUserCanvas.gameObject.SetActive(false);
          DeleteUserCanvas.gameObject.SetActive(false);
          ReleaseBlockCanvas.gameObject.SetActive(false);
          ChangePasswordCanvas.gameObject.SetActive(false);
          ConfigurationCanvas.gameObject.SetActive(false);
          HistoryCanvas.gameObject.SetActive(false);
     
     }









}
