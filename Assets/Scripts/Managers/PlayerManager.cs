using UnityEngine;

public class PlayerManager : MonoBehaviour {
    
    // globals
    private MainManager mainManager;
    
    [SerializeField]
    private int skulls;

    void Start() {
        mainManager = GameObject.Find("MainManager").gameObject.GetComponent<MainManager>();
        
        skulls = 0;
    }

    public void readLevelData(Level level) {
        
    }
}