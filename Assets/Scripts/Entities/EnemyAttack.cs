using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    
    [SerializeField] private Enemy m_enemy;
    public float attackDelay = 1.5f;
    public bool readyToAttack = true;
    public int damage = 5;
    public float range = 20f;
    [SerializeField] private bool isAttackingEnemy = false;

    void Update() {
        
        // ready to attack
        if (isAttackingEnemy && readyToAttack) {
            var towers = new List<Tower>();
            
            // find towers in attack radius
            foreach (var tower in Physics2D.OverlapCircleAll(transform.position, range))
                if (tower.gameObject.CompareTag("TowerCollider"))
                    towers.Add(tower.transform.parent.gameObject.GetComponent<Tower>());

            // detected some towers
            if (towers.Count > 0) {
                var randTower = towers[Random.Range(0, towers.Count)];
                if (randTower.getIsHeld())
                    return;
                
                // attack a random tower within the attack radius
                StartCoroutine(attack(randTower));
            }
        }
    }

    // attack tower
    IEnumerator attack(Tower towerToAttack) {
        if (towerToAttack) {
            
            // do not attack a tower that is held (being placed)
            if (towerToAttack.getIsHeld())
                yield return null;
            
            readyToAttack = false;
            m_enemy.attack();
            towerToAttack.takeDamage(damage);
            yield return new WaitForSeconds(attackDelay);
            readyToAttack = true;
        }
        
        yield return null;
    }
}
