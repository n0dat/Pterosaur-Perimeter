using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {
	[SerializeField] private MainManager mainManager;

	void Awake() {
		mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
	}

	public void pauseGame() {
		mainManager.getStateManager().pauseGame();
	}
}
