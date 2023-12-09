using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour {
    
    // globals
    public bool playIntro = false;
    public MainManager mainManager;

    private void Awake() {
        DontDestroyOnLoad(this);
        if (playIntro)
            StartCoroutine(introStart());
        else {
            mainManager.getSceneManager().loadScene("MainMenuScene");
            Destroy(this);
        }
    }

    // start the intro
    private IEnumerator introStart() {
        mainManager.getSceneManager().loadScene("Test Intro Scene");
        yield return new WaitForSeconds(13f);
        mainManager.getSceneManager().loadScene("MainMenuScene");
    }
}
