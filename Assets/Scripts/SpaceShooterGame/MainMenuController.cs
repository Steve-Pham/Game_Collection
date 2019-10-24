using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{

     // THESE ARE THE GAME OBJECT VARIABLES USED FOR DROP DOWN MENUS, INPUT FIELDS, AND MANAGING THE CANVAS'
     public GameObject TitleCanvas, MainCanvas, HistoryCanvas, GameLevels;
     public GameObject ConfigButtons, GameLevelsButtons, BronzeScreen, SilverScreen, GoldScreen;
     public GameObject Configuration, ConfigurationAudio, ConfigurationEnemy, ConfigurationBackground;
     public Dropdown BackgroundMusicDropdown, WinningMusicDropdown, ShootingSoundDropdown, DestroySoundDropdown;
     public Dropdown BackgroundMusicVolumeDropdown, WinningMusicVolumeDropdown, ShootingSoundVolumeDropdown, DestroySoundVolumeDropdown;
     public Dropdown BackgroundDropDown;

     private float[] Volumes = { 1.0f, 0.5f, 0.1f }; // volume of audio

     public Sprite[] backgrounds; // Different backgrounds
     public Image currentBackground; // Keep

     public AudioClip[] BackgroundMusics = new AudioClip[3];
     //public AudioSource music; 

     // This is for the bronze screen
     public Image[] b_enemies;
     public InputField b_scoreInput;
     public InputField b_maxEnemy;

     // This is for the silver screen
     public Image[] s_enemies;
     public InputField s_scoreInput;
     public InputField s_maxEnemy;

     // This is for the gold screen
     public Image[] g_enemies;
     public InputField g_scoreInput;
     public InputField g_maxEnemy;

     // Create instances for each level => Bronze, Silver, Gold
     private Level bronze = new Level("bronze", 100, 5);
     private Level silver = new Level("silver", 200, 10);
     private Level gold = new Level("gold", 300, 15);


     /* CONFIG */
     // Keep track of enemy colors and enemy scores 
     public InputField[] EnemyScores;
     public Dropdown[] EnemyColors;
    

     // This is the audio for the Game => Background, Shooting, Winning,  Destroy
     public AudioSource BackgroundMusic;
     public static int WinningSound;
     public static int ShootingSound;
     public static int DestroySound;

     public static int WinningSoundVolume;
     public static int ShootingSoundVolume;
     public static int DestroySoundVolume;

     public static int BackgroundSound;
     public static int BackgroundSoundVolume;

     private Settings settings = new Settings();


     // Enemy Colors
     public Dropdown e0color, e1color, e2color, e3color, e4color;

     public static int e0col, e1col, e2col, e3col, e4col;

     public static int[] enemyColors = { e0col, e1col, e2col, e3col, e4col };

     // Level Background

     public Dropdown bronzeBgdropdown, silverBgdropdown, goldBgdropdown;
     public static int bronzeBg, silverBg, goldBg;



     // Start is called before the first frame update
     void Start()
    {
          // Add default enemies 
          bronze.addEnemy(0);
          silver.addEnemy(0);
          silver.addEnemy(1);
          gold.addEnemy(0);
          gold.addEnemy(1);
          gold.addEnemy(2);

          // Set Default scores to enemies
          // Set Default colors to enemies
          for (int i = 0; i < 5; i++)
          {
               EnemyScores[i].text = "40";
               EnemyColors[i].value = 0;
          }

          // Set audio 
          BackgroundMusicDropdown.value = 0;
          WinningMusicDropdown.value = 0;
          ShootingSoundDropdown.value = 0;
          DestroySoundDropdown.value = 0;

          // Default background
          BackgroundDropDown.value = 0;

          // Set colours 
          e0color.value = 0;
          e1color.value = 0;
          e2color.value = 0;
          e3color.value = 0;
          e4color.value = 0;

          bronzeBgdropdown.value = 0;
          silverBgdropdown.value = 0;
          goldBgdropdown.value = 0;
     }

    // Update is called once per frame
    void Update()
    {
          // This is to change the background music
          AudioClip tmp = BackgroundMusic.clip;
          BackgroundMusic.clip = BackgroundMusics[BackgroundMusicDropdown.value];
          BackgroundMusic.volume = Volumes[BackgroundMusicVolumeDropdown.value];
          if (tmp != BackgroundMusic.clip)
               BackgroundMusic.Play();

          // Get values of the audio of Winning, Shooting, and Destroyed so it can be used in other scripts

          WinningSound = WinningMusicDropdown.value;
          WinningSoundVolume = WinningMusicVolumeDropdown.value;

          ShootingSound = ShootingSoundDropdown.value;
          ShootingSoundVolume = ShootingSoundVolumeDropdown.value;

          DestroySound = DestroySoundDropdown.value;
          DestroySoundVolume = DestroySoundVolumeDropdown.value;

          BackgroundSound = BackgroundMusicDropdown.value;
          BackgroundSoundVolume = BackgroundMusicVolumeDropdown.value;

          currentBackground.sprite = backgrounds[BackgroundDropDown.value];

          e0col = e0color.value;
          e1col = e1color.value;
          e2col = e2color.value;
          e3col = e3color.value;
          e4col = e4color.value;

          bronzeBg = bronzeBgdropdown.value;
          silverBg = silverBgdropdown.value;
          goldBg = goldBgdropdown.value;
     }

   


     //*****************
     //Title Buttons

     // Method to go from title to main menu screen
     public void TitleToMainMenu()
     {
          //music.Play();
          TitleCanvas.SetActive(false);
          MainCanvas.SetActive(true);
     }

     // Method to quit application
     public void EndGame()
     {
          SceneManager.LoadScene("FileMenu");
     }

     //*****************
     // Main Menu Buttons

     public void StartGame() 
     {
          //music.Play();
          settings.setLevels(bronze, silver, gold);
          settings.setScores(int.Parse(EnemyScores[0].text), int.Parse(EnemyScores[1].text), int.Parse(EnemyScores[2].text), int.Parse(EnemyScores[3].text), int.Parse(EnemyScores[4].text));
          settings.setColors(e0col, e1col, e2col, e3col, e4col);
          SceneManager.LoadScene("SpaceGame");
     }


     public void MainMenuToConfiguration()
     {
          //music.Play();
          MainCanvas.SetActive(false);
          Configuration.SetActive(true);
     }

     // Going from Main Menu to Title
     public void MainMenuToTitle()
     {
          //music.Play();
          MainCanvas.SetActive(false);
          TitleCanvas.SetActive(true);
     }

     public void MainMenuToGameLevels()
     {
          //music.Play();
          MainCanvas.SetActive(false);
          GameLevels.SetActive(true);
     }

     public void MainMenuToHistory()
     {
          //music.Play();
          MainCanvas.SetActive(false);
          HistoryCanvas.SetActive(true);
     }

     public void BackButtonConfig()
     {
          //music.Play();
          Configuration.SetActive(false);
          MainCanvas.SetActive(true);
     }

     public void BackButtonGameLevels()
     {
          //music.Play();
          GameLevels.SetActive(false);
          MainCanvas.SetActive(true);
     }

     public void BackButtonGameHistory()
     {
          //music.Play();
          HistoryCanvas.SetActive(false);
          MainCanvas.SetActive(true);
     }


     //*********
     // CONFIGURATION SETTINGS

     public void ConfigurationToConfigAudio()
     {
          //music.Play();
          ConfigButtons.SetActive(false);
          ConfigurationAudio.SetActive(true);
     }

     public void ConfigurationToConfigBackground()
     {
          //music.Play(); 
          ConfigButtons.SetActive(false);
          ConfigurationBackground.SetActive(true);
     }

     public void ConfigurationToConfigEnemies()
     {
          ConfigButtons.SetActive(false);
          ConfigurationEnemy.SetActive(true);
     }

     public void ConfigAudioToConfiguration()
     {
          //music.Play();
          ConfigurationAudio.SetActive(false);
          ConfigButtons.SetActive(true);
     }

     public void ConfigBackgroundToConfiguration()
     {
          //music.Play();
          ConfigurationBackground.SetActive(false);
          ConfigButtons.SetActive(true);
     }

     public void ConfigEnemyToConfiguration()
     {
          //music.Play();
          ConfigurationEnemy.SetActive(false);
          ConfigButtons.SetActive(true);
     }

     //**********
     // GAME LEVEL MENU

     public void GameLevelsToBronze ()
     {
          //music.Play(); 
          GameLevelsButtons.SetActive(false);
          startBronze();
          BronzeScreen.SetActive(true);

     }

     public void GameLevelsToSilver()
     {
          //music.Play(); 
          GameLevelsButtons.SetActive(false);
          startSilver();
          SilverScreen.SetActive(true);
     }

     public void GameLevelsToGold()
     {
          //music.Play(); 
          GameLevelsButtons.SetActive(false);
          startGold(); 
          GoldScreen.SetActive(true);
     }

     public void BronzeToGameLevels()
     {
          //music.Play();
          updateBronze();
          BronzeScreen.SetActive(false);
          GameLevelsButtons.SetActive(true);
     }

     public void SilverToGameLevels()
     {
          //music.Play();
          updateSilver();
          SilverScreen.SetActive(false);
          GameLevelsButtons.SetActive(true);
     }

     public void GoldToGameLevels()
     {
          //music.Play();
          updateGold();
          GoldScreen.SetActive(false);
          GameLevelsButtons.SetActive(true);
     }


     //********
     // BRONZE SETTINGS

     // Initialize Bronze Level settings
     private void startBronze()
     {
          // set enemies color
          for (int i = 0; i < 5; i++)
          {
               b_enemies[i].GetComponent<Image>().color = new Color32(186, 186, 186, 255);
          }
          foreach (int enemy in bronze.getEnemies())
          {
               b_enemies[enemy].GetComponent<Image>().color = new Color32(255, 100, 100, 255);
          }

          // set score
          b_scoreInput.text = bronze.getScoreValue().ToString();
          b_maxEnemy.text = bronze.getMaxEnemy().ToString();
     }

     // Update Bronze Level Settings
     private void updateBronze()
     {
          // set enemies color
          for (int i = 0; i < 5; i++)
          {
               b_enemies[i].GetComponent<Image>().color = new Color32(186, 186, 186, 255);
          }
          foreach (int enemy in bronze.getEnemies())
          {
               b_enemies[enemy].GetComponent<Image>().color = new Color32(255, 100, 100, 255);
          }


          // set high score and max enemies
          bronze.setTopScore(int.Parse(b_scoreInput.text));
          bronze.setMaxEnemies(int.Parse(b_maxEnemy.text)); 
     }


     // The following functions are TOGGLE functions when the user wants to play with different enemies
     public void Bronze_setEnemy0()
     {
          //music.Play();
          if (bronze.toggleEnemy(0) == true)
          {
               silver.addEnemy(0);
               gold.addEnemy(0);
          }
          updateBronze();
     }
     public void Bronze_setEnemy1()
     {
          //music.Play();
          if (bronze.toggleEnemy(1) == true)
          {
               silver.addEnemy(1);
               gold.addEnemy(1);
          }
          updateBronze();
     }
     public void Bronze_setEnemy2()
     {
          //music.Play();
          if (bronze.toggleEnemy(2) == true)
          {
               silver.addEnemy(2);
               gold.addEnemy(2);
          }
          updateBronze();
     }
     public void Bronze_setEnemy3()
     {
          //music.Play();
          if (bronze.toggleEnemy(3) == true)
          {
               silver.addEnemy(3);
               gold.addEnemy(3);
          }
          updateBronze();
     }
     public void Bronze_setEnemy4()
     {
          //music.Play();
          if (bronze.toggleEnemy(4) == true)
          {
               silver.addEnemy(4);
               gold.addEnemy(4);
          }
          updateBronze();
     }


     //********
     // SILVER MENU

     // This is to initialize the silver game menu
     private void startSilver()
     {
          // set enemies color
          for (int i = 0; i < 5; i++)
          {
               s_enemies[i].GetComponent<Image>().color = new Color32(186, 186, 186, 255);
          }
          foreach (int enemy in silver.getEnemies())
          {
               s_enemies[enemy].GetComponent<Image>().color = new Color32(255, 100, 100, 255);
          }

          // set top score and max amount of enemies on screen
          s_scoreInput.text = silver.getScoreValue().ToString();
          s_maxEnemy.text = silver.getMaxEnemy().ToString();
     }
     private void updateSilver()
     {
          // set enemies color
          for (int i = 0; i < 5; i++)
          {
               s_enemies[i].GetComponent<Image>().color = new Color32(186, 186, 186, 255);
          }
          foreach (int enemy in silver.getEnemies())
          {
               s_enemies[enemy].GetComponent<Image>().color = new Color32(255, 100, 100, 255);
          }

          // set score
          silver.setTopScore(int.Parse(s_scoreInput.text));
          silver.setMaxEnemies(int.Parse(s_maxEnemy.text));
     }


     // The following functions are TOGGLE functions when the user wants to play with different enemies
     public void Silver_setEnemy0()
     {
          //music.Play();
          if (silver.toggleEnemy(0) == true)
          {
               gold.addEnemy(0);
          }
          else
          {
               bronze.removeEnemy(0);
          }
          updateSilver();
     }
     public void Silver_setEnemy1()
     {
          //music.Play();
          if (silver.toggleEnemy(1) == true)
          {
               gold.addEnemy(1);
          }
          else
          {
               bronze.removeEnemy(1);
          }
          updateSilver();
     }
     public void Silver_setEnemy2()
     {
          //music.Play();
          if (silver.toggleEnemy(2) == true)
          {
               gold.addEnemy(2);
          }
          else
          {
               bronze.removeEnemy(2);
          }
          updateSilver();
     }
     public void Silver_setEnemy3()
     {
          //music.Play();
          if (silver.toggleEnemy(3) == true)
          {
               gold.addEnemy(3);
          }
          else
          {
               bronze.removeEnemy(3);
          }
          updateSilver();
     }
     public void Silver_setEnemy4()
     {
          //music.Play();
          if (silver.toggleEnemy(4) == true)
          {
               gold.addEnemy(4);
          }
          else
          {
               bronze.removeEnemy(4);
          }
          updateSilver();
     }

     //***********
     // GOLD SETTINGS

     // THIS IS TO INITALIZE THE GOLD LEVEL SETTINGS
     private void startGold()
     {
          // set enemies color
          for (int i = 0; i < 5; i++)
          {
               g_enemies[i].GetComponent<Image>().color = new Color32(186, 186, 186, 255);
          }
          foreach (int enemy in gold.getEnemies())
          {
               g_enemies[enemy].GetComponent<Image>().color = new Color32(255, 100, 100, 255);
          }

          // set high score and max amount of enemies on screen
          g_scoreInput.text = gold.getScoreValue().ToString();
          g_maxEnemy.text = gold.getMaxEnemy().ToString();
     }
    
     // UPDATE GOLD LEVEL SETTINGS 
     private void updateGold()
     {
          // set enemies color
          for (int i = 0; i < 5; i++)
          {
               g_enemies[i].GetComponent<Image>().color = new Color32(186, 186, 186, 255);
          }
          foreach (int enemy in gold.getEnemies())
          {
               g_enemies[enemy].GetComponent<Image>().color = new Color32(255, 100, 100, 255);
          }


          // set top score and max amount of enemies for gold level
          gold.setTopScore(int.Parse(g_scoreInput.text));
          gold.setMaxEnemies(int.Parse(g_maxEnemy.text));
     }

     // The following functions are TOGGLE functions when the user wants to play with different enemies
     public void Gold_setEnemy0()
     {
          //music.Play();
          if (gold.toggleEnemy(0) == false)
          {
               bronze.removeEnemy(0);
               silver.removeEnemy(0);
          }
          updateGold();
     }
     public void Gold_setEnemy1()
     {
          //music.Play();
          if (gold.toggleEnemy(1) == false)
          {
               bronze.removeEnemy(1);
               silver.removeEnemy(1);
          }
          updateGold();
     }
     public void Gold_setEnemy2()
     {
          //music.Play();
          if (gold.toggleEnemy(2) == false)
          {
               bronze.removeEnemy(2);
               silver.removeEnemy(2);
          }
          updateGold();
     }
     public void Gold_setEnemy3()
     {
          //music.Play();
          if (gold.toggleEnemy(3) == false)
          {
               bronze.removeEnemy(3);
               silver.removeEnemy(3);
          }
          updateGold();
     }
     public void Gold_setEnemy4()
     {
          //music.Play();
          if (gold.toggleEnemy(4) == false)
          {
               bronze.removeEnemy(4);
               silver.removeEnemy(4);
          }
          updateGold();
     }



}
