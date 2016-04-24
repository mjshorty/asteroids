﻿using UnityEngine;
using System.Collections.Generic;

namespace entity
{
    public class Enemy : Entity
    {
        [SerializeField]
        private float m_PositionalAcceleration = 10.0f;

        [SerializeField]
        private float m_RotationAcceleration = 10.0f;

        [SerializeField]
        private float m_Accuracy = 0.25f;

        protected List<Player> m_Targets = new List<Player>();

        void Start()
        {
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

        protected override void UpdateEntity()
        {
            base.UpdateEntity();

            Player target = GetClosestPlayer();
            if(target == null)
            {
                return;
            }

            // find the direction to the player
            //Quaternion.Lerp(transform.rotation, targetRotation, m_Acceleration * Time.deltaTime);

            // find our direction
            float rotation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector3 direction = Vector3.zero;

            direction.x -= Mathf.Sin(rotation);
            direction.y += Mathf.Cos(rotation);

            // find out where we are in relation to the player
            Vector3 dirToPlayer = target.transform.position - transform.position;

            // move towards the player
            m_Acceleration.x += dirToPlayer.normalized.x * m_PositionalAcceleration;
            m_Acceleration.y += dirToPlayer.normalized.y * m_PositionalAcceleration;

            // if we are in view of the player fire
            float meDotTarget = Vector3.Dot(direction, dirToPlayer.normalized);
            if(meDotTarget > m_Accuracy)
            {
                FireWeapons();
            }
        }

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

        void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Player")
            {
                Lives = Lives - 1; ;
            }
        }
    }
}