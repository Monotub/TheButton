using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineTest : MonoBehaviour
{
    Animator anim;
    bool onMenuCamera = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchState();
        }
    }

    private void SwitchState()
    {
        if (onMenuCamera)
        {
            anim.Play("Game Camera");
        }
        else
            anim.Play("Menu Camera");

        onMenuCamera = !onMenuCamera;
    }
}
