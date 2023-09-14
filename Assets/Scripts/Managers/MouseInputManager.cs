using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : MonoBehaviour {
    
    [SerializeField]
    private Camera mainCamera;
    
    [SerializeField]
    private GameObject towerObj;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        getNewCamera();
    }

    public void getNewCamera() {
        StartCoroutine(GetCamera());
    }

    private IEnumerator GetCamera() {
        while (mainCamera == null) {
            mainCamera = Camera.main;
            
            yield return null;
        }
        
        Debug.Log("Camera found");
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            var hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) { // game object hit
                
                if (hit.collider.gameObject.GetComponent<Tower>() != null) { // hit a Tower game object
                    if (hit.collider.gameObject.GetComponent<Tower>().beingHeld())
                        return;
                    
                    if (towerObj != null && hit.collider.gameObject.GetInstanceID() == towerObj.GetInstanceID()) {
                        if (towerObj.GetComponent<Tower>().isSelected())
                            towerObj.GetComponent<Tower>().deselect();
                        else
                            towerObj.GetComponent<Tower>().select();
                        return;
                    }
                    
                    if (towerObj != null)
                        if (towerObj.GetComponent<Tower>().isSelected())
                            towerObj.GetComponent<Tower>().deselect();
                    
                    towerObj = hit.collider.gameObject;
                    towerObj.GetComponent<Tower>().select();
                }
                else {
                    if (towerObj != null)
                        towerObj.GetComponent<Tower>().deselect();
                    towerObj = null;
                }
            }
            else {
                if (towerObj != null) {
                    towerObj.GetComponent<Tower>().deselect();
                    towerObj = null;
                    return;
                }
            }
        }
    }
    
}
