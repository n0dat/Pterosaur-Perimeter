using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    // globals
    [SerializeField] private int levelIndex;
    [SerializeField] private LevelType levelType;
    [SerializeField] private int startingCurrency;
    [SerializeField] private Disaster disaster;
    [SerializeField] private List<Vector3> checkpoints;
    [SerializeField] private GameObject checkpointObject;
    [SerializeField] private string levelName;
    
    public RoundState roundState = RoundState.Waiting;
    public Round[] rounds;
    public int currentRound = 0;

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

    public void loadCheckpoints() {
        checkpoints.Clear();
        foreach (Transform checkpoint in checkpointObject.transform) {
            var checkpointPos = checkpoint.gameObject.transform.position;
            checkpoints.Add(new Vector3(checkpointPos.x, checkpointPos.y, 0f));
        }
    }

    public List<Vector3> getCheckpoints() {
        return checkpoints;
    }
}