using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
     // Keep track of canvas'
     public Canvas MenuCanvas, GameCanvas, HistoryCanvas;
     public Dropdown HisDropdown; 


     private int turn = 0; // 0 for player, 1 for ai

     // This are nums for rock, paper, scissors
     private const int rock = 1;
     private const int paper = 2;
     private const int scissors = 3;

     // Keep track of player and AI voices
     private int playerchoice;
     private int AIchoice;

     // Keep track of player and AI scores
     private int playerScore = 0;
     private int AIScore = 0; 

     // These are the text for the ingame
     public GameObject playerScoretext;
     public GameObject AIScoreText;
     public GameObject RoundWinnerText;
     public GameObject CurrentWinnerText;
     public GameObject RoundResultText;
     public GameObject AIChoiceText;
     public GameObject WinnerText; 

     // Keep track of rounds
     private int round = 1;

     // Keep records of who played the game
     public static List<GameLog> logs = new List<GameLog>();


     // 
     void Start()
     {
          RoundWinnerText.GetComponent<Text>().text = "Round: " + round;
          playerScoretext.GetComponent<Text>().text = "Player Score: " + playerScore;
          AIScoreText.GetComponent<Text>().text = "Computer Score: " + AIScore;
          CurrentWinnerText.GetComponent<Text>().text = "Current Winner: Tied";
          RoundResultText.GetComponent<Text>().text = "Starting Round";
          AIChoiceText.GetComponent<Text>().text = "Game Not Started";
          WinnerText.GetComponent<Text>().text = " ";

          // Get the exisiting records and store them into the logs array
          string path = Application.streamingAssetsPath + "/rpsLogs.json";

          string jsonString = File.ReadAllText(path);
          gameDataRPS data = JsonUtility.FromJson<gameDataRPS>(jsonString);
          logs.Clear();
          if (!(data == null))
          {
               foreach (GameLog log in data.RockPaperScissors)
               {
                    logs.Add(log);
               }
          }

     }

     // This is to start the game from the main menu
     public void StartGame()
     {
          FileMenu.canHistory.Push(MenuCanvas);
          MenuCanvas.gameObject.SetActive(false);
          GameCanvas.gameObject.SetActive(true);
          startOver();
     }


    // Update is called once per frame
    void Update()
    {
          // Get the winner when the round exceeds ten
          if (round > 10)
          {
               string winner = checkWinner();
               if (Users.useratm == null)
               {
                    logs.Add(new GameLog("admin", System.DateTime.Now.ToString(), winner, "n/a"));
               }
               else
               {
                    logs.Add(new GameLog(Users.useratm.username, System.DateTime.Now.ToString(), winner, "n/a"));
               }
               startOver();
               GameCanvas.gameObject.SetActive(false);
               SaveGameData();
               MenuCanvas.gameObject.SetActive(true);

          }

          // If round is under 10, then keep playing
          if (round <= 10)
          {

               if (turn == 0)
               {
                    return;
               }
               AITurn();
               checkRoundWin();
               turn = 0;
               round++;

               if (round <= 10) RoundWinnerText.GetComponent<Text>().text = "Round: " + round;
   
               playerScoretext.GetComponent<Text>().text = "Player Score: " + playerScore;
               AIScoreText.GetComponent<Text>().text = "Computer Score: " + AIScore;
               currentWin();
          }  

     }

     // This is to determine whose turn 
     public void PlayersTurn(int choice)
     {
          playerchoice = choice; 
          turn = 1; // bots turn
     }

     // This is the AI's turn to choose
     // This function determines what the AI picks
     public void AITurn()
     {
          AIchoice = Random.Range(1, 4);

          if (AIchoice == 1) AIChoiceText.GetComponent<Text>().text = "Computer picked Rock";
          else if (AIchoice == 2) AIChoiceText.GetComponent<Text>().text = "Computer picked Paper";
          else AIChoiceText.GetComponent<Text>().text = "Computer picked Scissor";
     }

     // This is to check who the round
     public void checkRoundWin()
     {
          if (playerchoice == AIchoice)
          {
               // draw
               RoundResultText.GetComponent<Text>().text = "Tied";
               return; 
          }
          else if ((playerchoice == rock && AIchoice == scissors) || (playerchoice == paper && AIchoice == rock) || (playerchoice == scissors && AIchoice == paper))
          {
               // player wins the round
               playerScore++;
               RoundResultText.GetComponent<Text>().text = "Player Won this Round";
          }
          else
          {    
               // computer wins the round
               AIScore++;
               RoundResultText.GetComponent<Text>().text = "Computer Won this Round";
          }
     }

     // This is change the text to the current winner
     public void currentWin()
     {
          if (playerScore == AIScore)
          {
               CurrentWinnerText.GetComponent<Text>().text = "Current Winner: Tied";
          }
          else if (playerScore > AIScore) 
          {
               CurrentWinnerText.GetComponent<Text>().text = "Current Winner: Player";
          }
          else
          {
               CurrentWinnerText.GetComponent<Text>().text = "Current Winner: AI";
          }
     }
     
     // This is to check the winner after the ten rounds are up
     public string checkWinner()
     {
          string winner; 

          if (playerScore == AIScore)
          {
               WinnerText.GetComponent<Text>().text = "Tied!";
               winner = "Tie";
          }
          else if (playerScore > AIScore)
          {
               WinnerText.GetComponent<Text>().text = "Winner: You";
               winner = "Player";
          }
          else
          {
               WinnerText.GetComponent<Text>().text = "Winner: AI";
               winner = "Bot";
          }

          return winner;

     }


     // This is to restart the game
     public void startOver()
     {
          playerScore = 0;
          AIScore = 0;
          round = 1;
          RoundWinnerText.GetComponent<Text>().text = "Round: " + round;
          playerScoretext.GetComponent<Text>().text = "Player Score: " + playerScore;
          AIScoreText.GetComponent<Text>().text = "Computer Score: " + AIScore;
          RoundResultText.GetComponent<Text>().text = "Round Result";
          CurrentWinnerText.GetComponent<Text>().text = "Current Winner";
          AIChoiceText.GetComponent<Text>().text = "AI Choice";
     }

     // This function is used to save the game data
     public void SaveGameData()
     {
          gameDataRPS newData = new gameDataRPS();
          string path = Application.streamingAssetsPath + "/rpsLogs.json";
          foreach (GameLog log in logs)
          {
               newData.Add(log);
          }
          Debug.Log(JsonUtility.ToJson(newData));
          File.WriteAllText(path, JsonUtility.ToJson(newData));
     }
     
     // This is used to open the history menu
     public void HistoryMenu()
     {
          FileMenu.canHistory.Push(MenuCanvas);
          HideCanvas();

          HisDropdown.ClearOptions();
          LoadHistoryDropdown();

          HistoryCanvas.gameObject.SetActive(true);
          HisDropdown.Show();
     }

     // This function is used to store the history of users inside a drop down menu
     public void LoadHistoryDropdown()
     {
          HisDropdown.ClearOptions();
          string header = string.Format("{0,-20}{1,-30}{2,-20}", "Username", "Date", "Score");
          HisDropdown.options.Add(new Dropdown.OptionData() { text = header });

          foreach (GameLog log in logs)
          {
               string currentLog = string.Format("{0,-20}{1,-30}{2,-20}", log.Username, log.Date, log.Score);
               HisDropdown.options.Add(new Dropdown.OptionData() { text = currentLog });
          }
     }

     // This is the back button to either go back to main menu (with all games) or back to start of the game
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

     // This function is used to the hide canvas'
     public void HideCanvas()
     {
          MenuCanvas.gameObject.SetActive(false);
          HistoryCanvas.gameObject.SetActive(false);
          GameCanvas.gameObject.SetActive(false);
     }

}

// This function is used to store the game data of this game
[System.Serializable]
public class gameDataRPS
{
     public List<GameLog> RockPaperScissors = new List<GameLog>();

     public void Add(GameLog log)
     {
          RockPaperScissors.Add(log);
     }
}
