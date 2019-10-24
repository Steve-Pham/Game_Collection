using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour {
    [Header("Set in Inspector")]

    // Prefab for instantiating apples
    public GameObject applePrefab;

    public float speed = 1f;

    public float leftandRightEdge = 10f;

    public float chanceToChangeDirections = 0.1f;

    public float secondsBetweenAppleDrops = 1f; 

    // Start is called before the first frame update
    void Start()
    {
        // Dropping apples every second
        Invoke("DropApple", 2f);
    }

    void DropApple()
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);
    }

    // Update is called once per frame
    void Update() {
        // Basic movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        // Changing direction
        if (pos.x  < -leftandRightEdge)
        {
            speed = Mathf.Abs(speed); // Move right
        } else if (pos.x  > leftandRightEdge)
        {
            speed = -Mathf.Abs(speed); // Move left
        }
    }

    void FixedUpdate()
    {
        // Changing direction Randomly is now time-based because of FixedUpdate
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1; // Change direction 
        }
    }
}
