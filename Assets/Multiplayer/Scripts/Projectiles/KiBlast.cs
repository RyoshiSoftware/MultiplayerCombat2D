using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.NetworkSpawnables
{
    public class KiBlast : NetworkBehaviour
    {
        public Rigidbody2D rigidBody2D;
        public Animator animator;
        private TargettingState targettingState;

        public float force = 1000f;


        [ClientCallback]
        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        [ClientCallback]
        private void Start()
        {
            LaunchBlast();
        }

        [ClientCallback]
        private void FixedUpdate()
        {
            // if (targettingState.isCurrentState && targettingState.reticle.activeInHierarchy)
            // {
            //     Transform target = targettingState.reticle.transform;

            //     Vector2 direction = (Vector2)target.transform.position - rigidBody2D.position;
            //     direction.Normalize();
            //     float rotateAmount = Vector3.Cross (direction, transform.up).z;
            //     rigidBody2D.angularVelocity = -rotateAmount * 200f;
            //     rigidBody2D.velocity = transform.up * 5f;
            // }
        }

        private void LaunchBlast()
        {
            rigidBody2D.AddForce(transform.right * force);
        }
    }
}