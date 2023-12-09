using UnityEngine;

// class to disallow towers to be placed on top of one another
public class TowerCollision : MonoBehaviour {
	public Tower tower;

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