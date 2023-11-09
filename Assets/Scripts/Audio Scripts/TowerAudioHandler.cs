using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class TowerAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    
    [SerializeField] private List<AudioClip> m_lazerClips;
    
    public void shoot()
    {
        //Only play a damage sound 25 percent of the time.
        if (m_lazerClips.Count <= 0)
        {
            Destroy(gameObject);
            return;
        }

        AudioClip clip = m_lazerClips[Random.Range(0, m_lazerClips.Count)];
        m_audioSource.PlayOneShot(clip, 0.2f);
        Destroy(gameObject, clip.length + 0.1f);
    }
}
