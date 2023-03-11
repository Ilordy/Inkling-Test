using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InklingTest
{
    public class Player : MonoBehaviour
    {
        #region Variables
        [SerializeField] float m_speed, m_rotationSpeed, m_jumpFactor;
        private Animator m_animator;
        private Rigidbody m_rb;
        private CapsuleCollider m_playerCollider;
        private Vector3 m_inputVector;
        private bool m_jump, m_crouched;
        private const string RUNNING_PARAMETER = "Moving", JUMP_PARAMETER = "Jump",
        CROUCHED_PARAMETER = "Crouch";
        #endregion

        #region Life Cycle
        void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            m_animator = GetComponent<Animator>();
            m_playerCollider = GetComponent<CapsuleCollider>();
        }

        void Update()
        {
            if (!Manager.Instance.GameActive) return;
            m_inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            UpdateRotation();
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
                m_jump = true;
            if (Input.GetKeyDown(KeyCode.C) && !m_crouched)
                m_crouched = true;
            UpdateAnimations();
            Crouch();
        }

        void FixedUpdate()
        {
            Vector3 moveVector = m_inputVector * m_speed * Time.deltaTime;
            m_rb.velocity = new Vector3(moveVector.x, m_rb.velocity.y, moveVector.z);
            Jump();
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Death"))
                Manager.Instance.GameOver();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Handles rotation of player model.
        /// </summary>
        void UpdateRotation()
        {
            if (m_inputVector == Vector3.zero) return;
            Quaternion viewDir = Quaternion.LookRotation(transform.position + m_inputVector - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, viewDir, m_rotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Adds force to the player in the y direction if player has pressed the jump button.
        /// </summary>
        void Jump()
        {
            if (!m_jump) return;
            m_rb.AddForce(Vector3.up * m_jumpFactor);
            m_jump = false;
        }

        /// <summary>
        /// Checks if the player is currently grounded via raycasting.
        /// </summary>
        bool IsGrounded()
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, float.MaxValue))
            {
                return Vector3.Distance(transform.position, hit.point) < 0.15f; //hard coded value, could be improved..
            }
            return false;
        }
        /// <summary>
        /// Handles animations based on inputs recieved from the Update function.
        /// </summary>
        void UpdateAnimations()
        {
            m_animator.SetBool(RUNNING_PARAMETER, m_inputVector != Vector3.zero);
            if (m_jump)
                m_animator.SetTrigger(JUMP_PARAMETER);
            if (m_crouched)
                m_animator.SetTrigger(CROUCHED_PARAMETER);
        }
        /// <summary>
        /// Manipulates the player's collider's height and center based on crouch input.
        /// </summary>
        void Crouch()
        {
            if (m_crouched)
            {
                m_playerCollider.height = 1.06f;
                m_playerCollider.center = new Vector3(0, 0.43f, 0); //Values taken from inspector.
                m_crouched = m_inputVector == Vector3.zero;
            }
            else
            {
                m_playerCollider.center = new Vector3(0, 0.91f, 0);
                m_playerCollider.height = 1.85f; //Values taken from inspector.
            }
        }
        #endregion
    }
}
