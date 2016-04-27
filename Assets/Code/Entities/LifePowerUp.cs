using UnityEngine;
using System.Collections;

namespace entity
{
    public class LifePowerUp : PowerUp
    {
        /// <summary>
        /// Give the player an extra life
        /// </summary>
        /// <param name="player">The player to award the extra life powerup to</param>
        override protected void AwardPowerUp(Player player)
        {
            // Not great that this is hard coded
            if (player.Lives < 3)
            {
                player.Lives += 1;
                Lives = 0;
            }
        }
    }
}