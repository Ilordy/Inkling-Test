using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InklingTest
{
    public class ObstacleRotator : MonoBehaviour
    {
        #region Variables
        [SerializeField] int m_rotationSpeedMin, m_rotationSpeedMax;
        private int m_currentRotSpeed;
        #endregion

        #region Life Cycle
        void Start()
        {
            m_currentRotSpeed = Random.Range(m_rotationSpeedMin, m_rotationSpeedMax);
            m_currentRotSpeed = Random.value > 0.5f ? -m_currentRotSpeed : m_currentRotSpeed;
        }

        void Update()
        {
            if (!Manager.Instance.GameActive) return;
            transform.Rotate(Vector3.up * m_currentRotSpeed * Time.deltaTime);
        }
        #endregion
    }
}
