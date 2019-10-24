using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Basket : MonoBehaviour
{
    public Text scoreGT;

     public static int realScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Find a reference to the ScoreCounter GameObject
        GameObject scoreGO = GameObject.Find("ScoreCounter");

        //Get the text component of that GameObject
        scoreGT = scoreGO.GetComponent<Text>();

        //Set the starting number of points to 0
        scoreGT.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current screen position of the mouse from Input
        Vector3 mousePos2D = Input.mousePosition;

        // The Camera's z position sets how far to push the mouse i 
        mousePos2D.z = -Camera.main.transform.position.z;

        // Convert the point from 2D screen space into 3D game world space
        Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 pos = this.transform.position;
        pos.x = mousePos3d.x;
        this.transform.position = pos;
    }

    void OnCollisionEnter(Collision coll)
    {
        // Find out what hit this basket
        GameObject collidedWith = coll.gameObject;
        if (collidedWith.tag == "Apple")
        {
            Destroy(collidedWith);


            int score = int.Parse(scoreGT.text);
            score += 100;
            realScore += 100;
            scoreGT.text = score.ToString();

            // Track highscore
            if (score > HighScore.score)
            {
                HighScore.score = score; 
            }
        }
    }
}
