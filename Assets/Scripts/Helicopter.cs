using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour {
    
    public GameObject topRotor;
    public GameObject backRotor;

    void Update()
    {
        backRotor.transform.Rotate(10,0,0);
        topRotor.transform.Rotate(0,0,3);
    }
}
