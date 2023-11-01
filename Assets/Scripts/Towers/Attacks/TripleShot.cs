using System.Collections.Generic;
using UnityEngine;

public class TripleShotShooter : MonoBehaviour {
    public Transform tackPrefab;
    public float tackSpeed = 5f;
    public int numTacks = 3;
    public float shootInterval = 1f;
    public float lastShootTime;
    public float maxTackDistance = 10f;
    public int totalTacks;
    public int attackAngleStep = 15;
    
    Vector2 mousePosition;
    Vector2 towerPosition;
    Vector2 attackDirection;
    
    public List<Transform> tacks;
    
    void Update() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        towerPosition = transform.position;
        attackDirection = (mousePosition - towerPosition).normalized;
        
        if (Time.time - lastShootTime >= shootInterval) {
            lastShootTime = Time.time;
            ShootTacks();
        }
        
        totalTacks = tacks.Count;
        Transform[] list = new Transform[totalTacks];
        tacks.CopyTo(list);

        foreach (var tack in list) {
            float distanceTraveled = Vector3.Distance(transform.position, tack.transform.position);
            if (distanceTraveled >= maxTackDistance) {
                tacks.Remove(tack);
                Destroy(tack.gameObject);
            }
        }
    }

    void ShootTacks() {
        // spawn one tack at each -attackAngleStep, 0, attackAngleStep for a total of three tacks
        for (int i = -(attackAngleStep); i <= attackAngleStep; i += attackAngleStep) {
            var tack = Instantiate(tackPrefab, transform.position, Quaternion.identity);
            tacks.Add(tack);
            var angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg + i;
            tack.transform.eulerAngles = new Vector3(0f, 0f, angle);
            tack.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0f, 0f, angle) * Vector2.right * tackSpeed;            
        }
    }
}