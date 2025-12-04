using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ParticleSystem destroyParticles;
    public int damage = 1;
    public void Die()
    {
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
    }
}
