using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
     static public Player S; // Singleton // a
     [Header("Set in Inspector")]
     // These fields control the movement of the ship
     public float speed = 30;
     public float rollMult = -45;
     public float pitchMult = 30;
     public float gameRestartDelay = 2f;
     public GameObject projectilePrefab;
     public float projectileSpeed = 40;
     public Weapon[] weapons;
     [Header("Set Dynamically")]
     [SerializeField]
     public float _shieldLevel = 1;

     // This variable holds a reference to the last triggering GameObject
     public GameObject lastTriggerGo = null;

     public delegate void WeaponFireDelegate(); // a
                      // Create a WeaponFireDelegate field named fireDelegate.
     public WeaponFireDelegate fireDelegate;


     private float[] Volumes = { 1.0f, 0.5f, 0.1f };
     public AudioClip[] ShootingSounds = new AudioClip[3];
     public AudioSource Shoot;



     void Start()
     {
          if (S == null)
          {
               S = this; // Set the Singleton // a
               ClearWeapons();
               weapons[0].SetType(WeaponType.blaster);
          }
          else
          {
               Debug.LogError("Player.Awake() - Attempted to assign second Player.S!");
        }
          // fireDelegate += TempFire; 

          // Set up audio sounds
          Shoot.clip = ShootingSounds[MainMenuController.ShootingSound];
          Shoot.volume = Volumes[MainMenuController.ShootingSoundVolume];
     }
     void Update()
     {
          // Pull in information from the Input class
          float xAxis =
          Input.GetAxis("Horizontal"); // b
          float yAxis =
          Input.GetAxis("Vertical"); // b
                                     // Change transform.position based on the axes
          Vector3 pos = transform.position;
          pos.x += xAxis * speed * Time.deltaTime;
          pos.y += yAxis * speed * Time.deltaTime;
          transform.position = pos;
          // Rotate the ship to make it feel more dynamic // c
          transform.rotation =
          Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
/*
          // Allow the ship to fire
          if (Input.GetKeyDown(KeyCode.Space))
          { // a
               TempFire();
          }
*/

          if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
          { // d
               Shoot.Play();
               fireDelegate();
               // e
          }
     }



     void OnTriggerEnter(Collider other)
     {
          Transform rootT = other.gameObject.transform.root;
          GameObject go = rootT.gameObject;
          //print("Triggered: "+go.name); // b
          // Make sure it's not the same triggering go as last time
          if (go == lastTriggerGo)
          { // c
               return;
          }
          lastTriggerGo = go; // d
          if (go.tag == "Enemy")
          { // If the shield was triggeredby an enemy
               shieldLevel--; // Decrease the level of the shield by 1
               Destroy(go); // … and Destroy theenemy // e
          }
          else if (go.tag == "PowerUp")
          {
               // If the shield was triggered by a PowerUp
               AbsorbPowerUp(go);
          }
          else
          {
               print("Triggered by non-Enemy:"+go.name); // f
        }
     }

     public void AbsorbPowerUp(GameObject go)
     {
          PowerUp pu = go.GetComponent<PowerUp>();
          switch (pu.type)
          {
               case WeaponType.shield: // a
                    shieldLevel++;
                    break;
               default:
                    // b
                    if (pu.type == weapons[0].type)
                    { // If it is the same type // c
                         Weapon w = GetEmptyWeaponSlot();
                         if (w != null)
                         {
                              // Set it to pu.type
                              w.SetType(pu.type);
                         }
                    }
                    else
                    { // If this is a different weapon type // d
                         ClearWeapons();
                         weapons[0].SetType(pu.type);
                    }
                    break;
          }
          pu.AbsorbedBy(this.gameObject);
     }

     public float shieldLevel
     {
          get
          {
               return (_shieldLevel); // a
          }
          set
          {
               _shieldLevel = Mathf.Min(value, 4); // b
                                                   // If the shield is going to be set to less than zero
               if (value < 0)
               {
                    // When player dies, then add the game to history 
                    if (Users.useratm== null)
                    {
                         SpaceMenu.logs.Add(new GameLog("admin", System.DateTime.Now.ToString(), Main.score.ToString(), Main.lvl.ToString()));
                    }
                    else
                    {
                         SpaceMenu.logs.Add(new GameLog(Users.useratm.username, System.DateTime.Now.ToString(), Main.score.ToString(), Main.lvl.ToString()));
                    }

                    SpaceMenu.SaveGameData();
                    Destroy(this.gameObject);
                    // Tell Main.S to restart the game after a delay
                    Main.S.DelayedRestart(gameRestartDelay); // a
               }
          }
     }

     Weapon GetEmptyWeaponSlot()
     {
          for (int i = 0; i < weapons.Length; i++)
          {
               if (weapons[i].type == WeaponType.none)
               {
                    return (weapons[i]);
               }
          }
          return (null);
     }
     void ClearWeapons()
     {
          foreach (Weapon w in weapons)
          {
               w.SetType(WeaponType.none);
          }
     }
}