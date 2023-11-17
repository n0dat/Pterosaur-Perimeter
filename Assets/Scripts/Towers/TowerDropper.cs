using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

//Script is attached to all buttons in the tower drop UI.
public class TowerDropper : MonoBehaviour {
    
    //Required References
    private new Camera camera; //The camera for the scene. Make sure it is called "Main Camera" or initialization code in "Start()" will not work.
    [SerializeField] private TowerDropUIHandler m_UIHandler; //The UIHandler handles the animation for sliding in and out.
    [SerializeField] private PlayerManager m_playerManager; //For tower purchasing
    
    private GameObject heldTower;
    private bool towerHeld;
    private bool canBePlaced = true;
    [SerializeField] private int m_towerCost;
    
    [SerializeField] private Transform towerToSpawn;

    void Start() {
        camera = GameObject.Find("LevelCamera").GetComponent<Camera>();
        m_playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    void Update() {
        if (towerHeld) {
            var loc = camera.ScreenToWorldPoint(Input.mousePosition);
            loc.z = 0;
            heldTower.transform.position = loc;
            
            var towerRef = heldTower.GetComponent<Tower>();
            towerRef.holdTower();
            
            if (Input.GetMouseButtonDown(0) && towerRef.isValidPosition())
                dropTower();
            
            if (Input.GetMouseButtonDown(1))
                killTower();
        }
    }

    public GameObject instantiateTower(Vector3 loc) {
        loc.z = 0;
        var obj = Instantiate(towerToSpawn, loc, Quaternion.identity);
        obj.transform.SetParent(null);
        return obj.gameObject;
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
        if (!hasAllReferences())
            return;
        
        m_playerManager.skullsCredit(m_towerCost);
        
        Destroy(heldTower);
        towerHeld = false;
    }

    public void purchaseTower() {
        //Make sure references are actually set
        if (!hasAllReferences())
            return;
        
        //Attempt to actually purchase the tower given the player's currency amount.
        if (!m_playerManager.skullsCost(m_towerCost)) 
            return;
        
        m_UIHandler.closeMenu();
        towerHeld = true;
        heldTower = instantiateTower(camera.ScreenToWorldPoint(Input.mousePosition));
    }

    private bool hasAllReferences() {
        /*
        if (!m_playerManager || !m_UIHandler || !camera) {
            Debug.Log("Missing required references in TowerDropper script.");
            return false;
        }
        */
        return true;
    }
    
}
