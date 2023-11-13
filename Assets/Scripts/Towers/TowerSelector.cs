using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TowerSelector : MonoBehaviour {
    
    [SerializeField]
    private Camera mainCamera;
    
    [SerializeField]
    private GameObject towerObj;
    
    private Tower towerRef;
    
    private MainManager mainManager;

    //Manage double clicks.
    [SerializeField] private double m_timeForDoubleClick = 0.5; //Time to allow for double click in seconds.
    private double m_timeLastClick = 0;
    

    void Awake() {
        //DontDestroyOnLoad(this.gameObject);
        //mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
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
        //if (mainManager.getStateManager().getGameState() != StateManager.GameState.Playing)
         //   return;
        
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
                    towerRef.hardDeselect();
                    towerObj = null;
                    towerRef = null;
                }
                return;
            }
            
            if (hit.collider.gameObject.CompareTag("UI"))
                return;

            if (hit.collider.gameObject.CompareTag("TowerCollider")) { // hit a Tower game object
                Debug.Log("Clicked on tower collider");
                
                if (Time.time - m_timeLastClick > m_timeForDoubleClick)
                {
                    m_timeLastClick = Time.time;
                    return;
                }
                
                if (hit.collider.transform.parent.gameObject.GetComponent<Tower>().beingHeld())
                    return;

                if (towerObj) {
                    towerRef = towerObj.gameObject.GetComponent<Tower>();
                    if (hit.collider.transform.parent.gameObject.GetInstanceID() == towerObj.GetInstanceID()) {
                        if (towerRef.isSelected())
                            towerRef.hardDeselect();
                        else
                            towerRef.select();
                        return;
                    }
                    
                    if (towerRef.isSelected())
                        towerRef.deselect();
                }
                
                
                
                towerObj = hit.collider.transform.parent.gameObject;
                towerRef = towerObj.GetComponent<Tower>();
                towerRef.select();

                m_timeLastClick = Time.time;
            }
            else {
                if (towerObj)
                    towerRef.hardDeselect();
                towerObj = null;
                towerRef = null;
            }
        }
    }
}
