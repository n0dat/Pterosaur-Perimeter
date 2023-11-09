using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAudioSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_enemyAudioSource;
    
    public void hit()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.hit();
    }

    public void damage()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.damage();
    }

    public void death()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.death();
    }

    public void eggSteal()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.eggSteal();
    }

    public void quip()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.quip();
    }
}
