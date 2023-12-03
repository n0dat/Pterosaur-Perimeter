using UnityEngine;

public class TowerCollision : MonoBehaviour {
	public GameObject towerObj;
	public Tower tower;

	void Awake() {
		tower = towerObj.GetComponent<Tower>();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (!tower.getIsHeld()) {
			Debug.Log("Tower is not held : " + towerObj.GetInstanceID());
			return;
		}
		
		var col = collision.gameObject;

		if (col.CompareTag("TowerCollider")) {
			Debug.Log("Started colliding with a tower");
			tower.setValidPosition(false);
		}
	}
	
	private void OnCollisionExit2D(Collision2D collision) {
		if (!tower.getIsHeld()) {
			Debug.Log("Tower is not held : " + towerObj.GetInstanceID());
			return;
		}
        
		var col = collision.gameObject;

		if (col.CompareTag("TowerCollider")) {
			Debug.Log("Stopped colliding with a tower");
			tower.setValidPosition(true);
		}
	}
}