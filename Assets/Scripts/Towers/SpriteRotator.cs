using UnityEngine;

// class that will rotate sprite to a specific Vector3 direction
public class SpriteRotator : MonoBehaviour {
    // globals
    [SerializeField] private GameObject spriteObject;
    private new SpriteRenderer renderer;
    public bool isEnabled = true;

    void Awake() {
        spriteObject = gameObject.transform.GetChild(0).gameObject;
        renderer = spriteObject.GetComponent<SpriteRenderer>();
    }

    // set direction to rotate to
    public void setDir(Vector3 dir) {
        if (!isEnabled)
            return;
        
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        // set rotation to the target direction
        spriteObject.transform.rotation = Quaternion.Slerp(spriteObject.transform.rotation, rotation, 1f);
    }

    // return the sprite renderer for the tower
    public SpriteRenderer getRenderer() {
        return renderer;
    }
}