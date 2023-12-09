using Unity.Mathematics;
using UnityEngine;

public class EnemyAudioSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_enemyAudioSource;
    
    // play hit sound
    public void hit()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.hit();
    }

    // play damage sound
    public void damage()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.damage();
    }

    // play death sound
    public void death()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.death();
    }

    // play egg steal sound
    public void eggSteal()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.eggSteal();
    }

    // play quip sound from enemy
    public void quip()
    {
        EnemyAudioHandler handler = Instantiate(m_enemyAudioSource, transform.position, quaternion.identity).GetComponent<EnemyAudioHandler>();
        handler.quip();
    }
}
