using UnityEngine;
using System.Collections;

namespace ui
{
    /// <summary>
    /// Display the powerup image
    /// </summary>
    public class PowerUp : MonoBehaviour
    {
        [SerializeField]
        private entity.Player m_Player = null;

        /// <summary>
        /// Update the powerup image
        /// </summary>
        void Update()
        {
            bool powerUp = m_Player.Bomb != null;

            UnityEngine.UI.Image renderer = GetComponent<UnityEngine.UI.Image>();
            renderer.enabled = powerUp;

            foreach(var childRenderer in GetComponentsInChildren<UnityEngine.UI.Image>())
            {
                childRenderer.enabled = powerUp;
            }
        }
    }
}
