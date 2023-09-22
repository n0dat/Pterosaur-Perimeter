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
                directionToCollider = Quaternion.Inverse(transform.rotation) * directionToCollider;

                // time for some math
                var rotationAngle = transform.position.z;
                if (rotationAngle < 0)
                    rotationAngle += 360;
                
                Vector2 target = new Vector2(coneDistance * Mathf.Sin(rotationAngle), coneDistance * Mathf.Cos(rotationAngle));
                Vector2 tempVec = transform.position.normalized * coneDistance;
                var distance = Vector2.Distance(transform.position, collider.transform.position);
                Vector2 tempVecCol = collider.transform.position.normalized * distance;
                float angleToCollider = Vector2.Angle(collider.transform.position.normalized, transform.position.normalized);
                
                if (angleToCollider <= coneAngle * 0.5f && directionToCollider.magnitude <= coneDistance) {
                    Debug.Log("Angle to collider: " + angleToCollider);
                    Debug.Log("Attacking");
                    enemy.takeDamage(tower.getAttackDamage(), tower);
                }
            }
        }
    }
}