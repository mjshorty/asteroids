﻿using UnityEngine;
using System.Collections;

namespace ui
{
    public class Health : MonoBehaviour
    {
        private UnityEngine.UI.Image m_HealthBar = null;

        [SerializeField]
        private entity.Entity m_Player = null;

        // Use this for initialization
        void Start()
        {
            m_HealthBar = GetComponent<UnityEngine.UI.Image>();
        }

        // Update is called once per frame
        void Update()
        {
            m_HealthBar.fillAmount = m_Player.HealthPercentage;
        }
    }
}