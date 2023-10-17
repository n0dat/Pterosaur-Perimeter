using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Script is attached to all buttons in the tower drop UI.
public class TowerDropper : MonoBehaviour {
    
    //Required References
    private new Camera camera; //The camera for the scene. Make sure it is called "Main Camera" or initialization code in "Start()" will not work.
    [SerializeField] private TowerDropUIHandler m_UIHandler; //The UIHandler handles the animation for sliding in and out.
    [SerializeField] private PlayerManager m_playerManager; //For tower purchasing
    
    [SerializeField] private string m_towerName; //The name of the prefab tower you want to load upon purchase.
    private GameObject heldTower;
    private bool towerHeld;
    private bool canBePlaced;
    [SerializeField] private int m_towerCost;

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
        var obj = Instantiate(Resources.Load(m_towerName), loc, Quaternion.identity) as GameObject;
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

    private void killTower()
    {
        if (!hasAllReferences())
            return;
        
        m_playerManager.skullsCredit(m_towerCost);
        Destroy(heldTower);
        towerHeld = false;
    }

    public void purchaseTower()
    {
        //Make sure references are actually set
        if (!hasAllReferences())
            return;
        //Attempt to actually purchase the tower given the player's currency amount.
        if (!m_playerManager.skullsCost(m_towerCost)) //TODO: Flash currency counter in updateCurrency script
            return;
        
        m_UIHandler.OnMouseEnter();
        towerHeld = true;
        heldTower = instantiateTower(camera.ScreenToWorldPoint(Input.mousePosition));
    }

    private bool hasAllReferences()
    {
        if (!m_playerManager || !m_UIHandler || !camera)
        {
            Debug.Log("Missing required references in TowerDropper script.");
            return false;
        }

        return true;
    }
    
}
