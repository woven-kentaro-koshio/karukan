using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance
        {
            get
            {
                return s_Instance;
            }
        }
        #region Fields

        const float k_Acceleration = 20.0f;
        const float k_Deceleration = 35.0f;

        public float maxForwardSpeed = 8.0f;
        public float rotationSpeed;
        public float maxRotationSpeed = 1200;
        public float minRotationSpeed = 800;
        public float gravity = 20.0f;

        private static PlayerController s_Instance;
        private PlayerInput m_PlayerInput;
        private CharacterController m_ChController;
        private Animator m_Animator;
        private CameraController m_CameraController;

        private Quaternion m_TargetRotation;

        private readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");

        private float m_DesiredForwardSpeed;
        private float m_ForwardSpeed;
        private float m_VerticalSpeed;

        #endregion

        #region UnityMethods

        private void Awake()
        {
            m_ChController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            s_Instance= this;
            m_Animator = GetComponent<Animator>();
            m_CameraController = Camera.main.GetComponent<CameraController>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            ComputeForwardMovement();
            ComputeVerticalMovement();
            ComputeRotation();

            if (m_PlayerInput.IsMoveInput)
            {
                float rotationSpeed = Mathf.Lerp(maxRotationSpeed, minRotationSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
                m_TargetRotation = Quaternion.RotateTowards(
                    transform.rotation, 
                    m_TargetRotation, 
                    rotationSpeed * Time.fixedDeltaTime);
                transform.rotation = m_TargetRotation;
            }
        }
        private void OnAnimatorMove()
        {
            Vector3 movement = m_Animator.deltaPosition;
            movement += m_VerticalSpeed * Vector3.up * Time.fixedDeltaTime;
            m_ChController.Move(movement);
        }

        #endregion

        #region Methods

        private void ComputeForwardMovement()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;
            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            float acceleration = m_PlayerInput.IsMoveInput ? k_Acceleration : k_Deceleration;
            m_ForwardSpeed = Mathf.MoveTowards(
                m_ForwardSpeed, 
                m_DesiredForwardSpeed, 
                Time.fixedDeltaTime * acceleration);

            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        private void ComputeVerticalMovement()
        {
            m_VerticalSpeed = -gravity;
        }

        private void ComputeRotation()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;
            Quaternion cameraRotation = Quaternion.Euler(
                0,
                m_CameraController.PlayerCam.m_XAxis.Value,
                0);
            Vector3 cameraDirection = cameraRotation * Vector3.forward;
            Quaternion targetRotation;

            if (Mathf.Approximately(Vector3.Dot(moveInput, Vector3.forward), -1.0f))
            {
                // moving backward
                targetRotation = Quaternion.LookRotation(-cameraDirection);
            } else
            {
                // moving direction except backward
                Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, m_PlayerInput.MoveInput);
                targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);
            }

            m_TargetRotation = targetRotation;
        }

        #endregion




    }
}
