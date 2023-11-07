using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerTower : MonoBehaviour
{
    public float healDelay = 0.5f;
    public bool readyToHeal = true;

    public int healStren = 10;
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Tower") && readyToHeal){
            readyToHeal = false;
            StartCoroutine(healing(other.gameObject.GetComponent<Tower>()));
        }
    }

    IEnumerator healing(Tower towerToHeal){
        if(towerToHeal){
            towerToHeal.heal(healStren);
            yield return new WaitForSeconds(healDelay);
            readyToHeal = true;
        }
        yield return null;
        
    }

}
