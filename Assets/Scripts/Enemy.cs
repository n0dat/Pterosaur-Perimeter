using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour {
	
	
	[SerializeField]
	private float totalDistance = 0f, distanceCovered = 0f;
	public float health = 0f;
	public float movementSpeed = 10f;
	
	[SerializeField] private bool isDisplayingHit = false;
	
	private Vector3 targetWaypoint;
	private int waypointIndex = 0;
	public List<Vector3> waypoints;

	void Start() {
		targetWaypoint = waypoints[0];
		findTotalDistance();
		health = 100f;
	}

	void Update() {
		
		// used for targeting
		distanceCovered += (movementSpeed * Time.deltaTime) / totalDistance;
		
		Vector3 dir = targetWaypoint - transform.position;
		transform.Translate(dir.normalized * (Time.deltaTime * movementSpeed), Space.World);
		
		if (Vector3.Distance(transform.position, targetWaypoint) <= 0.6f)
			getNextCheckpoint();
	}

	void getNextCheckpoint() {
		if (waypointIndex >= waypoints.Count - 1) {
			Destroy(gameObject);
			return;
		}
		targetWaypoint = waypoints[++waypointIndex];
	}

	private void OnDestroy() {
		
		// cancel hit indication
		StopCoroutine(indicateHit());
		isDisplayingHit = false;
		
		var roundManager = GameObject.Find("RoundSpawner");
		if (roundManager != null)
			roundManager.GetComponent<RoundManager>().removeEnemy(this.gameObject.GetInstanceID());
	}

	public float getTotalDistance() {
		return totalDistance;
	}

	public float getTravelDistance() {
		return distanceCovered;
	}

	private void findTotalDistance() {
		totalDistance = 0f;
		for (int i = 0; i < waypoints.Count - 1; i++)
			totalDistance += Vector3.Distance(waypoints[i], waypoints[i + 1]);
	}

	public void takeDamage(float damage, Tower attacker) {
		
		if (gameObject == null)
			return;
		
		Debug.Log("Took damage for: " + damage);
		if (!isDisplayingHit) StartCoroutine(indicateHit());
		else {
			StopCoroutine(indicateHit());
			StartCoroutine(indicateHit());
		}
		
		if (health - damage <= 0) {
			attacker.enemyKilled();
			Destroy(gameObject);
			return;
		}
		
		health = health - damage;
	}

	private IEnumerator indicateHit() {
		gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		isDisplayingHit = true;
		
		yield return new WaitForSeconds(0.08f);
		
		gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		isDisplayingHit = false;
	}
}