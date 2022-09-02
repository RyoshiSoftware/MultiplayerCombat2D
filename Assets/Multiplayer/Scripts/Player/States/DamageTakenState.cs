using System.Collections;
using System.Collections.Generic;
using RyoshiSoftware.Multiplayer.PlayerController2D;
using Mirror;
using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class DamageTakenState : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;

        private string thisState = "DamageTakenState";
        private string movingState = "MovingState";

        private int thisStateHash;
        private int movingStateHash;

        private readonly int damagedBlendTreeHash = Animator.StringToHash("Damaged");
        private readonly int locomotionBlendTreeHash = Animator.StringToHash("Locomotion");

        public bool isCurrentState;

        [ClientCallback]
        private void Start()
        {
            thisStateHash = thisState.GetHashCode();
            movingStateHash = movingState.GetHashCode();
        }

        public void EnterDamageTakenState()
        {
            playerController.animator.Play(damagedBlendTreeHash);
        }

        private void ExitDamagTakenState()
        {
            playerController.animator.Play(locomotionBlendTreeHash);
            playerController.SwitchState(thisStateHash, movingStateHash);
        }
    }
}
