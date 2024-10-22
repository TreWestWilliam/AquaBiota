using UnityEngine;

public class CleanerSpray : MonoBehaviour
{
    [SerializeField] private CleanerData CleanerType;
    public CleanerData cleanerType
    {
        get
        {
            return CleanerType;
        }
    }

    [SerializeField] private ParticleSystem sprayParticles;
    [SerializeField] private ParticleSystemRenderer sprayRenderer;
    [SerializeField] private Collider sprayCollider;

    public bool spraying = false;

    public void setCleaner(CleanerData cleaner)
    {
        CleanerType = cleaner;
        ParticleSystem.MainModule main = sprayParticles.main;
        main.startColor = cleaner.color;
        if(cleaner.mesh == null)
        {
            sprayRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        }
        else
        {
            sprayRenderer.renderMode = ParticleSystemRenderMode.Mesh;
            sprayRenderer.mesh = cleaner.mesh;
        }

        sprayRenderer.sharedMaterial = cleaner.material;
    }

    public void setSprayer(SprayerData sprayer)
    {
        ParticleSystem.MainModule main = sprayParticles.main;
        main.startLifetime = sprayer.range;

        ParticleSystem.ShapeModule shape = sprayParticles.shape;
        shape.angle = sprayer.spread;

        float finalRad = (Mathf.Tan(sprayer.spread * Mathf.Deg2Rad) * sprayer.range) + shape.radius;

        CapsuleCollider capsule = sprayCollider as CapsuleCollider;
        if(capsule != null)
        {
            capsule.radius = finalRad;
            capsule.height = sprayer.range + 0.2f;
            capsule.center = new Vector3(0, 0, capsule.height / 2f);
        }
    }

    public void startSpray(Transform location)
    {
        spraying = true;
        transform.position = location.position;
        transform.rotation = location.rotation;
        sprayCollider.gameObject.SetActive(true);
        sprayParticles.Play();
    }

    public void stopSpray()
    {
        sprayParticles.Stop();
        sprayCollider.gameObject.SetActive(false);
        spraying = false;
    }
}
