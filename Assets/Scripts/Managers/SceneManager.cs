using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

    [SerializeField] private Scene currentScene;
    
    private MainManager mainManager;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        mainManager.getSettingsManager().getNewCamera();
    }

    public Scene getCurrentScene() {
        return currentScene;
    }

    public void loadScene(string sceneToLoad) {
        StartCoroutine(loadSceneAsync(sceneToLoad));
    }

    IEnumerator loadSceneAsync(string sceneToLoad) {
        var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
        if (asyncLoad == null)
            Debug.Log("Failed to load scene : " + sceneToLoad);
        
        while (!asyncLoad.isDone)
            yield return null;
        
        mainManager.getSettingsManager().getNewCamera();
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    public void reloadScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    
}
