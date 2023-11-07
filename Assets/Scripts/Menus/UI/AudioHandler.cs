using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private List<AudioSource> m_audioList;
    
    public bool playAudio(string soundName)
    {
        for (int i = 0; i < m_audioList.Count; i++)
        {
            if (m_audioList[i].gameObject.name != soundName)
                continue;
            
            m_audioList[i].Play();
            return true;
        }

        return false;
    }
}
