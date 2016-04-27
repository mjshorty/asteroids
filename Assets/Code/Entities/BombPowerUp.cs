using UnityEngine;
using System.Collections;

namespace entity
{
    /// <summary>
    /// Award the player a bomb to use against enemies
    /// </summary>
    public class BombPowerUp : PowerUp
    {
        /// <summary>
        /// Give the player a bomb powerup
        /// </summary>
        /// <param name="player">The player to award the bomb powerup to</param>
        override protected void AwardPowerUp(Player player)
        {
            if (player.Bomb == null)
            {
                GameObject powerUp = utils.Pool.Instance.Create(m_PowerUpPrefab.gameObject, player.transform.position, player.transform) as GameObject;

                player.Bomb = powerUp.GetComponent<SpecialWeapon>();
                Lives = 0;
            }
        }
    }
}