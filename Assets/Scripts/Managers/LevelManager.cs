using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    
    // globals
    [SerializeField] private Level currentLevel;
    
    private MainManager mainManager;

    void Start() {
        mainManager = GameObject.Find("MainManager").gameObject.GetComponent<MainManager>();
    }

    public void loadLevel(Level level) {
        currentLevel = level;
    }

    public Level getCurrentLevel() {
        return currentLevel;
    }
}
