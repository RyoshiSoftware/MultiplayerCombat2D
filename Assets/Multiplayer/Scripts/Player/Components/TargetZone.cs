using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class TargetZone : MonoBehaviour
    {
        [SerializeField] GameObject reticle;
        [SerializeField] TargettingState targettingStateRef;
        [SerializeField] PlayerController playerController;
        public GameObject target;
        private Vector2 originalReticlePos;
        private string movingState = "MovingState";
        private string targettingState = "TargettingState";
        private int movingStateHash;
        private int targettingStateHash;

        private void Start()
        {
            movingStateHash = movingState.GetHashCode();
            targettingStateHash = targettingState.GetHashCode();
            
            originalReticlePos = reticle.transform.position;
        }
        private void Update()
        {
            if(reticle.activeInHierarchy && target != null)
            {
                reticle.transform.position = target.transform.position;
            }
            else if (target == null)
            {
                reticle.transform.position = originalReticlePos; 
            }

            if (target != null)
            {
                targettingStateRef.hasTarget = true;
            }
            else 
            {
                targettingStateRef.hasTarget = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Target>() ?? null) { return; }

            if (collision != null)
            {
                targettingStateRef.hasTarget = true;
                target = collision.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Target>() ?? null) { return; }
            if (!targettingStateRef.isCurrentState) { return; }

            targettingStateRef.hasTarget = false;
            playerController.SwitchState(targettingStateHash, movingStateHash);
            reticle.SetActive(false);
            target = null;
        }
    }
}