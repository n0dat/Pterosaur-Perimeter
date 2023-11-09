using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TowerAudioSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_towerAudio;

    public void shoot()
    {
        TowerAudioHandler handler = Instantiate(m_towerAudio, transform.position, quaternion.identity).GetComponent<TowerAudioHandler>();
        handler.shoot();
    }
}
