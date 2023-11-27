using UnityEngine;

public class MainManager : MonoBehaviour {
    
    private Settings settingsManager;
    private SceneManager sceneManager;
    private StateManager stateManager;
    private PlayerManager playerManager;
    private GameManager gameManager;

    void Awake() {
	    DontDestroyOnLoad(this);
	    
	    settingsManager = GameObject.Find("SettingsManager").GetComponent<Settings>();
	    sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
	    stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
	    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public Settings getSettingsManager() {
	    return this.settingsManager;
    }

    public SceneManager getSceneManager() {
	    return this.sceneManager;
    }

    public StateManager getStateManager() {
	    return this.stateManager;
    }

    public GameManager getGameManager() {
	    return this.gameManager;
    }
}
