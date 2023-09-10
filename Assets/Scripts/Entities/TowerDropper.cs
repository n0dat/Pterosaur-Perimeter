using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDropper : MonoBehaviour {
    
    // globals
    
    [SerializeField]
    private GameObject tower;
    
    [SerializeField]
    private GameObject heldTower;
    
    [SerializeField]
    private bool towerHeld;
    
    [SerializeField]
    private bool canBePlaced;
    
    [SerializeField]
    private string towerName;

    void Update() {
        if (towerHeld) {
            var loc = Input.mousePosition;
            loc.z = 0;
            heldTower.transform.position = loc;
            Debug.Log("Tower Position = x: " + heldTower.transform.position.x + " y: " + heldTower.transform.position.y + " z: " + heldTower.transform.position.z);

            if (Input.GetMouseButtonDown(0))
                dropTower();
        }
    }

    public GameObject instantiateTower(Vector3 loc) {
        loc.z = 0;
        var obj = Instantiate(Resources.Load(towerName), loc, Quaternion.identity) as GameObject;
        obj.transform.SetParent(null);
        return obj;
    }

    public void dropTower() {   
        // instantiate tower at mouse coordinates if it can be placed
        if (canBePlaced && towerHeld) {
            instantiateTower(Input.mousePosition);
            Destroy(heldTower);
            towerHeld = false;
        }
    }

    public void createTower() {
        towerHeld = true;
        heldTower = instantiateTower(Input.mousePosition);
    }


}
