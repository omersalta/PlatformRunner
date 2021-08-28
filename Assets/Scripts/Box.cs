using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    
    
    static private float fallTime = 1.2f;
    static private float fallVelocity = 5f;
    static private float updatefrequency = 0.01f;
    
    void Start()
    {
        
    }
    
    public void CollideWithPlayer() {
        ChangeColor();
        //FallBox();
        Invoke("FallBox",0.2f);
    }

    private void ChangeColor() {
        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    private void FallBox() {
        StartCoroutine(FallCoroutine(0f));
    }

    
    IEnumerator FallCoroutine(float currentFrequency) {
        yield return new WaitForSeconds(updatefrequency);

        transform.position += Vector3.down * Time.deltaTime * fallVelocity;
        
        if (currentFrequency >= fallTime) {
            StopCoroutine(FallCoroutine(0f));
            Destroy(gameObject);
        }else {
            if (currentFrequency > 0.2f) {
                GetComponent<BoxCollider>().enabled = false;
            }
            StartCoroutine(FallCoroutine(currentFrequency+updatefrequency));
        }
    }
    
}
