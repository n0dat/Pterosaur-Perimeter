using UnityEngine;

public class GameManager : MonoBehaviour {
    // globals
    private MainManager mainManager;
    
    [SerializeField] private bool[] levelsCompleted = {false, false, false, false, false};

    void Awake() {
        DontDestroyOnLoad(this);
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    public void completeLevel(int index) {
        levelsCompleted[index] = true;
    }

    public bool isLevelCompleted(int index) {
        return levelsCompleted[index];
    }
}