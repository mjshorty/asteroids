using UnityEngine;
using System.Collections;

namespace entity
{
    public class Player : Entity
    {
        [SerializeField]
        private float m_RotationSpeed = 10.0f;

        // Update is called once per frame
        override protected void UpdateEntity()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                float rotation = transform.rotation.eulerAngles.z;
                Vector3 acceleration = Vector3.zero;

                acceleration.x += Mathf.Sin(Mathf.Deg2Rad * rotation);
                acceleration.y += -Mathf.Cos(Mathf.Deg2Rad * rotation);

                m_Acceleration += acceleration * 10.0f;
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0.0f, 0.0f, -Time.deltaTime * m_RotationSpeed);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0.0f, 0.0f, Time.deltaTime * m_RotationSpeed);
            }
        }
    }
}