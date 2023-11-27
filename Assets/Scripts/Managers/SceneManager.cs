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
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
        mainManager.getSettingsManager().getNewCamera();
        if (asyncLoad == null)
            Debug.Log("Failed to load scene : " + sceneToLoad);
    }

    public void reloadScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
