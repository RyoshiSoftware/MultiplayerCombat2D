using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingAnimation : MonoBehaviour
{
    Animator anim;

    readonly int Dash = Animator.StringToHash("Dash");

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    void Start() 
    {
        anim.SetFloat("horizontal", 0);
        anim.SetFloat("vertical", -1);
        anim.Play(Dash);
    }
}
