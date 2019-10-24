using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
     public static List<GameLog> logs = new List<GameLog>();
     public Canvas HistoryCanvas, MenuCanvas;

     public Dropdown HistoryDropdown;

     // Start is called before the first frame update
     // This is just for obtaining existing records
     void Start()
     {
          string path = Application.streamingAssetsPath + "/memoryLogs.json";
          string jsonString = File.ReadAllText(path);
          gameDataMemory data = JsonUtility.FromJson<gameDataMemory>(jsonString);
          logs.Clear();
          if (!(data == null))
          {
               foreach (GameLog log in data.MemoryGame)
               {
                    logs.Add(log);
               }
          }
     }



     // This function is used to determine what the buttons do
     // If manager == 0, then load the game (start)
     // If manager == 1, then quit the game (end)
     public void MenuButtons(int manager)
     {
          if (manager == 0)
          {
               SceneManager.LoadScene("Game");
          }
          else if (manager == 1)
          {
               Application.Quit();
          }
          else
          {
               Application.Quit();
          }
     }

     // This for the back button
     public void BackButton()
     {
          if (MenuCanvas.gameObject.activeInHierarchy)
          {
               HideCanvas();
               SceneManager.LoadScene("FileMenu");
          }
          else
          {
               HideCanvas();
               FileMenu.canHistory.Pop().gameObject.SetActive(true);
          }

     }

     // This is the History Menu button
     public void HistoryMenu()
     {
          FileMenu.canHistory.Push(MenuCanvas);
          HideCanvas();

          HistoryDropdown.ClearOptions();
          LoadHistoryDropdown();

          HistoryCanvas.gameObject.SetActive(true);
          HistoryDropdown.Show();
     }

     // This is to hide all the canvas'
     public void HideCanvas()
     {
          MenuCanvas.gameObject.SetActive(false);
          HistoryCanvas.gameObject.SetActive(false);
     }

     // Load the history drop down menu
     public void LoadHistoryDropdown()
     {
          HistoryDropdown.ClearOptions();
          string header = string.Format("{0,-20}{1,-30}{2,-20}", "Username", "Date", "Score");
          HistoryDropdown.options.Add(new Dropdown.OptionData() { text = header });

          foreach (GameLog log in logs)
          {
               string currentLog = string.Format("{0,-20}{1,-30}{2,-20}", log.Username, log.Date, log.Score);
               HistoryDropdown.options.Add(new Dropdown.OptionData() { text = currentLog });
          }
     }

     // This is to save the game data 
     public static void SaveGameData()
     {
          gameDataMemory newData = new gameDataMemory();
          string path = Application.streamingAssetsPath + "/memoryLogs.json";
          foreach (GameLog log in logs)
          {
               newData.Add(log);
          }
          Debug.Log(JsonUtility.ToJson(newData));
          File.WriteAllText(path, JsonUtility.ToJson(newData));
     }

}


// This is to add game logs to existing history
[System.Serializable]
public class gameDataMemory
{
     public List<GameLog> MemoryGame = new List<GameLog>();

     public void Add(GameLog log)
     {
          MemoryGame.Add(log);
     }
}
