using UnityEngine;
using System.Collections;

/// <summary>
/// Destroy a particle effect once it has finished playing
/// </summary>
public class DestroyOnEffectEnd : MonoBehaviour
{
    /// <summary>
    /// The particle system componetn
    /// </summary>
    private ParticleSystem m_Effect = null;

	/// <summary>
    /// Fixup the class
    /// </summary>
	void Start ()
    {
        m_Effect = GetComponent<ParticleSystem>();
	}
	
	/// <summary>
    /// Monitor the particle effect, once its no longer active return it to the pool
    /// </summary>
	void Update ()
    {
        if(!m_Effect.IsAlive())
        {
            utils.Pool.Instance.Destroy(gameObject);
        }
	}
}
