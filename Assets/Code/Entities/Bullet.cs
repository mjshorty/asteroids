using UnityEngine;
using System.Collections;

namespace entity
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private int m_Damage = 10;

        [SerializeField]
        private float m_Velocity = 10.0f;

        [SerializeField]
        private float m_Dampening = 1.0f;

        // Update is called once per frame
        void Update()
        {
            KillWhenOffScreen();
            CalculatePosition();
        }

        private void CalculatePosition()
        {
            float x = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
            float y = -Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);

            Vector3 vel = Vector3.zero;
            vel.x += m_Velocity * Time.deltaTime * x;
            vel.y += m_Velocity * Time.deltaTime * y;

            transform.position += vel;

            m_Velocity *= m_Dampening;
        }

        private void KillWhenOffScreen()
        {
            Renderer renderer = GetComponent<Renderer>();
            if(!renderer.isVisible)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}