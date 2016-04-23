using UnityEngine;
using System.Collections;

namespace entity
{
    public class Player : Entity
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        override protected void UpdateEntity()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_Acceleration += transform.forward.normalized * 10.0f;
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.Rotate(0.0f, 0.0f, Time.deltaTime);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.Rotate(0.0f, 0.0f, -Time.deltaTime);
            }
        }
    }
}