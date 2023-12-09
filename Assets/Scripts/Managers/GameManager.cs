using UnityEngine;

public class GameManager : MonoBehaviour {
    // globals
    private MainManager mainManager;
    
    [SerializeField] private bool[] levelsCompleted = {false, false, false, false, false};

    void Awake() {
        DontDestroyOnLoad(this);
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    // Changes the index of the levels complete to true one level is complete
    public void completeLevel(int index) {
        levelsCompleted[index] = true;
    }
    // return a boolean to check if that level is complete
    // checks by passing an index
    public bool isLevelCompleted(int index) {
        return levelsCompleted[index];
    }
}