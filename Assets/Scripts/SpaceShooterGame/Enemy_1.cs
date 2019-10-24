using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Enemy_1 extends the Enemy class
public class Enemy_1 : Enemy
{ // a
     [Header("Set in Inspector: Enemy_1")]
     // # seconds for a full sine wave
     public float waveFrequency = 2;
     // sine wave width in meters
     public float waveWidth = 4;
     public float waveRotY = 45;
     private float x0; // The initial x value of pos
     private float birthTime;
     private int score1;
     // Start works well because it's not used by the Enemy superclass


     void Start()
     {

          score1 = Settings.getScore(1);
          // Set x0 to the initial x position of Enemy_1
          x0 = pos.x; // b
          birthTime = Time.time;

          foreach (Transform child in this.transform)
          {
               switch (Settings.getColor(1))
               {
                    case 1: child.gameObject.GetComponent<Renderer>().material.color = Main.S.blue; break;
                    case 2: child.gameObject.GetComponent<Renderer>().material.color = Main.S.green; break;
                    case 3: child.gameObject.GetComponent<Renderer>().material.color = Main.S.red; break;
               }
          }
     }
     public int getScore()
     {
          return score1;
     }


     // Override the Move function on Enemy
     public override void Move()
     { // c
       // Because pos is a property, you can't directly set pos.x
       // so get the pos as an editable Vector3
          Vector3 tempPos = pos;
          // theta adjusts based on time
          float age = Time.time - birthTime;
          float theta = Mathf.PI * 2 * age / waveFrequency;
          float sin = Mathf.Sin(theta);
          tempPos.x = x0 + waveWidth * sin;
          pos = tempPos;
          // rotate a bit about y
          Vector3 rot = new Vector3(0, sin * waveRotY, 0);
          this.transform.rotation = Quaternion.Euler(rot);
          // base.Move() still handles the movement down in y
          base.Move();
          // d
          // print( bndCheck.isOnScreen );
     }

     void OnCollisionEnter(Collision coll)
     {
          GameObject otherGO = coll.gameObject; // a
          switch (otherGO.tag)
          {
               case "ProjectilePlayer": // b
                    Projectile p = otherGO.GetComponent<Projectile>();
                    // If this Enemy is off screen, don't damage it.
                    if (!bndCheck.isOnScreen)
                    { // c
                        
                         Destroy(otherGO);
                    }
                    // Hurt this Enemy
                    base.ShowDamage();
                    // Get the damage amount from the Main WEAP_DICT.
                    base.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                    if (base.health <= 0)
                    {
                         // Tell the Main singleton that this ship was destroyed // b
                         if (!notifiedOfDestruction)
                         {
                              Main.enemiesScreen--;
                              Main.S.shipDestroyed(this, score1);
                         }
                         notifiedOfDestruction = true;
                         // Destroy this Enemy
                         Destroy(this.gameObject);
                    }
                    Destroy(otherGO); // e
                    break;
               default:
                    print("Enemy hit by non-ProjectileHero: " +
                    otherGO.name); // f
                    break;
          }
     }

}