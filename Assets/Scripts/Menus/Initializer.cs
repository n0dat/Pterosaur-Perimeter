using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour {
    void Awake() {
        DontDestroyOnLoad(this);
        StartCoroutine(delayStart());
    }


    IEnumerator delayStart() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test Intro Scene");
        yield return new WaitForSeconds(10f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        Destroy(this);
    }
}
