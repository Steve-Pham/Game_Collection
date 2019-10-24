using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy_2 : Enemy
{ // a
     [Header("Set in Inspector: Enemy_2")]
     // Determines how much the Sine wave will affect movement
     public float sinEccentricity = 0.6f;
     public float lifeTime = 10;
     [Header("Set Dynamically: Enemy_2")]
     // Enemy_2 uses a Sin wave to modify a 2-point linear interpolation
     public Vector3 p0;
     public Vector3 p1;
     public float birthTime;
     private int score2;



     void Start()
     {

          score2 = Settings.getScore(2);


          // Pick any point on the left side of the screen
          p0 =
          Vector3.zero; //  b
          p0.x = -bndCheck.camWidth - bndCheck.radius;
          p0.y = Random.Range(-bndCheck.camHeight,
          bndCheck.camHeight);
          // Pick any point on the right side of the screen
          p1 = Vector3.zero;
          p1.x = bndCheck.camWidth + bndCheck.radius;
          p1.y = Random.Range(-bndCheck.camHeight,
          bndCheck.camHeight);
          // Possibly swap sides
          if (Random.value > 0.5f)
          {
               // Setting the .x of each point to its negative will  move it to
          // the other side of the screen
               p0.x *= -1;
               p1.x *= -1;
          }
          // Set the birthTime to the current time
          birthTime =
          Time.time; // c

          foreach (Transform child in this.transform)
          {
               switch (Settings.getColor(2))
               {
                    case 1: child.gameObject.GetComponent<Renderer>().material.color = Main.S.blue; break;
                    case 2: child.gameObject.GetComponent<Renderer>().material.color = Main.S.green; break;
                    case 3: child.gameObject.GetComponent<Renderer>().material.color = Main.S.red; break;
               }
          }
     }

     // Method to get score
     public int getScore()
     {
          return score2;
     }


     public override void Move()
     {
          // Bézier curves work based on a u value between 0 & 1
          float u = (Time.time - birthTime) / lifeTime;
          // If u>1, then it has been longer than lifeTime sincebirthTime
          if (u > 1)
          {
               // This Enemy_2 has finished its life
               Main.enemiesScreen--;
               Destroy(this.gameObject
               ); // d
               return;
          }
          // Adjust u by adding a U Curve based on a Sine wave
          u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));
          // Interpolate the two linear interpolation points
          pos = (1 - u) * p0 + u * p1;
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
                              Main.S.shipDestroyed(this, score2);
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