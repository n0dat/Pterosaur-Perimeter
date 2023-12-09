using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

    [SerializeField] private Scene currentScene;
    
    private MainManager mainManager;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        
        // get required references
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        mainManager.getSettingsManager().getNewCamera();
    }

    // return current scene
    public Scene getCurrentScene() {
        return currentScene;
    }

    // load scene with name
    public void loadScene(string sceneToLoad) {
        StartCoroutine(loadSceneAsync(sceneToLoad));
    }

    // load sync in background
    IEnumerator loadSceneAsync(string sceneToLoad) {
        var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
        if (asyncLoad == null)
            Debug.Log("Failed to load scene : " + sceneToLoad);
        
        while (!asyncLoad.isDone)
            yield return null;
        
        mainManager.getSettingsManager().getNewCamera();
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    // reload the currently active scene
    public void reloadScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
