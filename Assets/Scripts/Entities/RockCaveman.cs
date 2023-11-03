using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCaveman : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private float rockRadius;
    [SerializeField] private float attackDamage = 8.0f;
    private bool isReadyThrow = true;
    [SerializeField] private float rechargeThrow = .18f;
    

    // Update is called once per frame2.
    void Update()
    {
        var rockThrower = gameObject.GetComponent<CircleCollider2D>();
        rockThrower.radius = rockRadius;
    }

    void OnCollisionEnter2D(Collision2D collision){
       
        Debug.Log("Can he throw: " + isReadyThrow);
        if(collision.gameObject.CompareTag("Tower") && isReadyThrow){
            // damage tower
            var tower = collision.gameObject.GetComponent<Tower>();
            float health = tower.getDurability();
            Debug.Log(health);
            health -= attackDamage;
            tower.setDurability(health);
            health = tower.getDurability();
            Debug.Log(health);

            Debug.Log("Made Contact");
            Debug.Log("The enemy has thrown : " + isReadyThrow);
            
        }

         if(isReadyThrow){
            StartCoroutine(WaitAndSwing(rechargeThrow));
            return;
        }
    }


    IEnumerator WaitAndSwing(float rechargeThrow){
        isReadyThrow = false;
        yield return new WaitForSeconds(rechargeThrow);
        isReadyThrow = true;
        Debug.Log("The caveman reloading throw: " + isReadyThrow);
    }
}
