using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowingCamera : MonoBehaviour {
    
    private GameObject TargetObject;
    
    [SerializeField]private Vector3 fallowingDistanceOffset;
    [SerializeField]private Vector3 lookingAngle;

    // Start is called before the first frame update
    void Start() {
        TargetObject = FindObjectOfType<Player>().gameObject;
    }
    
    void FixedUpdate() {
        if (TargetObject.transform.position.y > -1) {
            Vector3 ofset = fallowingDistanceOffset;
            transform.position = Vector3.Lerp(transform.position, TargetObject.transform.position+ofset, 0.5f);
        }
        else {
            TargetObject.GetComponent<Player>().die();
        }
    }
    
}
