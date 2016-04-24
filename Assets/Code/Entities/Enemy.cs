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
                Lives = Lives - 1; ;
            }
        }
    }
}