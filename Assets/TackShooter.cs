using UnityEngine;

public class TackShooter : MonoBehaviour {
    // globals
    public GameObject tack;
    
    public float tackSpeed = 5f;
    public int numTacks = 8;
    public float shootInterval = 1f;
    
    public float lastShootTime;
    
    public float maxTackDistance = 10f;
    
    public int totalTacks;

    void Update() {
        if (Time.time - lastShootTime >= shootInterval) {
            lastShootTime = Time.time;
            ShootTacks();
        }

        var list = GameObject.FindGameObjectsWithTag("Tack");
        totalTacks = list.Length;
        
        foreach (var tack in list) {
            float distanceTraveled = Vector3.Distance(transform.position, tack.transform.position);
            if (distanceTraveled >= maxTackDistance)
                Destroy(tack);
        }
    }

    void ShootTacks() {
        for (int i = 0; i < numTacks; i++) {
            float angle = i * 360f / numTacks;
            
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            var tack = Instantiate(this.tack, transform.position, rotation);
            var rb = tack.GetComponent<Rigidbody2D>();
            rb.velocity = tack.transform.up * tackSpeed;
        }
    }


}
