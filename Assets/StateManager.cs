using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	
	public enum GameState { Paused, Playing, Menu };
	
    [SerializeField]
    private GameState gameState = GameState.Menu;
    
    [SerializeField]
    private GameObject pauseMenu;
    
    /**
     * Called when initialized.
     *
     * Set pause menu reference to child object of this game object
     * Disable the pause menu object
     * Add self to DontDestroyOnLoad
     */
    void Awake() {
	    pauseMenu = gameObject.transform.GetChild(0).gameObject;
	    pauseMenu.SetActive(false);
	    DontDestroyOnLoad(this);
    }

    void Update() {
	    if (Input.GetKeyDown("escape")) {
		    if (gameState == GameState.Playing)
			    pauseGame();
		    else if (gameState == GameState.Paused)
			    resumeGame();
	    }
    }

    public void pauseGame() {
	    Debug.Log("Game paused");
	    gameState = GameState.Paused;
	    pauseMenu.SetActive(true);
	    Time.timeScale = 0f;
    }

    public void resumeGame() {
	    Debug.Log("Game un-paused");
	    gameState = GameState.Playing;
	    pauseMenu.SetActive(false);
	    Time.timeScale = 1f;
    }
}
