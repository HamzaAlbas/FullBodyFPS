using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FBFPS.Manager;

namespace FBFPS.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField] private float animationBlendSpeed;
        [SerializeField] private Transform camRoot;
        [SerializeField] private Transform cam;
        [SerializeField] private float upperCamLimit = -40f;
        [SerializeField] private float bottomCamLimit = 70f;
        [SerializeField] private float mouseSensitivity = 21.9f;

        private Rigidbody _rb;
        private InputManager _inputManager;
        private Animator _animator;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;
        private float _xRotation;

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity;
        #endregion

        private void Start()
        {
            //Getting necessary references.
            GetReferences();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void LateUpdate()
        {
            CameraController();
        }

        private void Move()
        {
            //Checking if player has animator.
            if (!_hasAnimator) return;

            //Defining target speed.
            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;

            //If player isn't walking or running targetSpeed should be 0.1f;
            if (_inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

            //Setting player velocity to target speed. And lerping it for smooth transition.
            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, animationBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, animationBlendSpeed * Time.fixedDeltaTime);

            //Calculating the difference between currentVelocity and rb velocity.
            var xVelDifference = _currentVelocity.x - _rb.velocity.x;
            var yVelDifference = _currentVelocity.y - _rb.velocity.y;

            //Setting player rb velocity to currentVelocity. To not make rb indifferent to other forces we apply force.
            _rb.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, yVelDifference)), ForceMode.VelocityChange);

            //Passing velocity values to animator.
            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }

        private void CameraController()
        {
            //Checking if player has animator.
            if (!_hasAnimator) return;

            //Getting mouse input from inputManager.
            var mouseX = _inputManager.Look.x;
            var mouseY = _inputManager.Look.y;

            //Setting cam positon to camRoot position.
            cam.position = camRoot.position;

            //Getting the product between vertical mouse movement, sensitivity and deltatime to calculate the vertical movements of camera.
            _xRotation -= mouseY * mouseSensitivity * Time.deltaTime;  // -= because its the opposite of player direction.

            //Clamping x rotation to limit vertical camera movements.
            _xRotation = Mathf.Clamp(_xRotation, upperCamLimit, bottomCamLimit);

            //Setting the target rotation to the camera.
            cam.localRotation = Quaternion.Euler(_xRotation, 0, 0);

            //Rotating player when camera turns.
            transform.Rotate(Vector3.up, mouseX * mouseSensitivity * Time.deltaTime);

        }

        private void GetReferences()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _rb = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();
            _xVelHash = Animator.StringToHash("xVelocity");
            _yVelHash = Animator.StringToHash("yVelocity");
        }
    }
}
