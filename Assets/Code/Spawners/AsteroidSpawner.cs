using UnityEngine;
using System.Collections;

namespace spawn
{
    public class AsteroidSpawner : Spawner
    {
        override protected void OnSpawn(GameObject spawn)
        {
            entity.Asteroid asteroid = spawn.GetComponent<entity.Asteroid>();
            asteroid.ConstantVelocity = asteroid.ConstantVelocity * Random.Range(-1.0f, 1.0f);
        }
    }
}