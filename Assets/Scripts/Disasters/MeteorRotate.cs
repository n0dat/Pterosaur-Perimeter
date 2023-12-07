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
    
    //Starts coroutine to rotate the meteor.
    public void rotate()
    {
        StartCoroutine(rotateMeteor());
    }

    //Function just rotates the meteor container at a constant rate.
    private IEnumerator rotateMeteor()
    {
        while (true)
        {
            m_meteorContainer.transform.Rotate(0f, 0f, 1f, Space.Self);
            yield return null;
        }
    }
    
}
