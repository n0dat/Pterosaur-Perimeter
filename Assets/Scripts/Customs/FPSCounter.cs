using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {
    
    private int frameCounter = 0;
    private float timeCounter = 0f;
    private float lastFramerate = 0f;
    private float refreshTime = 0.5f;
    private Text text;

    private void Start() {
        text = gameObject.GetComponent<Text>();
    }

    private void Update() {
        if (timeCounter < refreshTime) {
            timeCounter += Time.unscaledDeltaTime;
            frameCounter++;
        }
        else {
            lastFramerate = (float)frameCounter / timeCounter;
            frameCounter = 0;
            timeCounter = 0f;
        }
        text.text = ((int) lastFramerate).ToString();
    }
}