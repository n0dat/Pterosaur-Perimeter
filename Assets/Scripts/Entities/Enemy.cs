using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public enum FacingDirection { Up, Down, Left, Right, Unknown }

	public enum EnemyType { Standard, SingleClub, DoubleClub, Rock }

	// movement
	[SerializeField] private float totalDistance = 0f, distanceCovered = 0f;
	public Vector3 targetWaypoint;
	public int waypointIndex = 0;
	public List<Vector3> waypoints;
	public float nextCheckpointThreshold = 0.2f;
	public FacingDirection facingDirection = FacingDirection.Down;
	public float facingThreshold = 10f;
	public float rotationSpeed = 50f;
	public Quaternion targetRotation;
	public EnemyType enemyType = EnemyType.SingleClub;
	public float changeThreshold = 0.15f;

	// ui
	public LevelManager levelManager;
	private SpriteRenderer spriteRenderer;
	private bool isDisplayingHit = false;
	public float hitDelay = 0.2f;
	public GameObject sprite;
	[SerializeField] private HealthUIHandler m_healthBarHandler;
	
	// other
	public float health = 100f;
	public float movementSpeed = 10f;
	private PlayerManager m_playerManager;
	public int killReward = 50;
	
	//Audio and animation
	[SerializeField] private EnemyAudioSpawner m_audioHandler;
	[SerializeField] private Animator m_animator;
	
	void Awake() {
		setChangeThreshold();
		findTotalDistance();
		
		m_playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		spriteRenderer = sprite.GetComponent<SpriteRenderer>();
	}

	void Update() {
		// used for targeting
		distanceCovered += (movementSpeed * Time.deltaTime) / totalDistance;
		
		Vector3 dir = targetWaypoint - transform.position;
		transform.Translate(dir.normalized * (Time.deltaTime * movementSpeed), Space.World);
		
		sprite.transform.rotation = Quaternion.RotateTowards(sprite.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		
		if (Vector3.Distance(transform.position, targetWaypoint) <= nextCheckpointThreshold) {
			getNextCheckpoint();
		}
	}

	public void setWaypoints(List<Vector3> points) {
		waypoints = points;
		targetWaypoint = waypoints[0];
	}

	private void getNextCheckpoint() {
		//Reached the last checkpoint. Decrease player health.
		if (waypointIndex >= waypoints.Count - 1) {
			m_playerManager.hit();
			m_audioHandler.eggSteal();
			Destroy(gameObject);
			return;
		}
		targetWaypoint = waypoints[++waypointIndex];
		facingDirection = getFacingDirection(targetWaypoint);
		updateRotation(targetWaypoint);
	}

	private void OnDestroy() {
		// cancel hit indication
		if (isDisplayingHit) {
			StopCoroutine(indicateHit());
			isDisplayingHit = false;
		}
		
		if (levelManager)
			levelManager.removeEnemy(gameObject.GetInstanceID());
		
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
		if (!gameObject)
			return;
		
		if (!isDisplayingHit)
			StartCoroutine(indicateHit());
		else {
			StopCoroutine(indicateHit());
			StartCoroutine(indicateHit());
		}
		
		health -= damage;
		m_audioHandler.damage();
		
		m_healthBarHandler.setHealth((int)health);

		if (health > 0) return;
		
		Debug.Log("Enemy killed");
		attacker.enemyKilled();
		m_playerManager.skullsCredit(killReward);
		m_audioHandler.death();
		Destroy(gameObject);
	}

	private IEnumerator indicateHit() {
		spriteRenderer.color = Color.red;
		isDisplayingHit = true;
		
		yield return new WaitForSeconds(hitDelay);
		
		spriteRenderer.color = Color.white;
		isDisplayingHit = false;
	}

	private FacingDirection getFacingDirection(Vector3 target) {
		var angle = Vector3.Angle(transform.up, (target - transform.position));
		if (Mathf.Abs(angle - 0f) < facingThreshold)
			return FacingDirection.Up;
		if (Mathf.Abs(angle - 90f) < facingThreshold)
			return FacingDirection.Left;
		if (Mathf.Abs(angle - 180f) < facingThreshold)
			return FacingDirection.Down;
		if (Mathf.Abs(angle - 270f) < facingThreshold)
			return FacingDirection.Right;
		return FacingDirection.Unknown;
	}

	private void updateRotation(Vector3 target) {
		Vector3 direction = target - sprite.transform.position;
		var angle = Mathf.Atan2(direction.y, direction.x);
		var angleDegrees = angle * Mathf.Rad2Deg + -90f;
		targetRotation = Quaternion.Euler(0, 0, angleDegrees);
	}

	//Handles animation and sound.
	public void attack() {
		if (enemyType == EnemyType.Standard)
			return;
		
		m_animator.SetTrigger("hit");
		m_audioHandler.hit();
		m_audioHandler.quip();
	}

	private void setChangeThreshold() {
		if (enemyType == EnemyType.Standard) {
			changeThreshold = 0.18f;
			movementSpeed = 24f;
		}
		if (enemyType == EnemyType.SingleClub) {
			changeThreshold = 0.22f;
			movementSpeed = 26f;
		}
		if (enemyType == EnemyType.DoubleClub) {
			changeThreshold = 0.25f;
			movementSpeed = 28f;
		}
		if (enemyType == EnemyType.Rock) {
			changeThreshold = 0.3f;
			movementSpeed = 30f;
		}
	}
}