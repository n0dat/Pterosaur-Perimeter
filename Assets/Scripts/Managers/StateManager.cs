using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	
	public enum GameState { Paused, Playing, Menu, Settings, DisasterPrompt, End };
	
    [SerializeField] private GameState gameState;
    [SerializeField] private GameObject pauseMenu;

	private MainManager mainManager;
    
    void Awake() {
		mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
		
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
		    else if (gameState == GameState.DisasterPrompt)
			    hideDisasterPrompt();
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

	public void loadMainMenu() {
		resumeGame();
		mainManager.getSceneManager().loadScene("MainMenuScene");
		gameState = GameState.Menu;
	}

    public void setGameState(GameState state) {
	    gameState = state;
    }

    public GameState getGameState() {
	    return gameState;
    }

    public void showDisasterPrompt() {
	    gameState = GameState.DisasterPrompt;
	    Time.timeScale = 0f;
    }

    public void hideDisasterPrompt() {
	    gameState = GameState.Playing;
	    Time.timeScale = 1f;
    }

    public void endGame() {
	    gameState = GameState.End;
    }

    public void showEndPrompt() {
	    gameState = GameState.End;
	    Time.timeScale = 0f;
    }

    public void hideEndPrompt(bool val) {
	    if (val) {
		    gameState = GameState.Playing;
		    Time.timeScale = 1f;
	    }
	    else {
		    gameState = GameState.Menu;
		    Time.timeScale = 1f;
	    }
    }
}
