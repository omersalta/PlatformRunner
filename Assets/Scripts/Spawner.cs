using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public GameObject prefab;
    public bool active;
    public Vector2 delayRandomRange;
    public Vector2 fallowOffset;
    private float randomDelay;
    
    // Start is called before the first frame update
    void Start() {
        randomDelay = 0;
    }

    // Update is called once per frame
    void Update() {
        transform.position = FindObjectOfType<Player>().transform.position + new Vector3(0,fallowOffset.x,fallowOffset.y);
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
            var newTransform = transform;
            GameObjectUtil.Instantiate(prefab, newTransform.position);
            ResetDelay();
            StartCoroutine (EnemyGenerator ());
        }
        
    }

    void ResetDelay(){
        randomDelay = Random.Range (delayRandomRange.x, delayRandomRange.y);
    }
    
    
    
}
