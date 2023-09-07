using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour {
    
    // globals
    [SerializeField]
    private string sceneToLoad;
    
    [SerializeField]
    private bool isExitButton;

    public void ButtonPressed() {
        var sceneManager = GameObject.Find("SceneManager");
        if (sceneManager != null) {
            var sceneLoader = sceneManager.GetComponent<SceneLoader>();
            if (sceneLoader != null)
                sceneLoader.LoadScene(sceneToLoad);
            else
                Debug.Log("Unable to get Scene Loader component of Scene Manager");
        }
        else
            Debug.Log("Unable to find Scene Manager in current Scene");
    }

    public void ExitGame() {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}