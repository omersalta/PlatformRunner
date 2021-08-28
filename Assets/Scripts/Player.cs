using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    
    [SerializeField] private Vector2 controlLimit;
    private Vector3 playerInitialPos;

    private bool playerCannotGoLeftAnyMore;
    private bool playerCannotGoRigthAnyMore;

    private InputState IS;
    
    // Start is called before the first frame update
    void Start() {
        playerInitialPos = new Vector3(4, 0.5f, 8);
        IS = FindObjectOfType<InputState>();
    }

    void OnTriggerEnter(Collider other) {
        
        if (other.tag == "Box") {
            other.GetComponent<Box>().CollideWithPlayer();
        }
        
        if (other.tag == "Enemy") {
            FindObjectOfType<GameManager>().GameOver();
            Debug.Log("game over");
        }
        
        if (other.tag == "BotLargeCollider") {
            other.GetComponentInParent<Bot>().SlowDown();
        }
        
    }
    
    public void die() {
        FindObjectOfType<GameManager>().GameOver();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (transform.position.x < controlLimit.x) {
            playerCannotGoLeftAnyMore = true;
        }else {
            playerCannotGoLeftAnyMore = false;
        }
        
        if (transform.position.x > controlLimit.y) {
            playerCannotGoRigthAnyMore = true;
        }else {
            playerCannotGoRigthAnyMore = false;
        }
        
    }

    public float LimitedGapCalculator() {
        var currentGap = IS.GetHorizontalGap();
        
        if (playerCannotGoLeftAnyMore) {
            if (currentGap < 0) {
                return 0f;
            }
        }
        
        if (playerCannotGoRigthAnyMore) {
            if (0 < currentGap) {
                return 0f;
            }
        }
        return currentGap;
    }
    
}
