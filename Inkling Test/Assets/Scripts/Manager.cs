using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace InklingTest
{
    public class Manager : MonoBehaviour
    {
        #region Variables
        [SerializeField] GameObject m_playerCharacter;
        [SerializeField] Transform m_playerSpawnPosition;
        [Header("UI REFRENCES")]
        [SerializeField] GameObject m_mainMenu, m_controlsMenu, m_deathScreen;
        [SerializeField] Button m_startButton, m_controlsButton, m_backButton;
        bool m_gameActive;
        public static Manager Instance { get; private set; }
        public bool GameActive => m_gameActive;
        #endregion

        #region Life Cycle
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            m_startButton.onClick.AddListener(StartGame);
            m_backButton.onClick.AddListener(BackToMainMenu);
            m_controlsButton.onClick.AddListener(ShowControls);
        }
        #endregion

        #region Private Methods
        void StartGame()
        {
            m_gameActive = true;
            m_mainMenu.SetActive(false);
            m_backButton.gameObject.SetActive(true);
            m_playerCharacter.SetActive(true);
            m_playerCharacter.transform.position = m_playerSpawnPosition.position;
        }

        void BackToMainMenu()
        {
            m_gameActive = false;
            m_deathScreen.SetActive(false);
            m_mainMenu.SetActive(true);
            m_controlsMenu.SetActive(false);
            m_backButton.gameObject.SetActive(false);
        }

        void ShowControls()
        {
            m_controlsMenu.SetActive(true);
            m_mainMenu.SetActive(false);
            m_backButton.gameObject.SetActive(true);
        }
        #endregion

        #region Public Methods
        public void GameOver()
        {
            m_gameActive = false;
            m_deathScreen.SetActive(true);
            m_playerCharacter.SetActive(false);
        }
        #endregion
    }
}
