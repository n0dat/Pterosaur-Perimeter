using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    
    public float attackDelay = 0.5f;
    public bool readyToAttack = true;
    public int damage = 20;

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Tower") && readyToAttack) {
            readyToAttack = false;
            StartCoroutine(attack(other.gameObject.GetComponent<Tower>()));
        }
    }

    IEnumerator attack(Tower towerToAttack) {
        if (towerToAttack) {
            // TODO: trigger attack animation for the caveman here
            towerToAttack.takeDamage(damage);
            yield return new WaitForSeconds(attackDelay);
            readyToAttack = true;
        }
        
        yield return null;
    }
}