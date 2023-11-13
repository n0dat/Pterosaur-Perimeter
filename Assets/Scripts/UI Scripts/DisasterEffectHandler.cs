using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class DisasterEffectHandler : MonoBehaviour
{
    [SerializeField] private Image m_whiteCover;
    [SerializeField] private GameObject m_meteor;
    [SerializeField] private GameObject m_earthQuake;
    [SerializeField] private AudioSource m_audio;

    private bool m_inProgress = false;
    private Vector3 m_originalEarthQuakePosition;
    private Vector3 m_originalMeteorPosition;
    
    public void Start()
    {
        m_originalMeteorPosition = m_meteor.transform.position;
        m_originalEarthQuakePosition = m_earthQuake.transform.position;
    
    }

    public void earthQuakeDisaster(Vector3 earthQuakeTo, float duration)
    {
        if (m_inProgress)
            return;
        
        m_inProgress = true;
        m_whiteCover.gameObject.SetActive(true);
        StartCoroutine(shakeAndFlashCamera(duration, 3f));
        StartCoroutine(moveEarthQuake(earthQuakeTo, duration));
    }
    
    public void meteorDisaster(Vector3 meteorTo, float duration)
    {
        if (m_inProgress)
            return;
        
        m_inProgress = true;
        m_whiteCover.gameObject.SetActive(true);
        StartCoroutine(shakeAndFlashCamera(duration, 3f));
        StartCoroutine(moveMeteor(meteorTo, duration));
    }

    private void startRumble()
    {
        StartCoroutine(fadeInRumble());
    }

    private void stopRumble()
    {
        StartCoroutine(fadeOutRumble());
    }

    private IEnumerator fadeInRumble()
    {
        m_audio.volume = 0f;
        m_audio.mute = false;

        while (m_audio.volume < 1f)
        {
            m_audio.volume += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator fadeOutRumble()
    {
        m_audio.volume = 1f;

        while (m_audio.volume > 0f)
        {
            m_audio.volume -= Time.deltaTime;
            yield return null;
        }

        m_audio.mute = true;
    }
    
    private IEnumerator shakeAndFlashCamera(float duration = 1f, float magnitudeFactor = 1f)
    {
        Vector3 originalCameraPosition = transform.position;
        float timer = Time.time;
        
        startRumble();
        StartCoroutine(flashCover(duration));
        
        while (Time.time - timer < duration)
        {
            transform.position = originalCameraPosition + magnitudeFactor * (Vector3)Random.insideUnitCircle;
            yield return null;
        }

        transform.position = originalCameraPosition;
        stopRumble();
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
            trans -= Time.deltaTime*1.5f;
            setCoverTransparency(trans);
            yield return null;
        }
        
        m_whiteCover.gameObject.SetActive(false);

        m_inProgress = false;
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

    public IEnumerator moveMeteor(Vector3 finalMeteorPosition, float duration)
    {
        Vector3 translationVector = finalMeteorPosition - m_originalMeteorPosition;
        float startTime = Time.time;

        while (Vector3.Distance(m_meteor.transform.position, finalMeteorPosition) > 5f)
        {
            if (Time.time - startTime >= duration)
                break;
            
            m_meteor.transform.Translate(translationVector * Time.deltaTime*0.3f, Space.Self);
            yield return null;
        }

        float timeLeft = duration - (Time.time - startTime);

        if (timeLeft > 0)
            yield return new WaitForSeconds(timeLeft);
                
        m_meteor.transform.position = m_originalMeteorPosition;
    }

    public IEnumerator moveEarthQuake(Vector3 earthQuakePosition, float duration)
    {
        m_earthQuake.transform.position = earthQuakePosition;
        yield return new WaitForSeconds(duration);
        m_earthQuake.transform.position = m_originalEarthQuakePosition;
    }
}
