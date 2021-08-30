using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bot : MonoBehaviour,IRecyle {
    
    public static Vector2 startPosLimit = new Vector2(8,7);
    private static GameObject allBotsWantsToReachThis;
    public static Vector3 tagetCurrentDir;
    
    private static float forwardVelocity = 13f;
    private static float normalSpeedMult = 0.9f;
    private static float whenCloseSpeedMult = 0.7f;
    
    
    private List<Collider> collisions = new List<Collider>();   
    
    private bool isGrounded;
    private bool isReachedTarget;
    private bool isCloseTarget;
    private int i = 0;
    
     
    public void Restart() {
        RandomizePosition();
        ForceForward();
        isReachedTarget = false;
        isCloseTarget = false;
    }

    public void Shutdown() {
        
    }
    
    // Update is called once per frame
    void FixedUpdate() {
        //rb.velocity = new Vector3(0, -10, forwardVelocity*speedMult);
        var currentVVelocity = forwardVelocity;

        if (transform.position.y < -50) {
            GameObjectUtil.Destroy(gameObject);
        }
        
        if (!isGrounded) {
            currentVVelocity = 0;
        }

        float mult;
        Vector3 currentDirection;
        
        if (isReachedTarget) {
            transform.position += tagetCurrentDir * Time.deltaTime;
        }
        else {
            currentDirection = currentVVelocity * Vector3.forward;
            mult =  isCloseTarget ? whenCloseSpeedMult : normalSpeedMult;
            transform.position += currentDirection * mult * Time.deltaTime;
        }
        
    }
    
    public static void UpdateTargetV (Vector3 curentDir) {
        tagetCurrentDir = curentDir;
    }
    
    
    
    public void RandomizePosition() {
        var x = Random.Range(-0.5f, startPosLimit.x);
        var z = Random.Range(-0.5f, startPosLimit.y);
        
        Vector3 newLocPos = new Vector3(x, 0f, z);;
        transform.localPosition = reCalculatePosAccordingLimits(newLocPos);
    }

    private Vector3 reCalculatePosAccordingLimits (Vector3 givenPos) {

        if (givenPos.x < startPosLimit.x) {
            givenPos = new Vector3(startPosLimit.x, givenPos.y, givenPos.z);
            return givenPos;
        }
        
        if (givenPos.x > startPosLimit.y) {
            givenPos = new Vector3(startPosLimit.y, givenPos.y, givenPos.z);
            return givenPos;
        }
        
        return givenPos;
    }
    
    void ForceForward() {
        GetComponent<Animator>().SetFloat("Move", 1);
    }
    
    public void SlowDown() {
        Debug.Log("slowDowned Called");
        isCloseTarget = true;
    }
    
    public void YouReachYourTarget() {
        Debug.Log("YouReachYourTarget Called");
        isReachedTarget = true;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!collisions.Contains(collision.collider))
                {
                    collisions.Add(collision.collider);
                }
                isGrounded = true;
            }
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            isGrounded = true;
            if (!collisions.Contains(collision.collider))
            {
                collisions.Add(collision.collider);
            }
        }
        else
        {
            if (collisions.Contains(collision.collider))
            {
                collisions.Remove(collision.collider);
            }
            if (collisions.Count == 0) { isGrounded = false; }
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collisions.Contains(collision.collider))
        {
            collisions.Remove(collision.collider);
        }
        if (collisions.Count == 0) { isGrounded = false; }
    }

    
    
}
