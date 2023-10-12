using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour {
    
    // globals
    [SerializeField]
    private string sceneToLoad;
    
    [SerializeField]
    private bool isLevelStartButton, isSettingsExitButton, isSettingsEnterButton;
    
    private MainManager mainManager;

    void Start() {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        if (mainManager == null)
            Debug.Log("MainMenuButton : Unable to get MainManager component.");
    }

    public void buttonPressed() {
        
        mainManager.getLevelManager().displayDisasterNotification("METEOR");
        
        var sceneLoader = mainManager.getSceneManager();
        if (sceneLoader != null) {
            mainManager.getSceneManager().loadScene(sceneToLoad);
            if (isLevelStartButton) {
                mainManager.getStateManager().setGameState(StateManager.GameState.Playing);
                mainManager.getTowerSelector().getNewCamera();
            }
            if (isSettingsExitButton)
                mainManager.getSettingsManager().saveSettings();
            if (isSettingsEnterButton)
                mainManager.getSettingsManager().loadSettings();
        }
        else
            Debug.Log("Unable to get Scene Manager.");
    }

    public void exitGame() {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}