using UnityEngine;

public class EndScreenButton : MonoBehaviour {
    // globals
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private MainManager mainManager;

    void Awake() {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void restartLevel() {
        levelManager.resetLevel();
        mainManager.getStateManager().hideEndPrompt(true);
    }

    public void exit(bool state) {
        mainManager.getStateManager().hideEndPrompt(false);
        mainManager.getStateManager().setGameState(StateManager.GameState.Menu);
        mainManager.getSceneManager().loadScene("LevelSelectScene");
    }
}