using UnityEngine;

public class AttackType : MonoBehaviour {

    private enum AttackStyle { Single, Swipe };
    [SerializeField] private AttackStyle attackStyle = AttackStyle.Single;
    
    private float coneAngle = 45f;
    private float coneDistance = 50f;

    public void attack(GameObject sourceEnemy, GameObject sourceTower) {
        var tower = sourceTower.GetComponent<Tower>();
        var enemy = sourceEnemy.GetComponent<Enemy>();
        
        if (attackStyle == AttackStyle.Single) { // attack a single enemy every hit
            enemy.takeDamage(tower.getAttackDamage(), tower);
        }
        else if (attackStyle == AttackStyle.Swipe) { // 'slash' or 'swipe' attacking multiple enemies every hit
            Debug.Log("Swipe Started");
            coneDistance = tower.getAttackRange();
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, coneDistance);

            foreach (Collider2D collider in colliders) {
                Debug.Log("Collider");
                Vector2 directionToCollider = collider.transform.position - transform.position;
                
                //float angleToCollider = Vector2.Angle(collider.transform.position.normalized, transform.position.normalized);
                float angleToCollider = Mathf.Atan2(directionToCollider.y, directionToCollider.x) * Mathf.Rad2Deg;
                
                if (angleToCollider < 0)
                    angleToCollider += 360;
                
                if (angleToCollider <= coneAngle * 0.5f) {
                    Debug.Log("Angle to collider: " + angleToCollider);
                    Debug.Log("Attacking");
                    enemy.takeDamage(tower.getAttackDamage(), tower);
                }
            }
        }
    }
}