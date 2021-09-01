using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public GameObject prefab;
    public bool active;
    public Vector2 delayRandomRange;
    public Vector3 fallowOffset;
    public Vector2 xLimit;
    public Vector2 zLimit;

    [SerializeField] private int singlePartyCreateCount; 
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
            transform.position = P.transform.position + fallowOffset;
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
            InstatntiateMore(prefab, singlePartyCreateCount);
            ResetDelay();
            StartCoroutine (EnemyGenerator ());
        }
        
    }
    
    public Vector3 RandomizePosition_XZ(Vector2 xLimit, Vector2 zLimit) {
        
        var x = Random.Range(xLimit.x, zLimit.y);;
        var z = Random.Range(zLimit.x, zLimit.y);
        Vector3 newLocPos = new Vector3(x, 0, z);
        return newLocPos;   
    }

    void InstatntiateMore(GameObject prefab, int count) {

        
        for (int i = 0; i < count; i++) {
            var newPos = RandomizePosition_XZ(xLimit, zLimit);
            newPos += transform.position;
            GameObjectUtil.Instantiate(prefab, newPos);
        }
        
    }

    void ResetDelay(){
        randomDelay = Random.Range (delayRandomRange.x, delayRandomRange.y);
    }
    
    
    
}
