using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubCaveman : MonoBehaviour
{
    [SerializeField] private float clubRadius;
    [SerializeField] private int clubDamage = 5;
    private bool isReadySwing = true;
    

    [SerializeField] private float rechargeClub = .12f;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        var circleClub = gameObject.GetComponent<CircleCollider2D>();
        circleClub.radius = clubRadius;
    }


    void OnCollisionEnter2D(Collision2D collision){

        Debug.Log("Can he swing: " + isReadySwing);
        if(collision.gameObject.CompareTag("Tower") && isReadySwing){
            // damage tower
            Debug.Log("Made Contact");
            var tower = collision.gameObject.GetComponent<Tower>();
            int health = tower.getHealth();
            Debug.Log(health);
            health -= clubDamage;
            tower.setHealth(health);
            health = tower.getHealth();
            Debug.Log(health);
            Debug.Log("The enemy was swinging : " + isReadySwing);
        }

        if(isReadySwing){
            StartCoroutine(WaitAndSwing(rechargeClub));
            Debug.Log("The enemey ability to swing: " + isReadySwing);
            return;
        }

        
    }


    IEnumerator WaitAndSwing(float rechargeClub){
        isReadySwing = false;
        yield return new WaitForSeconds(rechargeClub);
        isReadySwing = true;
        Debug.Log("The enemy ability to swing: " + isReadySwing);
        
    }
}
