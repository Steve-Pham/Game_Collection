using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SpaceMenu : MonoBehaviour
{
     //Cavnas variables to hide different canvas', depending what was clicked
     public Canvas MenuCanvas, HistoryCanvas; 

     // This the drop down menu where it stores the history
     public Dropdown HistoryDropdown;

     // This the logs of games
     public static List<GameLog> logs = new List<GameLog>();

     // Start is called before the first frame update
     // This is where the history of users are loaded from the json file
     void Start()
    {
          string path = Application.streamingAssetsPath + "/spaceLogs.json";
          string jsonString = File.ReadAllText(path);
          gameDataShooter data = JsonUtility.FromJson<gameDataShooter>(jsonString);
          logs.Clear();
          if (!(data == null))
          {
               foreach (GameLog log in data.ShooterGame)
               {
                    logs.Add(log);
               }
          }
     }

    // Update is called once per frame
    void Update()
    {
        
    }

     // This is when the history menu is clicked 
     public void HistoryClick()
     {

          FileMenu.canHistory.Push(MenuCanvas);
          HideCanvas();

          HistoryDropdown.ClearOptions();
          LoadHistoryDropdown();

          HistoryCanvas.gameObject.SetActive(true);
          HistoryDropdown.Show();
     }

     // This is ued when to hide the canvas' 
     public void HideCanvas()
     {
          MenuCanvas.gameObject.SetActive(false);
          HistoryCanvas.gameObject.SetActive(false);
     }

     // This is used to load the history into the drop down menu to view
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

     // This is used to go back to the menu from the history menu
     public void HistoryBackClick()
     {

          HideCanvas();
          FileMenu.canHistory.Pop().gameObject.SetActive(true);

     }

     // This function is used to save the game data 
     public static void SaveGameData()
     {
          gameDataShooter newData = new gameDataShooter();
          string path = Application.streamingAssetsPath + "/spaceLogs.json";
          foreach (GameLog log in logs)
          {
               newData.Add(log);
          }
          //Debug.Log(JsonUtility.ToJson(newData));
          File.WriteAllText(path, JsonUtility.ToJson(newData));
     }
}

// This class is used to store and add logs to the history
[System.Serializable]
public class gameDataShooter
{
     public List<GameLog> ShooterGame = new List<GameLog>();

     public void Add(GameLog log)
     {
          ShooterGame.Add(log);
     }
}
