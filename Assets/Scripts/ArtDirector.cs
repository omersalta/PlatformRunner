using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;
using Random = UnityEngine.Random;
public class ArtDirector : MonoBehaviour {
    
    private PlayableDirector director;

    public List<GameObject> cartoonFXes;
    
    void Awake() {
        director = GetComponent<PlayableDirector>();
    }

    public void CreateFX(int index, Vector3 pos) {
        if (cartoonFXes.ElementAtOrDefault(index) == null) {
            Debug.LogWarning("index out of range");
            return;
        }

        
        GameObjectUtil.Instantiate(cartoonFXes[index], pos);
        
    }

    private Vector3 RandomizedPosForFX(float randRange) {
        var newPos = Camera.main.transform.position;
        return newPos + new Vector3(Random.Range(-randRange,randRange), Random.Range(-randRange,randRange), 8);
    }
    
    private void CreateFXMore(int index,int count) {
        
        for (int i = 0; i < count; i++) {
            CreateFX(index, RandomizedPosForFX(6));    
        }
        
    }

    private void SpesificEditedFXUsing(float totalTime, float updateFreq) {
        StartCoroutine(FXRoutine(0f));

        IEnumerator FXRoutine(float currentFreq) {
            Debug.Log("FXRoutine freq:"+ currentFreq);
            
            if (Random.Range(0,100)<40) {
                CreateFXMore(3, 1);
                CreateFXMore(2, 1);
                CreateFXMore(1, 1);
            }
            
            if (Random.Range(0,100)<50) {
                CreateFXMore(0, 1);
                CreateFXMore(1, 1);
            }
            
            if (Random.Range(0,100)<80) {
                CreateFXMore(2, 1);
            }
            
            yield return new WaitForSeconds(updateFreq);
            
            if (currentFreq < totalTime) {
                StartCoroutine(FXRoutine(currentFreq + updateFreq));
            }else {
                StopCoroutine(FXRoutine(0));
            }

        }
    }
    
    public void startTimeline() {
        director.Play();
    }
    
    public void PlayerFinishingCinematic(GameObject Player) {
        SpesificEditedFXUsing(4,0.3f);
        FindObjectOfType<CharacterControl>().Lock();
        float totalTime = 1.5f;
        float freq = 0.01f;
        Vector3 targetPos = GameObject.Find("FinishTarget").transform.position;
        Vector3 singleFreqDiff = Vector3.LerpUnclamped(Vector3.zero, targetPos-Player.transform.position,1/(totalTime/freq) );
        
        Player.transform.rotation = Quaternion.LookRotation(singleFreqDiff);
        StartCoroutine(FınıshRunCourrutine(0f));
        
        IEnumerator FınıshRunCourrutine(float currentFrequancy) {
            yield return new WaitForSeconds(freq);
            Player.transform.position += singleFreqDiff;
            if (currentFrequancy < totalTime) {
                StartCoroutine(FınıshRunCourrutine(currentFrequancy+freq));
            }else {
                StopCoroutine(FınıshRunCourrutine(currentFrequancy));
                director.Evaluate();
                Debug.Log("timeline started");
                startTimeline();
            }
        }
        
    }
}
