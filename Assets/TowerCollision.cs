using UnityEngine;

public class TowerCollision : MonoBehaviour {
	public GameObject towerObj;
	public Tower tower;

	void Awake() {
		tower = towerObj.GetComponent<Tower>();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (!tower.getIsHeld())
			return;

		var col = collision.gameObject;

		if (col.CompareTag("TowerCollider") || col.CompareTag("Tower"))
			tower.setValidPosition(false);
	}
	
	void OnCollisionExit2D(Collision2D collision) {
		if (!tower.getIsHeld())
			return;
        
		var col = collision.gameObject;

		if (col.CompareTag("TowerCollider") || col.CompareTag("Tower"))
			tower.setValidPosition(true);
	}
}