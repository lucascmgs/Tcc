using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamageManager : MonoBehaviour
{
    public int Damage;
    // Update is called once per frame

    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(this.name == "Explosion")
        Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
