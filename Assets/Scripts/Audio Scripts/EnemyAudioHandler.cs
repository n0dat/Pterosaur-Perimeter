using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private List<AudioClip> m_normalDeathClips;
    [SerializeField] private List<AudioClip> m_dramaticDeathClips;
    [SerializeField] private List<AudioClip> m_damageClips;
    [SerializeField] private List<AudioClip> m_eggStealClips;
    [SerializeField] private List<AudioClip> m_quipClips;
    [SerializeField] private List<AudioClip> m_hitClips;

    public void hit()
    {
        AudioClip clip = m_hitClips[Random.Range(0, m_hitClips.Count)];
        m_audioSource.PlayOneShot(clip);
        Destroy(gameObject, clip.length + 0.1f);
    }

    public void damage()
    {
        //Only play a damage sound 25 percent of the time.
        if (m_damageClips.Count <= 0 || Random.Range(0, 2) == 0)
        {
            Destroy(gameObject);
            return;
        }
        
        AudioClip clip = m_damageClips[Random.Range(0, m_damageClips.Count)];
        m_audioSource.PlayOneShot(clip);
        Destroy(gameObject, clip.length + 0.1f);
    }

    public void death()
    {
        //Only play a damage sound 25 percent of the time.
        if (m_normalDeathClips.Count <= 0 || m_dramaticDeathClips.Count <= 0 || Random.Range(0, 2) != 0)
        {
            Destroy(gameObject);
            return;
        }
        
        AudioClip clip;

        if (Random.Range(0, 10) != 0) // 90 percent of the time, play a normal death clip.
            clip = m_normalDeathClips[Random.Range(0, m_normalDeathClips.Count)];
        else
            clip = m_dramaticDeathClips[Random.Range(0, m_dramaticDeathClips.Count)];
        
        m_audioSource.PlayOneShot(clip);
        Destroy(gameObject, clip.length + 0.1f);
    }

    public void eggSteal()
    {
        //Only play a damage sound 25 percent of the time.
        if (m_eggStealClips.Count <= 0 || Random.Range(0, 2) != 0)
        {
            Destroy(gameObject);
            return;
        }

        AudioClip clip = m_eggStealClips[Random.Range(0, m_eggStealClips.Count)];
        m_audioSource.PlayOneShot(clip);
        Destroy(gameObject, clip.length + 0.1f);
    }

    public void quip()
    {
        //Only play a damage sound 25 percent of the time.
        if (m_quipClips.Count <= 0 || Random.Range(0, 10) != 0)
        {
            Destroy(gameObject);
            return;
        }

        AudioClip clip = m_quipClips[Random.Range(0, m_quipClips.Count)];
        m_audioSource.PlayOneShot(clip);
        Destroy(gameObject, clip.length + 0.1f);
    }
    
}
