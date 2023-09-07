using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    // globals
    [SerializeField]
    private Scene currentScene;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        currentScene = SceneManager.GetActiveScene();
    }

    public Scene getCurrentScene() {
        return currentScene;
    }

    public void LoadScene(string sceneToLoad) {
        currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
