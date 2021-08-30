using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public GameObject prefab;
    public bool active;
    public Vector2 delayRandomRange;
    public Vector2 fallowOffset;
    private float randomDelay;
    private Player P;
    
    // Start is called before the first frame update
    void Start() {
        randomDelay = 0;
        
        var player = FindObjectOfType<Player>();
        if (player) {
            P = player;  
        }
    }
    
    // Update is called once per frame
    void Update() {
        
        if (P) {
            transform.position = P.transform.position + new Vector3(0, fallowOffset.x, fallowOffset.y);
        }
        
    }

    public void ActiveSpawner() {
        active = true;
        StartCoroutine(EnemyGenerator());
    }
    
    IEnumerator EnemyGenerator(){

        yield return new WaitForSeconds (randomDelay);

        if (!active) {
            StopCoroutine(EnemyGenerator ());
        }
        else {
            var newPos = transform.position;
            
            GameObjectUtil.Instantiate(prefab, newPos);
            ResetDelay();
            StartCoroutine (EnemyGenerator ());
        }
        
    }

    void ResetDelay(){
        randomDelay = Random.Range (delayRandomRange.x, delayRandomRange.y);
    }
    
    
    
}
