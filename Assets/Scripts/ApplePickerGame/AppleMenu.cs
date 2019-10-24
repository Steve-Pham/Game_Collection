using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class AppleMenu : MonoBehaviour
{
     // Keep track of menu canvas and history canvas
     public Canvas MenuCanvas, HistoryCanvas;

     public Dropdown HistoryDropdown;

     // This is to keep track of the locks
     public static List<GameLog> logs = new List<GameLog>();


     // Start is called before the first frame update
     // Open JSON file, and record existing records
     void Start()
    {
          string path = Application.streamingAssetsPath + "/appleLogs.json";
          string jsonString = File.ReadAllText(path);
          gameData data = JsonUtility.FromJson<gameData>(jsonString);
          logs.Clear();
          if (!(data == null))
          {
               foreach (GameLog log in data.ApplePicker)
               {
                    logs.Add(log);
               }
          }
     }

     // Get the logs and return them
     public static List<GameLog> GetLogs()
     {
          return logs;
     }

     // Start menu button
     public void startButton()
     {
          SceneManager.LoadScene("AppleGame");
     }

     // History Menu button
     public void HistoryMenu()
     {
          FileMenu.canHistory.Push(MenuCanvas);
          HideCanvas();

          HistoryDropdown.ClearOptions();
          LoadHistoryDropdown();

          HistoryCanvas.gameObject.SetActive(true);
          HistoryDropdown.Show();
     }

     // This is to load the history 
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


     // Back button to game menu or back to start menu of game
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

     // Simply hide all canvas' 
     public void HideCanvas()
     {
          MenuCanvas.gameObject.SetActive(false);
          HistoryCanvas.gameObject.SetActive(false);
     }
}

// This class is used to record the game data
[System.Serializable]
public class gameData
{
     public List<GameLog> ApplePicker = new List<GameLog>();

     public void Add(GameLog log)
     {
          ApplePicker.Add(log);
     }
}
