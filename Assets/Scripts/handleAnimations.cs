using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handleAnimations : MonoBehaviour {
    
    private Animator m_Animator;
    
    void Start() {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            m_Animator.ResetTrigger("idle");
            m_Animator.SetTrigger("run");
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            m_Animator.ResetTrigger("run");
            m_Animator.SetTrigger("idle");
        }

    }
}
