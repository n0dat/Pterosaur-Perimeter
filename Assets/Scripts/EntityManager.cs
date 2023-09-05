using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {
    
    public List<GameObject> objects = new List<GameObject>();
    private List<GameObject> entities = new List<GameObject>();

    void Start() {
        initEntities();
        //runEntities();
    }

    private void initEntities() {
        foreach (var obj in objects)
            entities.Add(Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity));
    }

    private void runEntities() {
        foreach (var entity in entities)
            entity.GetComponent<Entity>().startMoving();
    }
}
