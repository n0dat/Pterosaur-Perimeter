using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelector : MonoBehaviour {
    
    [SerializeField]
    private Camera mainCamera;
    
    [SerializeField]
    private GameObject towerObj;
    
    private Tower towerRef;
    
    private MainManager mainManager;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        mainCamera = null;
        getNewCamera();
    }

    public void getNewCamera() {
        StartCoroutine(GetCamera());
    }

    private IEnumerator GetCamera() {
        while (!mainCamera) {
            mainCamera = Camera.main;
            yield return null;
        }
        
        Debug.Log("Camera found");
    }

    void Update() {
        if (mainManager.getStateManager().getGameState() != StateManager.GameState.Playing)
            return;
        
        if (Input.GetMouseButtonDown(0)) {
            if (!mainCamera) {
                getNewCamera();
                return;
            }
            
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            var hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (!hit.collider) {
                if (towerObj) {
                    towerRef.deselect();
                    towerObj = null;
                    towerRef = null;
                }
                return;
            }

            if (hit.collider.gameObject.CompareTag("Tower")) { // hit a Tower game object
                if (hit.collider.gameObject.GetComponent<Tower>().beingHeld())
                    return;

                if (towerObj) {
                    towerRef = towerObj.gameObject.GetComponent<Tower>();
                    if (hit.collider.gameObject.GetInstanceID() == towerObj.GetInstanceID()) {
                        if (towerRef.isSelected())
                            towerRef.deselect();
                        else
                            towerRef.select();
                        return;
                    }
                    
                    if (towerRef.isSelected())
                        towerRef.deselect();
                }
                
                towerObj = hit.collider.gameObject;
                towerRef = towerObj.GetComponent<Tower>();
                towerRef.select();
            }
            else {
                if (towerObj)
                    towerRef.deselect();
                towerObj = null;
                towerRef = null;
            }
        }
    }
}
