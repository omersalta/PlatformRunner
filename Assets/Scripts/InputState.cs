using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputState : Singleton<InputState> {

    private float currentGap = 0f;
    private Vector2 downPos;
    private Vector2 currentPos;
    private bool isDraging = false;
    public bool anyTap = false;

    private void Update() 
    {
        anyTap = false;
        currentPos = Input.mousePosition;
        
        #region Standalone Inputs
        if (!isDraging) {
            if (Input.GetMouseButtonDown(0))
            {
                isDraging = true;
                downPos = currentPos;
            }
        }
        else {
            InterpolateDownPos();
            UpdateCurrentGap();
        }
        
        if (Input.GetMouseButtonUp(0)) {
            ResetGap();
            Debug.Log("anytapping");
            isDraging = false;
            anyTap = true;
        }
        #endregion
        
        
        
        #region Mobile Inputs

        if (!isDraging) {
            if (Input.touches.Length > 0) {
                if (Input.touches[0].phase == TouchPhase.Began) {
                    isDraging = true;
                    downPos = currentPos;
                }

                else {
                    InterpolateDownPos();
                    UpdateCurrentGap();
                }
            }
        }

        if (Input.touches.Length > 0) {
            if (Input.touches[0].phase == TouchPhase.Ended) {
                ResetGap();
                Debug.Log("anytapping");
                isDraging = false;
                anyTap = true;
            }
        }

        #endregion
        
        
    }

    void printGap() {
        Debug.Log("gapNow :" + GetHorizontalGap());
    }

    void InterpolateDownPos() {
        downPos = Vector2.Lerp(downPos, currentPos, 0.05f);
    }

    void UpdateCurrentGap() {
        currentGap = -(downPos - currentPos).x;
    }
    
    public float GetHorizontalGap() {
        return  currentGap;
    }

    void ResetGap() {
        currentGap = 0;
    }
    
}
