using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDropper : MonoBehaviour {
    
    // globals
    
    [SerializeField]
    private new Camera camera;
    
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

    void Start() {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update() {
        if (towerHeld) {
            var loc = camera.ScreenToWorldPoint(Input.mousePosition);
            loc.z = 0;
            heldTower.transform.position = loc;
            heldTower.GetComponent<Tower>().holdTower();

            if (Input.GetMouseButtonDown(0) && heldTower.GetComponent<Tower>().isValidPosition())
                dropTower();
            
            if (Input.GetMouseButtonDown(1))
                killTower();
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
            instantiateTower(camera.ScreenToWorldPoint(Input.mousePosition));
            Destroy(heldTower);
            towerHeld = false;
        }
    }

    private void killTower() {
        Destroy(heldTower);
        towerHeld = false;
    }

    public void createTower() {
        towerHeld = true;
        heldTower = instantiateTower(camera.ScreenToWorldPoint(Input.mousePosition));
    }
}
