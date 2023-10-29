using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TackShooter : MonoBehaviour {
    // globals
    public Transform tackPrefab;
    
    public float tackSpeed = 5f;
    public int numTacks = 8;
    public float shootInterval = 1f;
    public float lastShootTime;
    public float maxTackDistance = 10f;
    public int totalTacks;
    
    public List<Transform> tacks;

    void Update() {
        if (Time.time - lastShootTime >= shootInterval) {
            lastShootTime = Time.time;
            ShootTacks();
        }

        totalTacks = tacks.Count;
        var list = new Transform[totalTacks];
        tacks.CopyTo(list);
        
        foreach (var tack in list) {
            float distanceTraveled = Vector3.Distance(transform.position, tack.transform.position);
            if (distanceTraveled >= maxTackDistance) {
                tacks.Remove(tack);
                Destroy(tack.gameObject);
            }
        }
    }

    void ShootTacks() {
        for (int i = 0; i < numTacks; i++) {
            float angle = i * 360f / numTacks;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            var tack = Instantiate(tackPrefab, transform.position, rotation);
            tacks.Add(tack);
            var rb = tack.GetComponent<Rigidbody2D>();
            rb.velocity = tack.transform.up * tackSpeed;
        }
    }


}
