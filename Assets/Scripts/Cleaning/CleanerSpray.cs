using UnityEngine;

public class CleanerSpray : MonoBehaviour
{
    public CleanerData cleanerType;

    [SerializeField] private ParticleSystem sprayParticles;
    [SerializeField] private Collider sprayCollider;
}
