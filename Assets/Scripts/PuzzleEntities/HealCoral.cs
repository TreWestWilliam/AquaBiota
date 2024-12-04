using UnityEngine;

public class HealCoral : MonoBehaviour, InteractableObject
{
    public Material Healed;
    public MeshRenderer CoralRenderer;
    public ParticleSystem SparkleParticles;
    private bool IsHealed = false;

    public void beginInteraction(Player player)
    {
        CoralRenderer.material = Healed;
        Material[] mats = new Material[CoralRenderer.materials.Length];  
        for (int i = 0;i<CoralRenderer.materials.Length;i++) 
        {
            mats[i] = Healed;
        }
        CoralRenderer.materials = mats;
        SparkleParticles.Play();
        IsHealed = true;
        enabled = false;
    }

    public void endInteraction(Player player, bool confirmed)
    {
        Debug.Log("This isnt supposed to happen.");
    }

    public string getInteractText()
    {
        return "Heal Coral (End of Temple)";
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public Transform getTransform()
    {
        return transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
