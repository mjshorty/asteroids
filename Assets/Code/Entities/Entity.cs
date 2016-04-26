using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace entity
{
    /// <summary>
    /// The base entity class all ai objects
    /// in the game will derive from this
    /// (players, asteroids, spaceships etc)
    /// </summary>
    public class Entity : MonoBehaviour
    {
        // Velocity and acceleration dampening
        [SerializeField]
        private float m_Dampening = 0.95f;

        // The inital number of lives granted to the entity
        [SerializeField]
        private int m_Lives = 1;

        // Any optional weapons assigned to the entity
        [SerializeField]
        public List<Weapon> m_Weapons = new List<Weapon>();

        // An optional effect that can be played on the death of the entity
        [SerializeField]
        private GameObject m_OnDeathEffectPrefab = null;

        // The grid effect that will be applied once the player has dieds
        [SerializeField]
        private grid.Grid.Force m_GridForce = grid.Grid.Force.Explosion;

        // The radius of the force applied to the grid
        [SerializeField]
        private float m_GridForceRadius = 10.0f;

        // The magnitude of the force applied to the grid
        [SerializeField]
        private float m_GridForceMagnitude = 100.0f;

        // The intial health of the entity
        [SerializeField]
        protected int m_InitialHealth = 100;

        // The current number of lives assigned to the entity
        private int m_CurrentLives = 0;

        // The current acceleration of the entity
        protected Vector3 m_Acceleration = Vector3.zero;

        // The current velocity of the entity
        protected Vector3 m_Velocity = Vector3.zero;

        // The current health of the entity
        protected int m_Health = 100;

        // The cached initial position of the entity
        protected Vector3 m_InitialPosition = Vector3.zero;

        // The intial rotation of the entity
        protected Quaternion m_InitialRotation = Quaternion.identity;

        // Get the percentage of health left
        public float HealthPercentage { get { return (float)m_Health / (float)m_InitialHealth; } }

        // Get or set the current nubmer of lives the entity has
        // Setting this to 0 may kill the entity
        public int Lives
        {
            get { return m_CurrentLives; }
            set
            {
                bool lifeLost = value < m_CurrentLives;
                m_CurrentLives = value;

                if (lifeLost)
                {
                    if (m_OnDeathEffectPrefab)
                    {
                        GameObject effectGO = utils.Pool.Instance.Create(m_OnDeathEffectPrefab, transform.position);
                    }

                    grid.Grid gameGrid = grid.Grid.Instance;
                    if(gameGrid)
                    {
                        if (m_GridForce == grid.Grid.Force.Explosion)
                        {
                            gameGrid.ApplyExplosiveForce(m_GridForceMagnitude, transform.position, m_GridForceRadius);
                        }
                        else if(m_GridForce == grid.Grid.Force.Implosion)
                        {
                            gameGrid.ApplyImplosiveForce(m_GridForceMagnitude, transform.position, m_GridForceRadius);
                        }
                        else
                        {
                            float rotation = transform.rotation.z * Mathf.Deg2Rad;
                            Vector3 direction = Vector3.zero;

                            direction.x -= Mathf.Sin(rotation);
                            direction.y += Mathf.Cos(rotation);

                            gameGrid.ApplyDirectedForce(direction * m_GridForceMagnitude, transform.position, m_GridForceRadius);
                        }
                    }

                    if (m_CurrentLives <= 0)
                    {
                        if(OnDeath(false))
                        {
                            // delayed destroy
                            StartCoroutine(DelayedDestroy());
                        }
                        else
                        {
                            utils.Pool.Instance.Destroy(gameObject);
                        }
                    }
                    else
                    {
                        ResetEntity();
                    }
                }
            }
        }

        /// <summary>
        /// Destroy the entity after 3 seconds
        /// hiding it in the meantime
        /// </summary>
        IEnumerator DelayedDestroy()
        {
            Renderer renderer = GetComponent<Renderer>();
            if(renderer)
            {
                renderer.enabled = false;
            }

            foreach(var childRenderer in GetComponentsInChildren<Renderer>())
            {
                childRenderer.enabled = false;
            }

            yield return new WaitForSeconds(3.0f);
            utils.Pool.Instance.Destroy(gameObject);
        }

        /// <summary>
        /// Fixup the entity
        /// </summary>
        void Start()
        {
            m_InitialPosition = transform.position;
            m_InitialRotation = transform.rotation;
            m_CurrentLives = m_Lives;

            ResetEntity();
        }

        /// <summary>
        /// Apply damage to the entity, deducted from health
        /// </summary>
        /// <param name="damage">Th amount of damage to apply</param>
        public void ApplyDamage(int damage)
        {
            m_Health -= damage;
            if (m_Health <= 0)
            {
                Lives = Lives - 1;
            }
        }

        /// <summary>
        /// Kill the entity due to the end of the game being reached
        /// </summary>
        public void GameOverDeath()
        {
            OnDeath(true);
        }

        /// <summary>
        /// Called when the entity is about to be deleted
        /// </summary>
        /// <param name="gameOver">Was this due to a game over?</param>
        /// <returns>Trigger a delayed death?</returns>
        virtual protected bool OnDeath(bool gameOver) { return false; }

        /// <summary>
        /// Update the entity
        /// </summary>
        virtual protected void UpdateEntity() {}

        /// <summary>
        /// Reset the entity to its original state
        /// </summary>
        virtual protected void OnResetEntity() {}

        /// <summary>
        /// Reset the entities transform
        /// </summary>
        protected void ResetEntity()
        {
            m_Acceleration = Vector3.zero;
            m_Velocity = Vector3.zero;
            m_Health = m_InitialHealth;

            transform.position = m_InitialPosition;
            transform.rotation = m_InitialRotation;

            OnResetEntity();
        }

        /// <summary>
        /// Update the position of the entity by integrating the accelration and velocity
        /// </summary>
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

        /// <summary>
        /// If the entity goes off screen wrap it around to the other side
        /// </summary>
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

        /// <summary>
        /// Update the entity
        /// </summary>
        void Update()
        {
            UpdateEntity();
            UpdatePosition();
            UpdateScreenWrap();
        }

        /// <summary>
        /// Fire any weapons attached to the entitys
        /// </summary>
        protected void FireWeapons()
        {
            foreach (var weapon in m_Weapons)
            {
                weapon.Fire();
            }
        }
    }
}