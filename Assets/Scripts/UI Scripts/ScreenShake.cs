using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public void Start()
    {
        shake();
    }
    
    public void shake()
    {
        StartCoroutine(shakeCamera(2f, 3f));
    }

    private IEnumerator shakeCamera(float duration = 1f, float magnitudeFactor = 1f)
    {
        Vector3 originalCameraPosition = transform.position;
        float timer = Time.time;

        while (Time.time - timer < duration)
        {
            transform.position = originalCameraPosition + magnitudeFactor * (Vector3)Random.insideUnitCircle;
            yield return null;
        }

        transform.position = originalCameraPosition;
    }
}
