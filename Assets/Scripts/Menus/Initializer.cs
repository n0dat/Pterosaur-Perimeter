using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour {
    
    // globals
    public bool playIntro = false;
   
    void Awake() {
        DontDestroyOnLoad(this);
        if (playIntro)
            StartCoroutine(introStart());
        else {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenuScene");
            Destroy(this);
        }
    }


    IEnumerator introStart() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test Intro Scene");
        yield return new WaitForSeconds(13f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        
    }
}
