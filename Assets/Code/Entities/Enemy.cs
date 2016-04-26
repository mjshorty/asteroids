using UnityEngine;
using System.Collections.Generic;

namespace entity
{
    public class Enemy : Entity
    {
        /// <summary>
        /// The positional acceleration of the enemy
        /// </summary>
        [SerializeField]
        private float m_PositionalAcceleration = 10.0f;

        /// <summary>
        /// How fast the enemy can rotate
        /// </summary>
        [SerializeField]
        private float m_RotationAcceleration = 0.4f;

        /// <summary>
        /// How accurate the enemy is at firing
        /// </summary>
        [SerializeField]
        private float m_Accuracy = 0.25f;

        /// <summary>
        /// A list of targets (players) to fire at
        /// </summary>
        protected List<Player> m_Targets = new List<Player>();

        /// <summary>
        /// Initialise the list of targets
        /// </summary>
        void Start()
        {
            // this function call is slow but we currently dont have any other way of passing through the player
            // maybe the spawner could pass the player to us
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
            foreach (var go in targets)
            {
                Player player = go.GetComponent<Player>();
                if (player)
                {
                    m_Targets.Add(player);
                }
            }
        }

        /// <summary>
        /// Update the enemy, tracking down and firing upon its target
        /// </summary>
        protected override void UpdateEntity()
        {
            base.UpdateEntity();

            Player target = GetClosestPlayer();
            if(target == null)
            {
                return;
            }

            // find our direction
            float rotation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector3 direction = Vector3.zero;

            direction.x -= Mathf.Sin(rotation);
            direction.y += Mathf.Cos(rotation);

            // find out where we are in relation to the player
            Vector3 dirToPlayer = target.transform.position - transform.position;
            Vector3 dirToPlayerNormalized = dirToPlayer.normalized;

            // turn to face the player
            float zRot = Mathf.Atan2(dirToPlayerNormalized.y, dirToPlayerNormalized.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(0.0f, 0.0f, zRot - 90.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, m_RotationAcceleration);

            // if we are in view of the player fire
            float meDotTarget = Vector3.Dot(direction, dirToPlayerNormalized);
            if(meDotTarget > m_Accuracy)
            {
                // move towards the player
                m_Acceleration.x += dirToPlayerNormalized.x * m_PositionalAcceleration;
                m_Acceleration.y += dirToPlayerNormalized.y * m_PositionalAcceleration;

                FireWeapons();
            }
        }

        /// <summary>
        /// Find the closest target player
        /// </summary>
        /// <returns>The closest target player</returns>
        protected Player GetClosestPlayer()
        {
            if(m_Targets.Count == 0)
            {
                return null;
            }

            Player player = m_Targets[0];
            float dstSq = Vector3.SqrMagnitude(transform.position - player.transform.position);

            for(int i = 1; i < m_Targets.Count; ++i)
            {
                float playerDstSq = Vector3.SqrMagnitude(transform.position - m_Targets[i].transform.position);
                if(playerDstSq < dstSq)
                {
                    dstSq = playerDstSq;
                    player = m_Targets[i];
                }
            }

            return player;
        }

        /// <summary>
        /// Handle collision with the player
        /// </summary>
        /// <param name="collision">The collision data</param>
        void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Player")
            {
                Lives = Lives - 1; ;
            }
        }
    }
}