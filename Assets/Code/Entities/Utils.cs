using UnityEngine;
using System.Collections;

namespace entity
{
    class Utils
    {
        /// <summary>
        /// If the entity goes off screen wrap it around to the other side
        /// </summary>
        /// <param name="go">The game object to wrap</param>
        static public void UpdateScreenWrap(GameObject go)
        {
            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            if (renderer.isVisible)
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
            else if (screenMax.y < 0)
            {
                float dist = screenMax.y - screenCenter.y;
                screenPos.y = (height - 1) + dist;
            }

            go.transform.position = cam.ScreenToWorldPoint(screenPos);
        }
    }
}