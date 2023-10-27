/*

using UnityEngine;
using Random = UnityEngine.Random;

public class Disaster : MonoBehaviour {
    // globals
    
    [SerializeField] private DisasterType disasterType;
    [SerializeField] private int triggerThreshold;
    [SerializeField] private string disasterString;
    
    public bool disasterTriggered;

    void Start() {
        disasterTriggered = false;
    }

    public DisasterType getDisasterType() {
        return disasterType;
    }

    // this is used to calculate whether or not we have a disaster this round
    public bool checkDisaster() {
        return Random.Range(1, 101) < triggerThreshold;
    }

    public void triggerDisaster() {
        disasterTriggered = true;
    }
}

*/