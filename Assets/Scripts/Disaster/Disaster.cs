using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Disaster : MonoBehaviour
{
    private bool locationOne = false;
    private bool locationTwo = false;
    // Start is called before the first frame update

    // sets the location based off random number 1 - 2 returns if both spot are accupide
    public void setLocation(int numLocation){
        if(locationOne && locationTwo)
            return;
        if(numLocation == 1)
            locationOne = true;
        if(numLocation == 2)
            locationTwo = true;
        
    }

    public bool getLocationOne(){
        return locationOne;
    }

    public bool getLocationTwo(){
        return locationTwo;
    }
}
