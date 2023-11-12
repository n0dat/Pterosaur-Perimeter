using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private Image m_whiteCover;
    
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
        StartCoroutine(flashCover(duration));
        Vector3 originalCameraPosition = transform.position;
        float timer = Time.time;

        while (Time.time - timer < duration)
        {
            transform.position = originalCameraPosition + magnitudeFactor * (Vector3)Random.insideUnitCircle;
            yield return null;
        }

        transform.position = originalCameraPosition;

        StartCoroutine(fadeCover(1f));
    }

    private IEnumerator flashCover(float duration = 1f)
    {
        float timer = Time.time;
        float trans = 0f;
        bool positive = true;
        
        while (Time.time - timer < duration)
        {
            if (positive)
                trans += 0.05f;
            else
                trans -= 0.05f;
            
            if (trans >= 1f)
                positive = false;
            else if (trans <= 0f)
                positive = true;
            
            setCoverTransparency(trans);
            yield return null;
        }
        setCoverTransparency(0f);
    }

    private IEnumerator fadeCover(float duration = 1f)
    {
        float trans = 1f;
        
        setCoverTransparency(1f);
        yield return new WaitForSeconds(duration);

        while (trans > 0f)
        {
            trans -= 0.01f;
            setCoverTransparency(trans);
            yield return null;
        }
    }

    //Set the transparancy of the white cover. 0 to 1.
    private void setCoverTransparency(float trans)
    {
        if (trans > 1f)
            trans = 1f;
        
        if (trans < 0f)
            trans = 0;
        
        m_whiteCover.color = new Color(m_whiteCover.color.r, m_whiteCover.color.g, m_whiteCover.color.b, trans);
    }
}
