using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PannelManager : MonoBehaviour {

    public GameObject StartMenuObject;
    public GameObject GameoverMenuObject;
    public GameObject nextLevelButtonObject;

    public GameObject levelTextObject;
    public GameObject hand;
    private bool direction = true;
    
    void Start() {
        OpenStartMenu();
        CloseGameoverMenu();
        UpdateLevelText();
    }

    public void GameStart() {
        CloseStartMenu();
        hand.SetActive(false);
    }
    
    public void Gameover() {
        nextLevelButtonObject.SetActive(false);
        OpenGameoverMenu();
        CloseStartMenu();
    }
    
    public void GameWon() {
        nextLevelButtonObject.SetActive(true);
        OpenGameoverMenu();
        CloseStartMenu();
    }

    public void RestartLevelButton() {
        GameObjectUtil.ResetPools();
        FindObjectOfType<LevelControl>().loadSceneAgain();
    }
    
    public void NextLevelButton() {
        GameObjectUtil.ResetPools();
        FindObjectOfType<LevelControl>().loadNextScene();
    }
    

    // Update is called once per frame
    void Update() {
        Animate(hand);
    }

    void UpdateLevelText() {
        levelTextObject.GetComponent<TextMeshProUGUI>().text += FindObjectOfType<LevelControl>().GetCurrentLevelIndex();
        //levelTextObject.GetComponent<TextMeshProUGUI>().text = "changed";
    }

    private void OpenGameoverMenu() {
        GameoverMenuObject.SetActive(true);
    }
    
    private void CloseGameoverMenu() {
        GameoverMenuObject.SetActive(false);
    }
    
    private void OpenStartMenu() {
        StartMenuObject.SetActive(true);
    }
    
    private void CloseStartMenu() {
        StartMenuObject.SetActive(false);
    }
    
    private float GetNewVelocityValue(float currentValue, float maxValue, float velocity) {
        
        var speedMult = maxValue - Mathf.Abs(currentValue);
        var scaleFix = maxValue / 3;
        speedMult += scaleFix;
        //speedMult = speedMult < scaleFix ? speedMult : scaleFix;

        if (!direction) {
            //negative
            var newPos = currentValue - (speedMult * velocity);
            if (newPos < -maxValue) {
                direction = !direction;
                return 0;
            }else {
                return -(speedMult * velocity);
            }

        }else {
            //positive
            var newPos = currentValue + (speedMult * velocity);
            if (newPos > maxValue) {
                direction = !direction;
                return 0;
            }else {
                return (speedMult * velocity);
            }
           
        }
    }
    
    void Animate(GameObject given) {
        var newPos = GetNewVelocityValue(given.transform.localPosition.x, 70, 0.03f);
        given.transform.localPosition += new Vector3(newPos, 0, 0);
    }
}
