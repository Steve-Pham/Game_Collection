using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
     public Canvas FileMenuCanvas, GameMenuCanvas; // Keep track of Game Menu Frames and File frames

     // This is the function when the Space Shooter Button to play the game
     public void PlaySpaceButton()
     {
          FileMenu.canHistory.Push(GameMenuCanvas);
          SceneManager.LoadScene("SpaceMenu");
     }

     // This is the function when the Apple Button to play the game
     public void PlayAppleButton()
     {
          FileMenu.canHistory.Push(GameMenuCanvas);
          SceneManager.LoadScene("AppleMenu");
     }

     // This is the function when the Memory Button to play the game
     public void PlayMemoryButton()
     {
          FileMenu.canHistory.Push(GameMenuCanvas);
          SceneManager.LoadScene("MainMenuCard");
     }

     // This is the function when the Rock Paper Scissors Button to play the game
     public void PlayRPSButton()
     {
          FileMenu.canHistory.Push(GameMenuCanvas);
          SceneManager.LoadScene("RPSscene");
     }

     // This is whe nthe file menu is opened to create users, delete users, etc...
     public void FileMenuButton()
     {
          FileMenu.canHistory.Push(GameMenuCanvas);
          GameMenuCanvas.gameObject.SetActive(false);
          FileMenuCanvas.gameObject.SetActive(true);
     }

     // This is the exit button to quit the whole game
     public void  ExitButton()
     {
          Users.useratm.logins.Add(new SessionLog());
          Users.dumpTKL();
          Application.Quit();
     }
}
