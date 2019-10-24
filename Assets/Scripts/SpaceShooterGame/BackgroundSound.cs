using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
     // Use this for initialization
     void Start()
     {

     }

     //Play Global
     private static BackgroundSound instance = null;
     public static BackgroundSound Instance
     {
          get { return instance; }
     }

     // Don't Destroy the object on load
     void Awake()
     {
          if (instance != null && instance != this)
          {
               Destroy(this.gameObject);
               return;
          }
          else
          {
               instance = this;
          }

          DontDestroyOnLoad(this.gameObject);
     }
     //Play Gobal End

     // Update is called once per frame
     void Update()
     {

     }
}
