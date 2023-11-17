using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeMenuClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private TowerSelector towerSelector;

	void Awake() {
		towerSelector = GameObject.Find("TowerContainer").GetComponent<TowerSelector>();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		towerSelector.checkForClicks = false;
		Debug.Log("Check for clicks: " + towerSelector.checkForClicks);
	}

	public void OnPointerExit(PointerEventData eventData) {
		towerSelector.checkForClicks = true;
		Debug.Log("Check for clicks: " + towerSelector.checkForClicks);
	}

	public void OnMouseEnter() {
		towerSelector.checkForClicks = false;
		Debug.Log("Check for clicks: " + towerSelector.checkForClicks);
	}

	public void OnMouseExit() {
		towerSelector.checkForClicks = true;
		Debug.Log("Check for clicks: " + towerSelector.checkForClicks);
	}
}