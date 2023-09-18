using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

    // globals
    [SerializeField]
    private Scene currentScene;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    public Scene getCurrentScene() {
        return currentScene;
    }

    public void loadScene(string sceneToLoad) {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.completed += onLoadSceneComplete;
    }

    public void onLoadSceneComplete(AsyncOperation asyncOp) {
        GameObject.Find("MouseManager").GetComponent<MouseInputManager>().getNewCamera();
    }
}
