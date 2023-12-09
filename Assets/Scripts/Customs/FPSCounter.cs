using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {
    
    private int frameCounter = 0;
    private float timeCounter = 0f;
    private float lastFramerate = 0f;
    private float refreshTime = 0.5f;
    private Text text;

    // get text component
    private void Start() {
        text = gameObject.GetComponent<Text>();
    }

    private void Update() {
        
        // calculate time between previous frame to get fps
        if (timeCounter < refreshTime) {
            timeCounter += Time.unscaledDeltaTime;
            frameCounter++;
        }
        else {
            lastFramerate = frameCounter / timeCounter;
            frameCounter = 0;
            timeCounter = 0f;
        }
        text.text = ((int) lastFramerate).ToString();
    }
}