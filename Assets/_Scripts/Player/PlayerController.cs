using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IntoTheMystic.Manager;
using System.Reflection;

namespace IntoTheMystic.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("The maximum angle for moving head up. Values must be entered in the negative")]
        [SerializeField] private float UpperLimit = -40f;

        [Tooltip("The maximum angle for moving head down")]
        [SerializeField] private float BottomLimit = 40f;

        [Tooltip("The camera movement sensitivity of the mouse")]
        [SerializeField] private float MouseSensitivity = 21.9f;

        [Tooltip("The vertical value for Jump Height and Force")]
        [SerializeField, Range(10, 500)] private float JumpFactor = 150f;


        [SerializeField] private LayerMask GroundCheck;

        [Tooltip("The Horizontal value for Jump Distance. 0 means player will not move forward/backward in jump. Higher valued result in greater distances")]
        [SerializeField] private float MovingJumpStrength = 0.8f;

        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private GameObject pickedObjectholder;

        private float Dis2Ground = 2.2f;
        private Rigidbody _playerRigidbody;
        private InputManager _inputManager;
        private Animator _animator;
        private SkinnedMeshRenderer _mesh;
        private bool _grounded = false;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;
        private int _jumpHash;
        private int _groundHash;
        private int _fallingHash;
        private int _zVelHash;
        private int _crouchHash;
        private float _xRotation;
        private bool hulkJump = false;

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity;
        private GravityLifter gravityScript;
        private float distanceFromPlayer = 0;
        private bool isParented = false;

        private void Start() {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidbody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();
            gravityScript = GetComponent<GravityLifter>();
            _mesh = GetComponentsInChildren<SkinnedMeshRenderer>()[0];


            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
            _zVelHash = Animator.StringToHash("Z_Velocity");
            _jumpHash = Animator.StringToHash("Jump");
            _groundHash = Animator.StringToHash("Grounded");
            _fallingHash = Animator.StringToHash("Falling");
            _crouchHash = Animator.StringToHash("Crouch");
            Physics.IgnoreLayerCollision(3, 3);
            Component []list = GetComponentsInChildren(typeof(Collider));
            Debug.Log(list.Length);
            for (int i = 0; i < list.Length; i++)
            {
                for (int j = i; j < list.Length; j++)
                {
                    Physics.IgnoreCollision(list[i] as Collider, list[j] as Collider);
                }
            }
        }


        private void FixedUpdate() {

            if (_inputManager.Picked)
            {
                if (isParented)
                {
                    pickedObjectholder.GetComponent<Collider>().isTrigger = true;
                }
                gravityScript.LiftObject();
                if (isParented)
                {
                    BoxCollider holderCollider = pickedObjectholder.GetComponent<BoxCollider>();
                    holderCollider.enabled = false;
                    holderCollider.center = Vector3.zero;
                    holderCollider.size = Vector3.zero;
                    isParented = false;
                }


                _inputManager.Picked = false;
            }

            SampleGround();
            Move();
            HandleJump();
            HandleCrouch();

            

        }
        private void LateUpdate() {
            CamMovements();
        }

        private void Move()
        {
            if(!_hasAnimator) return;

            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if(_inputManager.Crouch) targetSpeed = 1.5f;
            if(_inputManager.Move ==Vector2.zero) targetSpeed = 0;

            if(_grounded)
            {
                
            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y =  Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

            var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.z;

            _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0 , zVelDifference)), ForceMode.VelocityChange);
            }
            else
            {
                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(_currentVelocity.x * MovingJumpStrength,0,_currentVelocity.y * MovingJumpStrength)), ForceMode.VelocityChange);
            }


            _animator.SetFloat(_xVelHash , _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
            if (gravityScript.grabbedItem && !isParented)
            {
                Vector3 objectCenteredDifference = gravityScript.grabbedItem.GetComponentInChildren<Renderer>().bounds.center - gravityScript.grabbedItem.transform.position;
                objectCenteredDifference.x = 0;
                Rigidbody rb = gravityScript.grabbedItem.GetComponent<Rigidbody>();
                rb.MovePosition(Vector3.Lerp(rb.position, pickedObjectholder.transform.position - objectCenteredDifference, Time.deltaTime * 15.0f));
                if (gravityScript.grabbedItem.GetComponent<Collider>().bounds.Contains(pickedObjectholder.transform.position))
                {
                    pickedObjectholder.transform.rotation = Quaternion.identity;
                    Vector3 pos = gravityScript.grabbedItem.transform.position;
                    Vector3 localPos = pickedObjectholder.transform.InverseTransformPoint(pos);
                    gravityScript.grabbedItem.transform.SetParent(pickedObjectholder.transform);
                    gravityScript.grabbedItem.transform.localPosition = localPos;
                    isParented = true;
                }
            }
            if(isParented && !pickedObjectholder.GetComponent<BoxCollider>().enabled)
            {
                BoxCollider []itemColliders = gravityScript.grabbedItem.GetComponentsInChildren<BoxCollider>();
                foreach (BoxCollider itemCollider in itemColliders)
                {
                    itemCollider.isTrigger = true;
                }
                BoxCollider holderCollider = pickedObjectholder.GetComponent<BoxCollider>();
                holderCollider.isTrigger = false;
                holderCollider.center = pickedObjectholder.transform.InverseTransformPoint(gravityScript.grabbedItem.transform.GetComponentInChildren<Renderer>().bounds.center);
                holderCollider.size = itemColliders[0].bounds.size;
                holderCollider.enabled = true;
                foreach (BoxCollider itemCollider in itemColliders)
                {
                    itemCollider.enabled = false;
                }
                
            }

        }

        private void CamMovements()
        {
            if(!_hasAnimator) return;

            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CameraRoot.position;
            
            
            _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
            if(_inputManager.Crouch || !_grounded)
                _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit/2);
            else
                _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0 , 0);
            _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
            

            
        }

        private void HandleCrouch()
        {
            _animator.SetBool(_crouchHash, _inputManager.Crouch);
        }
        


        private void HandleJump()
        {
            if(!_hasAnimator) return;
            if(!_inputManager.Jump) return;
            //if(!_grounded) return;
            _animator.SetTrigger(_jumpHash);
        }

        public void JumpAddForce()
        {
            _playerRigidbody.AddForce(-_playerRigidbody.velocity.y * Vector3.up, ForceMode.VelocityChange);
            _playerRigidbody.AddForce(Vector3.up * JumpFactor, ForceMode.Impulse);
            _animator.ResetTrigger(_jumpHash);
        }

        private void SampleGround()
        {
            if(!_hasAnimator) return;
            
            RaycastHit hitInfo;
            if(Physics.Raycast(_playerRigidbody.worldCenterOfMass, Vector3.down, out hitInfo, Dis2Ground + 0.1f, GroundCheck))
            {
                //Grounded
                _grounded = true;
                SetAnimationGrounding();
                return;
            }
            //Falling
            Debug.Log(hitInfo.distance);
            Debug.Log("fall detected");
            _grounded = false;
            _animator.SetFloat(_zVelHash, _playerRigidbody.velocity.y);
            SetAnimationGrounding();
            return;
        }

        public void EnableHulkJump()
        {
            if (hulkJump == false)
            {
                JumpFactor *= 3;
                MovingJumpStrength *= 2;
                hulkJump = true;
            }
        }

        public void DisableHulkJump()
        {
            if (hulkJump)
            {
                JumpFactor /= 3;
                MovingJumpStrength /= 2;
                hulkJump = false;
            }
        }

        private void SetAnimationGrounding()
        {
            _animator.SetBool(_fallingHash, !_grounded);
            _animator.SetBool(_groundHash, _grounded);
        }
    }
}
