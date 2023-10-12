using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour {

    // globals
    [SerializeField] private int levelIndex;

    [SerializeField] private LevelType levelType;
    
    [SerializeField] private int startingCurrency;
    
    [SerializeField] private Disaster disaster;

    public LevelType getLevelType() {
        return levelType;
    }

    public int getLevelIndex() {
        return levelIndex;
    }

    public DisasterType getDisasterType() {
        return disaster.getDisasterType();
    }

    public int getStartingCurrency() {
        return startingCurrency;
    }
}