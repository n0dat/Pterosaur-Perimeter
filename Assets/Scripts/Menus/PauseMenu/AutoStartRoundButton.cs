using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStartRoundButton : MonoBehaviour {
    
    // globals
    private MainManager mainManager;
    
    private bool isEnabled;
    
    [SerializeField]
    private GameObject enabledSprite, disabledSprite;

    void Start() {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        isEnabled = false;
        enabledSprite = gameObject.transform.GetChild(1).gameObject;
        disabledSprite = gameObject.transform.GetChild(2).gameObject;
        enabledSprite.SetActive(false);
    }
    
    public void onClick() {
        Debug.Log("AutoStartRoundButton: Clicked");
        if (!isEnabled) {
            mainManager.getSettingsManager().setAutoStartRounds(true);
            isEnabled = true;
            enabledSprite.SetActive(true);
            disabledSprite.SetActive(false);
        }
        else {
            mainManager.getSettingsManager().setAutoStartRounds(false);
            isEnabled = false;
            enabledSprite.SetActive(false);
            disabledSprite.SetActive(true);
        }
    }
}