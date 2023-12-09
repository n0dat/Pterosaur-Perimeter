using UnityEngine;

public class MainMenuButton : MonoBehaviour {
    
    // globals
    [SerializeField] private string sceneToLoad;
    
    [SerializeField] private bool isLevelStartButton, isSettingsExitButton, isSettingsEnterButton;
    
    private MainManager mainManager;

    void Start() {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        if (mainManager == null)
            Debug.Log("MainMenuButton : Unable to get MainManager component.");
    }

    public void buttonPressed() {
        // load settings or level select scene
        var sceneLoader = mainManager.getSceneManager();
        if (sceneLoader != null) {
            mainManager.getSceneManager().loadScene(sceneToLoad);
            if (isLevelStartButton) {
                mainManager.getStateManager().setGameState(StateManager.GameState.Playing);
            }
            if (isSettingsExitButton)
                mainManager.getSettingsManager().saveSettings();
            if (isSettingsEnterButton)
                mainManager.getSettingsManager().loadSettings();
        }
        else
            Debug.Log("Unable to get Scene Manager.");
    }

    // exit the game from main menu
    public void exitGame() {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}