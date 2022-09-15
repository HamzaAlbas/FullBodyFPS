using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace FBFPS.Manager
{
    public class InputManager : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField] private PlayerInput playerInput;
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }

        private InputActionMap _currentMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _runAction;

        #endregion

        private void Awake()
        {
            //Hiding the mouse cursor.
            HideCursor();

            //Finding current input action map by getting from playerInput component.
            _currentMap = playerInput.currentActionMap;

            //Getting individual input actions from currentMap.
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");

            //Subscribing every callback function to InputActions.
            _moveAction.performed += onMove;
            _lookAction.performed += onLook;
            _runAction.performed += onRun;

            //Unsubscribing when player stop performing all of the actions.
            _moveAction.canceled += onMove;
            _lookAction.canceled += onLook;
            _runAction.canceled += onRun;
        }

        private void HideCursor()
        {
            //Hide and lock the mouse cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void onMove(InputAction.CallbackContext context)
        {
            //Getting information from passing context parameter.
            Move = context.ReadValue<Vector2>();
        }

        private void onLook(InputAction.CallbackContext context)
        {
            //Getting information from passing context parameter.
            Look = context.ReadValue<Vector2>();
        }

        private void onRun(InputAction.CallbackContext context)
        {
            //Getting information from passing context parameter.
            Run = context.ReadValueAsButton();
        }

        private void OnEnable()
        {
            //Enabling current action map when InputManager enabled.
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            //Disabling current action map when InputManager disabled.
            _currentMap.Disable();
        }
    }
}