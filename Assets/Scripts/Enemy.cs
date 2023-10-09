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
	private List<Vector3> waypoints;

	private RoundManager roundManager;
	private SpriteRenderer spriteRenderer;
	private static readonly int MColor = Shader.PropertyToID("m_Color");

	void Start() {
		targetWaypoint = waypoints[0];
		findTotalDistance();
		health = 100f;
		roundManager = GameObject.Find("RoundSpawner")?.GetComponent<RoundManager>();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}

	void Update() {
		
		// used for targeting
		distanceCovered += (movementSpeed * Time.deltaTime) / totalDistance;
		
		Vector3 dir = targetWaypoint - transform.position;
		transform.Translate(dir.normalized * (Time.deltaTime * movementSpeed), Space.World);

		if (Vector3.Distance(transform.position, targetWaypoint) <= 0.15f) {
			getNextCheckpoint();
		}
	}

	public void setWaypoints(List<Vector3> points) {
		waypoints = points;
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
		if (isDisplayingHit) {
			StopCoroutine(indicateHit());
			isDisplayingHit = false;
		}
		
		if (roundManager != null)
			roundManager.removeEnemy(this.gameObject.GetInstanceID());
	}

	public float getTotalDistance() {
		return totalDistance;
	}

	public float getTravelDistance() {
		return distanceCovered;
	}

	private void findTotalDistance() {
		totalDistance = 0f;
		int count = waypoints.Count - 1;
		for (int i = 0; i < count; i++)
			totalDistance += Vector3.Distance(waypoints[i], waypoints[i + 1]);
	}

	public void takeDamage(float damage, Tower attacker) {
		
		if (gameObject == null || health <= 0f)
			return;
		
		if (!isDisplayingHit)
			StartCoroutine(indicateHit());
		else {
			StopCoroutine(indicateHit());
			StartCoroutine(indicateHit());
		}
		
		health -= damage;
		
		if (health <= 0) {
			attacker.enemyKilled();
			Destroy(gameObject);
		}
	}

	private IEnumerator indicateHit() {
		spriteRenderer.color = Color.red;
		isDisplayingHit = true;
		
		yield return new WaitForSeconds(0.1f);
		
		spriteRenderer.color = Color.white;
		isDisplayingHit = false;
	}
}