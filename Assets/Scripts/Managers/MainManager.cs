using UnityEngine;

public class MainManager : MonoBehaviour {
    
    private Settings settingsManager;
    private SceneManager sceneManager;
    private StateManager stateManager;
    private PlayerManager playerManager;
    
    private bool[] levelsCompleted = {false, false, false, false, false};

    void Awake() {
	    DontDestroyOnLoad(this);
	    
	    settingsManager = GameObject.Find("SettingsManager").GetComponent<Settings>();
	    sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
	    stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
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

    public int getNumLevelsCompleted()
    {
	    int count = 0;
	    
	    foreach (bool lvl in levelsCompleted)
	    {
		    if (lvl)
			    count++;
	    }

	    return count;
    }
    
    public void completeLevel(int index) {
	    levelsCompleted[index] = true;
    }
}
