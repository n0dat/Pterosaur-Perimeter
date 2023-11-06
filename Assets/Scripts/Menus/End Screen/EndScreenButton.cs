using UnityEngine;

public class EndScreenButton : MonoBehaviour {
    // globals
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private MainManager mainManager;
    [SerializeField] private string sceneName;

    void Awake() {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void restartLevel() {
        mainManager.getSceneManager().loadScene("sceneName");
        levelManager.resetLevel();
        mainManager.getStateManager().hideEndPrompt(true);
    }

    public void exit(bool state) {
        mainManager.getStateManager().hideEndPrompt(false);
        mainManager.getStateManager().setGameState(StateManager.GameState.Menu);
        mainManager.getSceneManager().loadScene("LevelSelectScene");
    }
}