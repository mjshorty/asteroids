using UnityEngine;
using System.Collections;

namespace ui
{
    /// <summary>
    /// Display the players current heath
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// The health bar ui image
        /// </summary>
        private UnityEngine.UI.Image m_HealthBar = null;

        /// <summary>
        /// The player
        /// </summary>
        [SerializeField]
        private entity.Entity m_Player = null;

        /// <summary>
        /// Fixup the health bar
        /// </summary>
        void Start()
        {
            m_HealthBar = GetComponent<UnityEngine.UI.Image>();
        }

        /// <summary>
        /// Update the health bar
        /// </summary>
        void Update()
        {
            Vector3 scale = m_HealthBar.transform.localScale;
            scale.x = m_Player.HealthPercentage;
            m_HealthBar.transform.localScale = scale;
        }
    }
}