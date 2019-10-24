using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ApplePicker : MonoBehaviour
{
    [Header("Set in Inspector")]

    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList; 


    // Start is called before the first frame update
    void Start()
    {
        basketList = new List<GameObject>();
        for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }
    }

    public void AppleDestroyed()
    {

        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach( GameObject tGO in tAppleArray)
        {
            Destroy(tGO);
        }

        //Destroy one of the baskets
        // Get the index of the last basket in basketlist
        int basketIndex = basketList.Count - 1;
        // Get a reference to that Basket GameObject
        GameObject tBasketGO = basketList[basketIndex];
        // Remove the basket from the list and destroy the GameObject
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGO);

        if (basketList.Count ==0)
        {
               if (Users.useratm == null)
               {
                    AppleMenu.logs.Add(new GameLog("admin", System.DateTime.Now.ToString(), Basket.realScore.ToString(), "n/a"));
               }
               else
               {
                    AppleMenu.logs.Add(new GameLog(Users.useratm.username, System.DateTime.Now.ToString(), Basket.realScore.ToString(), "n/a"));
               }
               SaveGameData(); // save game data
               Basket.realScore = 0; // reset score back to 0
               SceneManager.LoadScene("AppleMenu");
       
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     // Save the current data to the JSON file for the history
     public void SaveGameData()
     {
          gameData newData = new gameData();
          string path = Application.streamingAssetsPath + "/appleLogs.json";
          foreach (GameLog log in AppleMenu.logs)
          {
               newData.Add(log);
          }
          Debug.Log(JsonUtility.ToJson(newData));
          File.WriteAllText(path, JsonUtility.ToJson(newData));
     }
}
