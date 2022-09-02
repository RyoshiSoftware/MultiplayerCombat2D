using UnityEngine;

namespace RyoshiSoftware.Multiplayer.PlayerController2D
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;

        public static InputManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public Controls controls { get; private set; }

        private void Awake()
        {
            if (_instance != null && _instance != this) { Destroy(this.gameObject); }

            controls = new Controls();
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        public Vector2 GetPlayerMovement()
        {
            return controls.PlayerControls.Movement.ReadValue<Vector2>();
        }

        public bool MovementPressedThisFrame()
        {
            return controls.PlayerControls.Movement.WasPressedThisFrame();
        }

        public bool MeleeAttackPressedThisFrame()
        {
            return controls.PlayerControls.MeleeAttack.WasPressedThisFrame();
        }

        public bool TargetPressedThisFrame()
        {
            return controls.PlayerControls.Target.WasPerformedThisFrame();
        }

        public bool CancelPressedThisFrame()
        {
            return controls.PlayerControls.Cancel.WasPerformedThisFrame();
        }

        public bool RangedAttackPressedThisFrame()
        {
            return controls.PlayerControls.RangedAttack.WasPerformedThisFrame();
        }

        public bool RangedAttackReleasedThisFrame()
        {
            return controls.PlayerControls.RangedAttack.WasReleasedThisFrame();
        }

        public bool BlockPressedThisFrame()
        {
            return controls.PlayerControls.Block.WasPerformedThisFrame();
        }

        public bool BlockReleasedThisFrame()
        {
            return controls.PlayerControls.Block.WasReleasedThisFrame();
        }

        public bool FlyPressedThisFrame()
        {
            return controls.PlayerControls.Fly.WasPressedThisFrame();
        }

        public bool SpiritBurstPressedThisFrame()
        {
            return controls.PlayerControls.SpiritBurst.WasPerformedThisFrame();
        }

        public bool SpiritBurstReleasedThisFrame()
        {
            return controls.PlayerControls.SpiritBurst.WasReleasedThisFrame();
        }

        public bool DashPressedThisFrame()
        {
            return controls.PlayerControls.Dash.WasPressedThisFrame();
        }

        public bool DashReleasedThisFrame()
        {
            return controls.PlayerControls.Dash.WasReleasedThisFrame();
        }

        public bool TeleportPressedThisFrame()
        {
            return controls.PlayerControls.Teleport.WasPressedThisFrame();
        }
    }
}
