using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {
    
	public MouseInputManager mouseInputManager;
    public Settings settingsManager;
    public SceneManager sceneManager;
    public StateManager stateManager;

    void Awake() {
	    mouseInputManager = GameObject.Find("MouseManager").GetComponent<MouseInputManager>();
	    settingsManager = GameObject.Find("SettingsManager").GetComponent<Settings>();
	    sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
	    stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    public MouseInputManager getMouseManager() {
	    return this.mouseInputManager;
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
}
