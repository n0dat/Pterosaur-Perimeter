using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        m_audioSource.PlayOneShot(m_hitClips[Random.Range(0, m_hitClips.Count)]);
    }

    public void damage()
    {
        //Only play a damage sound 25 percent of the time.
        if (m_damageClips.Count <= 0 || Random.Range(0, 4) != 0)
            return;
        
        m_audioSource.PlayOneShot(m_damageClips[Random.Range(0, m_damageClips.Count)]);
    }

    public void death()
    {
        //Only play a damage sound 25 percent of the time.
        if (m_normalDeathClips.Count <= 0 || m_dramaticDeathClips.Count <= 0 || Random.Range(0, 4) != 0)
            return;
        
        if (Random.Range(0, 10) != 0) // 90 percent of the time, play a normal death clip.
            m_audioSource.PlayOneShot(m_normalDeathClips[Random.Range(0, m_normalDeathClips.Count)]);
        else
            m_audioSource.PlayOneShot(m_dramaticDeathClips[Random.Range(0, m_dramaticDeathClips.Count)]);
    }

    public void eggSteal()
    {
        //Only play a damage sound 25 percent of the time.
        if (m_eggStealClips.Count <= 0 || Random.Range(0, 4) != 0)
            return;
        
        m_audioSource.PlayOneShot(m_eggStealClips[Random.Range(0, m_eggStealClips.Count)]);
    }

    public void quip()
    {
        //Only play a damage sound 25 percent of the time.
        if (m_quipClips.Count <= 0 || Random.Range(0, 4) != 0)
            return;
        
        m_audioSource.PlayOneShot(m_quipClips[Random.Range(0, m_quipClips.Count)]);
    }
}
