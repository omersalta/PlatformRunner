using System;
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

    private void Update() {
        if (transform.position.y < -10) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        
        if (other.tag == "Box") {
            other.GetComponent<Box>().CollideWithPlayer();
        }
        
        if (other.tag == "FinalBox") {
            FindObjectOfType<GameManager>().GameWon();
        }
        
        if (other.tag == "Enemy") {
            FindObjectOfType<GameManager>().GameOver();
            Debug.Log("game over");
        }
        
        if (other.tag == "BotLargeCollider") {
            other.GetComponentInParent<Bot>().SlowDown();
        }
        
        if (other.tag == "BotReachCollider") {
            other.GetComponentInParent<Bot>().YouReachYourTarget();
        }
        
    }
    
    private void OnTriggerStay(Collider other) {
        if (other.tag == "BarrierBoxR") {
            playerCannotGoRigthAnyMore = true;
        }
        if (other.tag == "BarrierBoxL") {
            playerCannotGoLeftAnyMore = true;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.tag == "BarrierBoxR") {
            playerCannotGoRigthAnyMore = false;
        }
        if (other.tag == "BarrierBoxL") {
            playerCannotGoLeftAnyMore = false;
        }
    }

    public void die() {
        FindObjectOfType<GameManager>().GameOver();
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
