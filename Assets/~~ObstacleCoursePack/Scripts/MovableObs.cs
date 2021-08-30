using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovableObs : MonoBehaviour
{
	public float distance = 5f; //Distance that moves the object
	public bool horizontal = true; //If the movement is horizontal or vertical
	public float speed = 3f;
	public float offset = 0f; //If yo want to modify the position at the start 

	private bool isStarted;
	private float maxRandomStartDelay;
	private bool isForward = true; //If the movement is out
	private Vector3 startPos;
   
    void Awake() {
	    maxRandomStartDelay = 1f;
	    isStarted = false;
	    startPos = transform.position;
		if (horizontal)
			transform.position += Vector3.right * offset;
		else
			transform.position += Vector3.forward * offset;
	}

    private void Start() {
	    Invoke("UnLock", Random.Range(0,maxRandomStartDelay));
    }

    // Update is called once per frame
    void Update() {
	    
	    if (!isStarted) {
		    return;
	    }
	    
	    if (isStarted) {
		    if (horizontal) {
			    if (isForward) {
				    if (transform.position.x < startPos.x + distance) {
					    transform.position += Vector3.right * Time.deltaTime * speed;
				    }
				    else
					    isForward = false;
			    }
			    else {
				    if (transform.position.x > startPos.x) {
					    transform.position -= Vector3.right * Time.deltaTime * speed;
				    }
				    else
					    isForward = true;
			    }
		    }
		    else {
			    if (isForward) {
				    if (transform.position.z < startPos.z + distance) {
					    transform.position += Vector3.forward * Time.deltaTime * speed;
				    }
				    else
					    isForward = false;
			    }
			    else {
				    if (transform.position.z > startPos.z) {
					    transform.position -= Vector3.forward * Time.deltaTime * speed;
				    }
				    else
					    isForward = true;
			    }
		    }
	    }
    }

    private void UnLock() {
	    isStarted = true;
    }
}
