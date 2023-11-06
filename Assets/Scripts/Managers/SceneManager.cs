using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

    // globals
    [SerializeField]
    private Scene currentScene;
    
    private MainManager mainManager;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    public Scene getCurrentScene() {
        return currentScene;
    }

    public void loadScene(string sceneToLoad) {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
        if (asyncLoad == null)
            Debug.Log("Failed to load scene : " + sceneToLoad);
    }
}
