using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy_3 : Enemy
{ // Enemy_3 extends Enemy
  // Enemy_3 will move following a Bezier curve, which is a linear
     // interpolation between more than two points.
     [Header("Set in Inspector: Enemy_3")]
     public float lifeTime = 5;
     [Header("Set Dynamically: Enemy_3")]
     public Vector3[] points;
     public float birthTime;
     private int score3;
     // Again, Start works well because it is not used by the Enemysuperclass
     void Start()
     {

          score3 = Settings.getScore(3);

          points = new Vector3[3]; // Initialize points
                                   // The start position has already been set by Main.SpawnEnemy()
          points[0] = pos;
          // Set xMin and xMax the same way that Main.SpawnEnemy() does
          float xMin = -bndCheck.camWidth + bndCheck.radius;
          float xMax = bndCheck.camWidth - bndCheck.radius;
          Vector3 v;
          // Pick a random middle position in the bottom half of the screen
          v = Vector3.zero;
          v.x = Random.Range(xMin, xMax);
          v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
          points[1] = v;
          // Pick a random final position above the top of the screen
          v = Vector3.zero;
          v.y = pos.y;
          v.x = Random.Range(xMin, xMax);
          points[2] = v;
          // Set the birthTime to the current time
          birthTime = Time.time;


          foreach (Transform child in this.transform)
          {
               switch (Settings.getColor(3))
               {
                    case 1: child.gameObject.GetComponent<Renderer>().material.color = Main.S.blue; break;
                    case 2: child.gameObject.GetComponent<Renderer>().material.color = Main.S.green; break;
                    case 3: child.gameObject.GetComponent<Renderer>().material.color = Main.S.red; break;
               }
          }
     }

     public int getScore()
     {
          return score3;
     }

     public override void Move()
     {
          // Bezier curves work based on a u value between 0 & 1
          float u = (Time.time - birthTime) / lifeTime;
          if (u > 1)
          {
               // This Enemy_3 has finished its life
               Main.enemiesScreen--;
               Destroy(this.gameObject);
               return;
          }
          // Interpolate the three Bezier curve points
          Vector3 p01, p12;
          u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
          p01 = (1 - u) * points[0] + u * points[1];
          p12 = (1 - u) * points[1] + u * points[2];
          pos = (1 - u) * p01 + u * p12;
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
                              Main.S.shipDestroyed(this, score3);
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