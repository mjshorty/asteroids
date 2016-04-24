using UnityEngine;
using System.Collections;

public class DestroyOnEffectEnd : MonoBehaviour
{
    private ParticleSystem m_Effect = null;

	// Use this for initialization
	void Start ()
    {
        m_Effect = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!m_Effect.IsAlive())
        {
            GameObject.Destroy(gameObject);
        }
	}
}
