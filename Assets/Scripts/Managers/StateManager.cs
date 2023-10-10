using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	
	public enum GameState { Paused, Playing, Menu, Settings };
	
    [SerializeField]
    private GameState gameState;
    
    [SerializeField]
    private GameObject pauseMenu;
    
    void Awake() {
	    DontDestroyOnLoad(this);
	    
	    gameState = GameState.Menu;
	    pauseMenu = gameObject.transform.GetChild(0).gameObject;
	    pauseMenu.SetActive(false);
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
	    gameState = GameState.Paused;
	    pauseMenu.SetActive(true);
	    Time.timeScale = 0f;
    }

    public void resumeGame() {
	    gameState = GameState.Playing;
	    pauseMenu.SetActive(false);
	    Time.timeScale = 1f;
    }

    public void setGameState(GameState state) {
	    gameState = state;
    }

    public GameState getGameState() {
	    return gameState;
    }
}
