using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields

        public float speed;
        public float rotationSpeed;

        private PlayerInput m_PlayerInput;
        private CharacterController m_ChController;
        private Camera m_MainCamera;

        #endregion

        #region UnityMethods

        private void Awake()
        {
            m_ChController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_MainCamera = Camera.main;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput;

            Quaternion camRotation = m_MainCamera.transform.rotation;
            Vector3 targetDirection = camRotation * moveInput;
            targetDirection.y = 0;

            // update position and rotation
            m_ChController.Move(targetDirection.normalized * speed * Time.fixedDeltaTime);
            m_ChController.transform.rotation = Quaternion.Euler(0, camRotation.eulerAngles.y, 0);
        }

        #endregion


    }
}
