using UnityEngine;
using System.Collections.Generic;

namespace entity
{
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        private float m_Dampening = 0.95f;

        [SerializeField]
        private int m_Lives = 1;

        [SerializeField]
        public List<Weapon> m_Weapons = new List<Weapon>();

        public int Lives
        {
            get { return m_Lives; }
            set
            {
                m_Lives = value;
                if (m_Lives == 0)
                {
                    OnDeath();
                    GameObject.Destroy(gameObject);
                }
                else
                {
                    ResetEntity();
                }
            }
        }

        [SerializeField]
        protected int m_InitialHealth = 100;

        protected Vector3 m_Acceleration = Vector3.zero;
        protected Vector3 m_Velocity = Vector3.zero;
        protected int m_Health = 100;

        protected Vector3 m_InitialPosition = Vector3.zero;
        protected Quaternion m_InitialRotation = Quaternion.identity;

        public float HealthPercentage { get { return (float)m_Health / (float)m_InitialHealth; } }

        void Start()
        {
            m_InitialPosition = transform.position;
            m_InitialRotation = transform.rotation;

            ResetEntity();
        }

        public void ApplyDamage(int damage)
        {
            m_Health -= damage;
            if (m_Health <= 0)
            {
                Lives = Lives - 1;
            }
        }

        virtual protected void OnDeath() { }

        virtual protected void UpdateEntity() {}

        virtual protected void OnResetEntity() {}

        protected void ResetEntity()
        {
            m_Acceleration = Vector3.zero;
            m_Velocity = Vector3.zero;
            m_Health = m_InitialHealth;

            transform.position = m_InitialPosition;
            transform.rotation = m_InitialRotation;

            OnResetEntity();
        }

        private void UpdatePosition()
        {
            m_Velocity += m_Acceleration * Time.deltaTime;
            transform.position += m_Velocity * Time.deltaTime;

            m_Acceleration *= m_Dampening;
            m_Velocity *= m_Dampening;

            if (Vector3.SqrMagnitude(m_Velocity) < 0.001f * 0.001f)
            {
                m_Velocity = Vector3.zero;
            }

            if (Vector3.SqrMagnitude(m_Acceleration) < 0.001f * 0.001f)
            {
                m_Acceleration = Vector3.zero;

            }
        }

        private void UpdateScreenWrap()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            if(renderer.isVisible)
            {
                return;
            }

            Camera cam = Camera.main;

            Vector3 center = renderer.bounds.center;
            Vector3 min = renderer.bounds.min;
            Vector3 max = renderer.bounds.max;

            Vector3 screenCenter = cam.WorldToScreenPoint(center);
            Vector3 screenMin = cam.WorldToScreenPoint(min);
            Vector3 screenMax = cam.WorldToScreenPoint(max);

            int width = cam.pixelWidth;
            int height = cam.pixelHeight;

            Vector3 screenPos = screenCenter;

            if (screenMin.x > width)
            {
                float dist = screenCenter.x - screenMin.x;
                screenPos.x = 1 - dist;
            }
            else if (screenMax.x < 0)
            {
                float dist = screenMax.x - screenCenter.x;
                screenPos.x = (width - 1) + dist;
            }

            if (screenMin.y > height)
            {
                float dist = screenCenter.y - screenMin.y;
                screenPos.y = 1 - dist;
            }
            else if(screenMax.y < 0)
            {
                float dist = screenMax.y - screenCenter.y;
                screenPos.y = (height - 1) + dist;
            }

            transform.position = cam.ScreenToWorldPoint(screenPos);
        }

        void Update()
        {
            UpdateEntity();
            UpdatePosition();
            UpdateScreenWrap();
        }

        protected void FireWeapons()
        {
            foreach (var weapon in m_Weapons)
            {
                weapon.Fire();
            }
        }
    }
}