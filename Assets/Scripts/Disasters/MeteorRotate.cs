using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRotate : MonoBehaviour
{
    [SerializeField] private GameObject m_meteorContainer;
    

    public void Start()
    {
        rotate();
    }
    public void rotate()
    {
        StartCoroutine(rotateMeteor());
    }

    private IEnumerator rotateMeteor()
    {
        while (true)
        {
            m_meteorContainer.transform.Rotate(0f, 0f, 1f, Space.Self);
            yield return null;
        }
    }

    public void moveMeteor()
    {
        
    }
    
}
