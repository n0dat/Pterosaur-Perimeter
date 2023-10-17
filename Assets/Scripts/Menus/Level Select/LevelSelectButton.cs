using UnityEngine;

public class LevelSelectButton : MonoBehaviour {
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Level levelToLoad;
    [SerializeField] private string levelPrefabName;
    
    private MainManager mainManager;

    void Start() {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        if (mainManager == null)
            Debug.Log("LevelSelectButton : Unable to get MainManager component.");
        
        levelToLoad = (Resources.Load("Levels/" + levelPrefabName) as GameObject).GetComponent<Level>();
    }

    public void buttonPressed() {
        var sceneLoader = mainManager.getSceneManager();
        if (sceneLoader) {
            mainManager.getSceneManager().loadScene(sceneToLoad);
            mainManager.getLevelManager().loadLevel(levelToLoad);
            mainManager.getStateManager().setGameState(StateManager.GameState.Playing);
            mainManager.getTowerSelector().getNewCamera();
        }
        else {
            Debug.Log("Unable to get Scene Manager");
        }
    }
}