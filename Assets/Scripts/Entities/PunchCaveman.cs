using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PunchCaveman : MonoBehaviour
{
    // Punch caveman ridgetbody
    
    [SerializeField] private float radius;
    [SerializeField] private int attackDamage = 1;
    private bool isReadyPunch = true;
    
    
    [SerializeField] private float rechargeHit = .05f;



    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        var circle = gameObject.GetComponent<CircleCollider2D>();
        //colid(circle);
        circle.radius = radius;
    }


    void OnCollisionEnter2D(Collision2D collider){
        Debug.Log("Can he punch: " + isReadyPunch);
        if(collider.gameObject.CompareTag("Tower") && isReadyPunch){
            Debug.Log("Made Contact");            
            var tower = collider.gameObject.GetComponent<Tower>();
            int health = tower.getHealth();
            Debug.Log(health);
            health -= attackDamage;
            tower.setHealth(health);
            health = tower.getHealth();
            Debug.Log(health);
            Debug.Log("The enemy punched : " + isReadyPunch);

        }

        if(isReadyPunch){
            //
            StartCoroutine(WaitAndCharge(rechargeHit));
            
            return;
        }
        
    }

    IEnumerator WaitAndCharge(float rechargeHit){
        isReadyPunch = false;
        yield return new WaitForSeconds(rechargeHit);
        isReadyPunch = true;
        Debug.Log("The enemey ability to punch: " + isReadyPunch);
        
    }

    

}
