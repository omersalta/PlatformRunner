using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelControl : Singleton<LevelControl> {
    
    
    private static int currentLevelSceneIndex = 1;
    [SerializeField] private string PreNameOfScene;
    
    void awake() {
        Debug.Log ("Script has been started");
        DontDestroyOnLoad (transform.gameObject);
    }
    
    void Start() {
        PreNameOfScene = "Level_";
    }

    public void loadNextScene() {
        
        string nextSceneName = GetScenePreName() + GetNextLevelIndex();
        
        if (Application.CanStreamedLevelBeLoaded(nextSceneName)) {
            currentLevelSceneIndex++;
            SceneManager.LoadScene(nextSceneName);
        }else{
            Debug.LogWarning("there is no next level");
        }

    }
    
    public void loadSceneAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public string GetCurrentLevelIndex() {
        return (currentLevelSceneIndex).ToString();
    }
    
    private string GetNextLevelIndex() {
        return (currentLevelSceneIndex + 1).ToString();
    }

    private string GetScenePreName() {
        return PreNameOfScene;
    }
}

