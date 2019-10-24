using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS CLASS IS USED TO KEEP TRACK OF VALUES FOR EACH LEVEL

public class Level
{

     // INSTANCE VARIABLES FOR NAME OF LEVEL, TOP SCORE, TYPES OF ENEMIES, AND MAXIMUM AMOUNT OF ENEMIES
     private string name;
     private int topScore;
     private ArrayList enemies;
     private bool shootingEnabled;
     private int maxEnemy;


     // Constructor for levels
     public Level(string name, int score, int max)
     {
          this.name = name;
          this.topScore = score;
          this.shootingEnabled = true;
          this.enemies = new ArrayList();
          this.maxEnemy = max;
     }

     // Set max enemies
     public void setMaxEnemies(int max)
     {
          this.maxEnemy = max;
     }

     // Set top score to advance to next level
     public void setTopScore(int score)
     {
          this.topScore = score;
     }

     // Set shooting
     public void setShootingEnabled(bool flag)
     {
          this.shootingEnabled = flag;
     }

     // Add enemy to level
     public bool addEnemy(int enemyID)
     {
          // check if obj enemies already contains the enemy
          for (int i = 0; i < this.enemies.Count; i++)
          {
               if (enemyID == (int)this.enemies[i])
               {
                    return false;
               }
          }
          this.enemies.Add(enemyID);
          return true;
     }

     // Remove enemy from level
     public bool removeEnemy(int enemyID)
     {
          // check if object contains the enemy
          for (int i = 0; i < this.enemies.Count; i++)
          {
               if (enemyID == (int)this.enemies[i])
               {
                    // enemy found
                    this.enemies.Remove(enemyID);
                    return true;
               }
          }
          // return false as an error if enemy not found
          return false;
     }

     // This is to add enemies or remove
     public bool toggleEnemy(int enemyID)
     {
          // check if object contains the enemy
          for (int i = 0; i < this.enemies.Count; i++)
          {
               if (enemyID == (int)this.enemies[i])
               {
                    // enemy found
                    this.enemies.Remove(enemyID);
                    return false;
               }
          }
          this.enemies.Add(enemyID);
          return true;
     }

     // Get the enemies
     public ArrayList getEnemies()
     {
          return this.enemies;
     }

     // Let shooting be enabled
     public bool getShootingEnabled()
     {
          return this.shootingEnabled;
     }


     // Get the score value
     public int getScoreValue()
     {
          return this.topScore;
     }


     // Get the maximum number of enemies
     public int getMaxEnemy()
     {
          return this.maxEnemy;
     }

}
