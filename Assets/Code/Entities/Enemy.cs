using UnityEngine;
using System.Collections;

namespace entity
{
    public class Enemy : Entity
    {
        void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Player")
            {
                --m_Lives;
                if (m_Lives == 0)
                {
                    GameObject.Destroy(gameObject);
                }
                else
                {
                    ResetEntity();
                }
            }
        }
    }
}