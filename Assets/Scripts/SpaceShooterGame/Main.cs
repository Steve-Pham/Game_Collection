using System.Collections; // Required for Arrays & otherCollections
using System.Collections.Generic; // Required to use Lists orDictionaries
using UnityEngine; // Required for Unity
using UnityEngine.SceneManagement; // For loading & reloading ofscenes
using UnityEngine.UI;


public class Main : MonoBehaviour
{
     static public Main S; // Asingleton for Main
     static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
     [Header("Set in Inspector")]
     public GameObject[] prefabEnemies; // Array of Enemy prefabs
     public float enemySpawnPerSecond = 0.5f; // # Enemies/second
     public float enemyDefaultPadding = 1.5f; // Padding for position


     // THESE GAME OBJECTS are used to for the restart/pause/end game buttons, and keeping of track of scores and times

     public GameObject pauseButton;
     public GameObject levelStatus;
     public GameObject scoreText;
     public GameObject pausedText;
     public GameObject e0Score, e1Score, e2Score, e3Score, e4Score;
     public GameObject timeText;
     public GameObject restartButton;
     public GameObject exitButton;

     public static int score = 0;
     public static int lvl = BRONZE;
     private Level currLvl = Settings.getLevel(BRONZE);

     const int BRONZE = 0;
     const int SILVER = 1;
     const int GOLD = 2;

     private float startTime;

     int[] enemyScores = { 0, 0, 0, 0, 0 };

     public Color red = Color.red;
     public Color green = Color.green;
     public Color blue = Color.blue;

     public static int enemiesScreen = 0; // Track maximum amount of enemies on screen



     // AUDIO CLIPS AND SOUNDS
     public AudioClip[] BackgroundMusics = new AudioClip[3];
     public AudioSource BackgroundMusic;

     private float[] Volumes = { 1.0f, 0.5f, 0.1f };
     public AudioClip[] DestroySounds = new AudioClip[3];
     public AudioSource DestroySound;

     public AudioClip[] WinningSounds = new AudioClip[3];
     public AudioSource WinningSound;


     public WeaponDefinition[] weaponDefinitions;

     public GameObject prefabPowerUp;
     // a
     public WeaponType[] powerUpFrequency = new
     WeaponType[] { WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield };

     private BoundsCheck bndCheck;


     // Different level backgrounds 
     public Sprite[] bronzebackgrounds; 
     public Sprite[] silverbackgrounds;
     public Sprite[] goldbackgrounds; 
     public Image currentBackground; // Keep




     public void shipDestroyed(Enemy e, int ss)
     {

          //Debug.Log("Ship Destroyed Screen: " + enemiesScreen);

          DestroySound.Play(); 

          // KEEP track of current score and enemy eliminations 
          score = score + ss;
          scoreText.GetComponent<Text>().text = "Score :" + score;
          CalculateEnemyElims(e);


          // Potentially generate a PowerUp
          if (Random.value <= e.powerUpDropChance)
          { // d
            // Choose which PowerUp to pick
            // Pick one from the possibilities in powerUpFrequency
               int ndx =
               Random.Range(0, powerUpFrequency.Length); // e
               WeaponType puType = powerUpFrequency[ndx];
               // Spawn a PowerUp
               GameObject go = Instantiate(prefabPowerUp) as GameObject;
               PowerUp pu = go.GetComponent<PowerUp>();
               // Set it to the proper WeaponType
               pu.SetType(puType); // f
                  // Set it to the position of the destroyed ship
               pu.transform.position = e.transform.position;
          }
     }

     // This is to calculate the individual enemy deaths
     public void CalculateEnemyElims(Enemy e)
     {
          if (e is Enemy_1)
          {
               enemyScores[1]++;
               e1Score.GetComponent<Text>().text = "" + enemyScores[1];
          }
          else if (e is Enemy_2)
          {
               enemyScores[2]++;
               e2Score.GetComponent<Text>().text = "" + enemyScores[2];
          }
          else if (e is Enemy_3)
          {
               enemyScores[3]++;
               e3Score.GetComponent<Text>().text = "" + enemyScores[3];
          }
          else if (e is Enemy_4)
          {
               enemyScores[4]++;
               e4Score.GetComponent<Text>().text = "" + enemyScores[4];
          }
          else if (e is Enemy)
          {
               enemyScores[0]++;
               e0Score.GetComponent<Text>().text = "" + enemyScores[0];
          }
     }


     void Awake()
     {
          S = this;
          // Set bndCheck to reference the BoundsCheck component on this GameObject
          bndCheck = GetComponent<BoundsCheck>();
          // Invoke SpawnEnemy() once (in 2 seconds, based ondefault values)
          Invoke("SpawnEnemy", 1f / enemySpawnPerSecond); // a

          // A generic Dictionary with WeaponType as the key
          WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>(); // a
          foreach (WeaponDefinition def in weaponDefinitions)
          { // b
               WEAP_DICT[def.type] = def;
          }
     }

     public void SpawnEnemy()
     {
          // Reach maximum number of enemies on screen, then don't spawn enemies
          if (enemiesScreen >= currLvl.getMaxEnemy())
          {
               Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
               return;
          }


          ArrayList enemies = currLvl.getEnemies();
          // random prefab
          int ndx = Random.Range(0, enemies.Count);


          GameObject go = Instantiate<GameObject>(prefabEnemies[(int)enemies[ndx]]);
          enemiesScreen++; // Increment enemies on screen to keep track of enemies on screen

          float enemyPadding = enemyDefaultPadding;
          if (go.GetComponent<BoundsCheck>() != null)
          {
               enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
          }

          Vector3 pos = Vector3.zero;
          float xMin = -bndCheck.camWidth + enemyPadding;
          float xMax = bndCheck.camWidth - enemyPadding;
          pos.x = Random.Range(xMin, xMax);
          pos.y = bndCheck.camHeight + enemyPadding;
          go.transform.position = pos;

          Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
     }

     public void DelayedRestart(float delay)
     {
          // Invoke the Restart() method in delay seconds
          Invoke("Restart", delay);
     }
     public void Restart()
     {
          Time.timeScale = 1;
          score = 0;
          enemiesScreen = 0;
          // Reload _Scene_0 to restart the game
          SceneManager.LoadScene("SpaceGame");
     }

     static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
     {
          if (WEAP_DICT.ContainsKey(wt))
          {
               return (WEAP_DICT[wt]);
          }
          return (new WeaponDefinition());
     }

     // This function is used to initialize the sound and initialize the level
     void Start()
     {
          DestroySound.clip = DestroySounds[MainMenuController.DestroySound];
          DestroySound.volume = Volumes[MainMenuController.DestroySoundVolume];

          WinningSound.clip = WinningSounds[MainMenuController.WinningSound];
          WinningSound.volume = Volumes[MainMenuController.WinningSoundVolume];

          BackgroundMusic.clip = BackgroundMusics[MainMenuController.BackgroundSound];
          BackgroundMusic.volume = Volumes[MainMenuController.BackgroundSoundVolume];

          BackgroundMusic.Play();

          if (lvl == BRONZE)
          {
               levelStatus.GetComponentInChildren<Text>().text = "Bronze";
               levelStatus.GetComponent<Image>().color = new Color32(189, 147, 84, 255);
               currentBackground.sprite = bronzebackgrounds[MainMenuController.bronzeBg];
          }
          else if (lvl == SILVER)
          {
               levelStatus.GetComponentInChildren<Text>().text = "Silver";
               levelStatus.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
               currentBackground.sprite = silverbackgrounds[MainMenuController.silverBg];
          }
          else
          {
               levelStatus.GetComponentInChildren<Text>().text = "Gold";
               levelStatus.GetComponent<Image>().color = new Color32(255, 199, 53, 255);
               currentBackground.sprite = goldbackgrounds[MainMenuController.goldBg];
          }
          startTime = Time.time;
          restartButton.SetActive(false);
     }

     // This function is used every frame, and it updates and checks if the score has been surpassed for the current level
     // It also increments the time
     void Update()
     {

          if (currLvl.getScoreValue() <= score && lvl != GOLD)
          {
               WinningSound.Play();
               lvl++; // Increment the level when score threshold is passed
               currLvl = Settings.getLevel(lvl);
               //Debug.Log("current max yo: " + currLvl.getMaxEnemy());
               if (lvl == BRONZE)
               {
                    levelStatus.GetComponentInChildren<Text>().text = "Bronze";
                    levelStatus.GetComponent<Image>().color = new Color32(189, 147, 84, 255);
                    currentBackground.sprite = bronzebackgrounds[MainMenuController.bronzeBg];
               }
               else if (lvl == SILVER)
               {
                    levelStatus.GetComponentInChildren<Text>().text = "Silver";
                    levelStatus.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
                    currentBackground.sprite = silverbackgrounds[MainMenuController.silverBg];
               }
               else
               {
                    levelStatus.GetComponentInChildren<Text>().text = "Gold";
                    levelStatus.GetComponent<Image>().color = new Color32(255, 199, 53, 255);
                    currentBackground.sprite = goldbackgrounds[MainMenuController.goldBg];
               }
          }

          if (currLvl.getScoreValue() <= score && lvl == GOLD)
          {
               WinningSound.Play();
          }

          timeText.GetComponent<Text>().text = "Time: " + System.String.Format("{0:0.00}", Time.time - startTime);

     }

     public void TogglePause()
     {
          if (Time.timeScale == 1)    //Not paused
          {
               pausedText.GetComponent<Text>().text = "PAUSED";
               Time.timeScale = 0;
               restartButton.SetActive(true);
               exitButton.SetActive(true);
          }
          else // This is to unpause the game
          {
               Time.timeScale = 1;
               restartButton.SetActive(false);
               exitButton.SetActive(false);
               pausedText.GetComponent<Text>().text = "";
          }
     }

     // This function is used to end the game
     public void endGame()
     {
          score = 0;
          enemiesScreen = 0;
          SceneManager.LoadScene("SpaceMenu");
     }
}