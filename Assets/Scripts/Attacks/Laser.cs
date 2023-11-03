using System;
using System.Data;
using UnityEngine;

public class Laser : MonoBehaviour {
    
    public Vector3 direction;
    public float speed;
    public int damage;
    public bool fired;
    
    public Tower parent;
    public Time spawnTime;
    private Enemy enemyHit;

    void Awake() {
        fired = false;
    }
    
    void Update() {
        transform.position += direction * (speed * Time.deltaTime);
        Destroy(gameObject, 5f);
    }

    public void shoot(Vector3 targetDirection, Enemy hit) {
        this.direction = targetDirection;
        enemyHit = hit;
        fired = true;
        transform.eulerAngles = new Vector3(0, 0, getAngle(direction));
    }

    private void OnTriggerEnter2D(Collider2D collider2D) {
        var enemy = collider2D.GetComponent<Enemy>();
        if (enemy) {
            enemy.takeDamage(parent.getAttackDamage(), parent);
            Destroy(gameObject);
        }
    }

    private float getAngle(Vector3 dir) {
        var dest = dir.normalized;
        float n = Mathf.Atan2(dest.y, dest.x) * Mathf.Rad2Deg;
        if (n < 0)
            n += 360;
        return n;
    }
}