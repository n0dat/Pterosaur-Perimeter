using UnityEngine;

public class MainManager : MonoBehaviour {
    
    private Settings settingsManager;
    private SceneManager sceneManager;
    private StateManager stateManager;
    private PlayerManager playerManager;
    
    // array to store which levels have been completed
    private bool[] levelsCompleted = {false, false, false, false, false};

    void Awake() {
	    DontDestroyOnLoad(this);
	    
	    settingsManager = GameObject.Find("SettingsManager").GetComponent<Settings>();
	    sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
	    stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    public Settings getSettingsManager() {
	    return settingsManager;
    }

    public SceneManager getSceneManager() {
	    return sceneManager;
    }

    public StateManager getStateManager() {
	    return stateManager;
    }

    // get the number of levels that have been completed
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
    
    // mark a level as completed
    public void completeLevel(int index) {
	    levelsCompleted[index] = true;
    }
}
