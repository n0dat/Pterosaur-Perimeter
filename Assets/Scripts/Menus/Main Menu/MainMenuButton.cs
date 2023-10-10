using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour {
    
    // globals
    [SerializeField]
    private string sceneToLoad;
    
    [SerializeField]
    private bool isExitButton, isLevelStartButton, isSettingsExitButton;
    
    private MainManager mainManager;

    void Start() {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        if (mainManager == null)
            Debug.Log("MainMenuButton : Unable to get MainManager component.");
    }

    public void ButtonPressed() {
        var sceneLoader = mainManager.getSceneManager();
        if (sceneLoader != null) {
            mainManager.getSceneManager().loadScene(sceneToLoad);
            if (isLevelStartButton)
                mainManager.getStateManager().setGameState(StateManager.GameState.Playing);
            if (isSettingsExitButton)
                mainManager.getSettingsManager().saveSettings();
        }
        else
            Debug.Log("Unable to get Scene Manager.");
    }

    public void ExitGame() {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}