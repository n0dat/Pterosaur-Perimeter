using System.Collections;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour {
	
	public MainManager mainManager;
	public Camera mainCamera;
	public Canvas pauseMenuCanvas;

	void Update() {
		if (mainManager.getStateManager().getGameState() == StateManager.GameState.Playing && !mainCamera)
			getNewCamera();
	}

	public void getNewCamera() {
		StartCoroutine(GetCamera());
	}

	private IEnumerator GetCamera() {
		while (!mainCamera) {
			mainCamera = Camera.main;
			pauseMenuCanvas.worldCamera = mainCamera;
			pauseMenuCanvas.sortingLayerName = "UI_Top";
			break;
		}
        
		Debug.Log("PauseMenuHandler : Camera found");
		yield return null;
	}
}
