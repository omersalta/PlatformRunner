using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : Singleton<LevelControl> {
    
    private int currentLevelSceneIndex;
    [SerializeField] private string PreNameOfScene;
    void Start() {
        currentLevelSceneIndex = 1;
        PreNameOfScene = "Level_";
    }

    public void loadNextScene() {
        SceneManager.LoadScene(GetScenePreName() + GetNextLevelIndex());
        currentLevelSceneIndex++;
    }
    
    public void loadSceneAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private string GetNextLevelIndex() {
        return (currentLevelSceneIndex + 1).ToString();
    }

    private string GetScenePreName() {
        return PreNameOfScene;
    }
}

