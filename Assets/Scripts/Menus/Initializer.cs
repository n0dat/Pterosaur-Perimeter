using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour {
    void Start() {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenuScene");
    }
}
