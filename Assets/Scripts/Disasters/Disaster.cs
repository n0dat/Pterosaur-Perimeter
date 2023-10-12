using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Disaster : MonoBehaviour {
    // globals
    
    [SerializeField] private DisasterType disasterType;
    [SerializeField] private int triggerThreshold;
    
    private bool disasterTriggered;

    void Start() {
        disasterTriggered = false;
    }

    public DisasterType getDisasterType() {
        return disasterType;
    }

    // this is used to calculate whether or not we have a disaster this round
    public bool checkDisaster() {
        var chance = Random.Range(1, 101);
        
        if (chance < triggerThreshold)
            return true;
        
        return false;
        
    }

    public void triggerDisaster() {
        disasterTriggered = true;
    }
}