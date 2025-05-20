using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroButtonHandle : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            animator.SetBool("ToClose", true);
        }
    }

    public void ChangeToStay()
    {
        animator.SetBool("ToChange", true);
    }
}
