using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

    // globals
    [SerializeField]
    private Scene currentScene;
    
    private StateManager stateManager;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    public Scene getCurrentScene() {
        return currentScene;
    }

    public void loadScene(string sceneToLoad) {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
        if (asyncLoad == null)
            Debug.Log("Failed to load scene : " + sceneToLoad);
        else {
            asyncLoad.completed += onLoadSceneComplete;
        }
    }

    public void onLoadSceneComplete(AsyncOperation asyncOp) {
        GameObject.Find("MouseManager").GetComponent<MouseInputManager>().getNewCamera();
    }
}
