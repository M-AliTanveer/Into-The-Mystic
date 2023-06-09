using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace IntoTheMystic.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput PlayerInput;

        public Vector2 Move {get; private set;}
        public Vector2 Look {get; private set;}
        public bool Run {get; private set;}
        public bool Jump {get; private set;}
        public bool Crouch {get; private set;}
        public bool Picked { get; set;}

        private InputActionMap currentMap;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction runAction;
        private InputAction jumpAction;
        private InputAction crouchAction;
        private InputAction pickAction;

        private void Awake() {
            HideCursor();
            currentMap = PlayerInput.currentActionMap;
            moveAction = currentMap.FindAction("Move");
            lookAction = currentMap.FindAction("Look");
            runAction  = currentMap.FindAction("Run");
            jumpAction = currentMap.FindAction("Jump");
            crouchAction = currentMap.FindAction("Crouch");
            pickAction = currentMap.FindAction("Pick");

            moveAction.performed += onMove;
            lookAction.performed += onLook;
            runAction.performed += onRun;
            jumpAction.performed += onJump;
            crouchAction.started += onCrouch;
            pickAction.started += onPick;

            moveAction.canceled += onMove;
            lookAction.canceled += onLook;
            runAction.canceled += onRun;
            jumpAction.canceled += onJump;
            crouchAction.canceled += onCrouch;

        }

        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }
        private void onLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }
        private void onRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }
        private void onJump(InputAction.CallbackContext context)
        {
            Jump = context.ReadValueAsButton();
        }
        private void onCrouch(InputAction.CallbackContext context)
        {
            Crouch = context.ReadValueAsButton();
        }
        private void onPick(InputAction.CallbackContext context)
        {
            Picked = !Picked;
        }
        private void OnEnable() {
            currentMap.Enable();
        }

        private void OnDisable() {
            currentMap.Disable();
        }
       
    }
}
