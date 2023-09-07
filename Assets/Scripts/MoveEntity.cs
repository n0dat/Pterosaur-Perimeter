using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEntity : MonoBehaviour {
    
    private MapCheckpoints targets;
    private bool returnToInitialStartPosition;
    private bool initialized;
    
    public float movementSpeed;
    public Vector3 initialPosition;
    public bool readyToMove;
    

    void Start() {
        targets = new MapCheckpoints();
        targets.add(new Vector3((float) -4.5, (float) -3.5, (float) 0));
        targets.add(new Vector3((float) -4.5, (float)  3.5, (float) 0));
        targets.add(new Vector3((float)  4.5, (float)  3.5, (float) 0));
        targets.add(new Vector3((float)  4.5, (float) -3.5, (float) 0));
        
        movementSpeed = 3.0f;
        readyToMove = false;
        
        initialPosition = transform.position;
        
        returnToInitialStartPosition = false;
        //initialized = true;
        
        go();
    }

    void Update() {
        if (readyToMove) {
            readyToMove = false;
            targets.reset();
            StartCoroutine(MoveToTargets());
        }
    }

    public void go() {
        StartCoroutine(MoveToTargets());
    }

    public void setCheckpoints(MapCheckpoints mapCheckpoints) {
        this.targets = mapCheckpoints;
    }
    
    private IEnumerator MoveToTargets() {
        while (targets.hasNext()) {
            float startTime = Time.time;
            var startPosition = transform.position;
            var endPosition = targets.next();

            while (Time.time - startTime < movementSpeed) {
                transform.position = Vector3.Lerp(startPosition, endPosition, (Time.time - startTime) / movementSpeed);
                yield return null;
            }
            
            transform.position = endPosition;
        }

        if (returnToInitialStartPosition) {
            float returnStartTime = Time.time;
            while (Time.time - returnStartTime < movementSpeed) {
                transform.position = Vector3.Lerp(targets.last(), initialPosition, (Time.time - returnStartTime) / movementSpeed);
                yield return null;
            }
        }
        
        readyToMove = true;
    }
}
