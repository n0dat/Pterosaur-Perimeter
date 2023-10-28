using UnityEngine;

public class TripleShotShooter : MonoBehaviour {
    public GameObject tackPrefab;
    public float tackSpeed = 5f;
    public int numTacks = 3;
    public float shootInterval = 1f;

    public float lastShootTime;
    
    public float maxTackDistance = 10f;
    
    public int totalTacks;
    
    Vector2 mousePosition;
    Vector2 towerPosition;
    Vector2 attackDirection;
    
    void Update() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        towerPosition = transform.position;
        attackDirection = (mousePosition - towerPosition).normalized;
        
        if (Time.time - lastShootTime >= shootInterval) {
            lastShootTime = Time.time;
            ShootTacks();
        }
        
        var list = GameObject.FindGameObjectsWithTag("Tack2");
        totalTacks = list.Length;

        foreach (var tack in list) {
            float distanceTraveled = Vector3.Distance(transform.position, tack.transform.position);
            if (distanceTraveled >= maxTackDistance)
                Destroy(tack);
        }
    }

    void ShootTacks() {

        for (int i = -15; i <= 15; i += 15) {
            var tack = Instantiate(tackPrefab, transform.position, Quaternion.identity);
            var angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg + i;
            tack.transform.eulerAngles = new Vector3(0f, 0f, angle);
            tack.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0f, 0f, angle) * Vector2.right * tackSpeed;            
        }
        /*
        float angle1 = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg - 15f;
        float angle2 = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        float angle3 = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg + 15f;
        
        //Quaternion rotation1 = Quaternion.Euler(0f, 0f, angle1);
        //Quaternion rotation2 = Quaternion.Euler(0f, 0f, angle2);
        //Quaternion rotation3 = Quaternion.Euler(0f, 0f, angle3);
        
        var rotation1 = Quaternion.AngleAxis(angle1, Vector3.forward);
        var rotation2 = Quaternion.AngleAxis(angle2, Vector3.forward);
        var rotation3 = Quaternion.AngleAxis(angle3, Vector3.forward);
        
        GameObject tack1 = Instantiate(tackPrefab, transform.position, rotation1);
        GameObject tack2 = Instantiate(tackPrefab, transform.position, rotation2);
        GameObject tack3 = Instantiate(tackPrefab, transform.position, rotation3);
        
        tack1.transform.rotation = Quaternion.Slerp(tack1.transform.rotation, rotation1, 1f);
        
        var targetDir1 = new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1));
        var targetDir2 = new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2));
        var targetDir3 = new Vector2(Mathf.Cos(angle3), Mathf.Sin(angle3));
        
        Rigidbody2D rb1 = tack1.GetComponent<Rigidbody2D>();
        rb1.velocity = targetDir1 * tackSpeed;
        
        Rigidbody2D rb2 = tack2.GetComponent<Rigidbody2D>();
        rb2.velocity = targetDir2 * tackSpeed;
        
        Rigidbody2D rb3 = tack3.GetComponent<Rigidbody2D>();
        rb3.velocity = targetDir3 * tackSpeed;
        
        
        // for (int i = 0; i < numTacks; i++) {
        //     float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg + i * 15f - 15f;
        //     Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        //     GameObject tack = Instantiate(tackPrefab, transform.position, rotation);
        //     Rigidbody2D rb = tack.GetComponent<Rigidbody2D>();
        //     rb.velocity = tack.transform.up * tackSpeed;
        // }
        */
    }
}