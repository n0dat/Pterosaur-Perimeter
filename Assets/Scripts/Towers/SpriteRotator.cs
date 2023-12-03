using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour {
    // globals
    [SerializeField] private GameObject spriteObject;
    private new SpriteRenderer renderer;
    public bool enabled = true;

    void Awake() {
        spriteObject = gameObject.transform.GetChild(0).gameObject;
        renderer = spriteObject.GetComponent<SpriteRenderer>();
    }

    public void setDir(Vector3 dir) {
        if (!enabled)
            return;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        spriteObject.transform.rotation = Quaternion.Slerp(spriteObject.transform.rotation, rotation, 1f);
    }

    public SpriteRenderer getRenderer() {
        return renderer;
    }
}