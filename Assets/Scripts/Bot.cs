using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bot : MonoBehaviour {
    
    private static float MaxSpeed = 12f;
    private float forceMult = 0.85f;
    public static bool startLock = true;
    
    private float currentSpeed = 0f;
    private Rigidbody rb;
    private int i = 0;
    
    void Start() {
        rb = GetComponent<Rigidbody>();
        RandomizePosition();
    }
    
    // Update is called once per frame
    void Update() {
        ForceForward();
    }

    public void RandomizePosition() {
        var x = Random.Range(-0.5f, 0.5f);
        var z = Random.Range(-0.5f, 0.5f);
        
        Vector3 newLocPos = transform.localPosition;
        transform.localPosition = newLocPos + new Vector3(x, 0f, z);
    }
    
    void ForceForward() {
        GetComponent<Animator>().SetFloat("Move", 1);
        Vector3 currentV = rb.velocity;
        rb.velocity = new Vector3(currentV.x, currentV.y, MaxSpeed*forceMult);
    }
    
    public void SlowDown() {
        forceMult = 0.605f;
    }
    
    
    
    
}
