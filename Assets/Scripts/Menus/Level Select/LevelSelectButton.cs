using UnityEngine;

public class LevelSelectButton : MonoBehaviour {
    [SerializeField] private string sceneToLoad;
    
    private MainManager mainManager;
    
    //Locked level logic.
    [SerializeField] private GameObject m_lockedIcon;
    [SerializeField] private AudioHandler m_audioHandler;
    private bool m_isLocked = true;

    void Start() {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        if (mainManager == null)
            Debug.Log("LevelSelectButton : Unable to get MainManager component.");
    }

    // level select button has been pressed
    public void buttonPressed() {

        // check if the level is locked
        if (m_isLocked)
        {
            m_audioHandler.playAudio("LevelLocked");
            return;
        }
        
        // load the scene associated with the level
        var sceneLoader = mainManager.getSceneManager();
        if (sceneLoader) {
            mainManager.getSceneManager().loadScene(sceneToLoad);
            mainManager.getStateManager().setGameState(StateManager.GameState.Playing);
        }
        else {
            Debug.Log("Unable to get Scene Manager");
        }
    }
    
    public void lockLevel()
    {
        m_isLocked = true;
        m_lockedIcon.SetActive(true);
    }

    public void unlockLevel()
    {
        m_isLocked = false;
        m_lockedIcon.SetActive(false);
    }
}