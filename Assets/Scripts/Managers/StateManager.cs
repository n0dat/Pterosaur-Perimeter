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
	    pauseMenu = gameObject.transform.GetChild(0).GetChild(0).gameObject;
	    pauseMenu.SetActive(false);
    }

    void Update() {
	    // check for escape key presses in different game states
	    if (Input.GetKeyDown("escape")) {
		    if (gameState == GameState.Playing)
			    pauseGame();
		    else if (gameState == GameState.Paused)
			    resumeGame();
		    else if (gameState == GameState.DisasterPrompt)
			    hideDisasterPrompt();
	    }
    }

    // pause the game
    public void pauseGame() {
	    gameState = GameState.Paused;
	    pauseMenu.SetActive(true);
	    Time.timeScale = 0f;
    }

    // un-pause the game (resume)
    public void resumeGame() {
	    gameState = GameState.Playing;
	    pauseMenu.SetActive(false);
	    Time.timeScale = 1f;
    }

    // load main menu scene and resume the game (used from pause menu button)
	public void loadMainMenu() {
		resumeGame();
		mainManager.getSceneManager().loadScene("MainMenuScene");
		gameState = GameState.Menu;
		mainManager.getSettingsManager().getNewCamera();
	}
	// sets the Game State 
    public void setGameState(GameState state) {
	    gameState = state;
    }
	// returns the gameState
    public GameState getGameState() {
	    return gameState;
    }
	// shows the disaster prompt
    public void showDisasterPrompt() {
	    gameState = GameState.DisasterPrompt;
	    Time.timeScale = 0f;
    }
	// Hides the disaster promt
    public void hideDisasterPrompt() {
	    gameState = GameState.Playing;
	    Time.timeScale = 1f;
    }
	// Sets teh gameState to an enum value of end
    public void endGame() {
	    gameState = GameState.End;
    }
	// Shows the endPromt
    public void showEndPrompt() {
	    gameState = GameState.End;
	    Time.timeScale = 0f;
    }

    // prompt at end of level Defeat or Victory
    public void hideEndPrompt(bool val) {
	    gameState = val ? GameState.Playing : GameState.Menu;
	    Time.timeScale = 1f;
    }
}
