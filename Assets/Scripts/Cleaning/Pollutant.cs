using System.Runtime.CompilerServices;
using UnityEngine;

public class Pollutant : MonoBehaviour
{
    public PollutionData pollutionData;

    public Polluter polluter;

    [SerializeField] private int Befoulment;
    public int befoulment
    {
        get
        {
            return Befoulment;
        }
    }

    [SerializeField] private ParticleSystem particles;


    private float cleanRate = 0f;
    private int cleaning = 0;

    private void Awake()
    {
        setParticleRate();
    }

    private void FixedUpdate()
    {
        if(cleaning > 0)
        {
            cleanOverTime(Time.fixedDeltaTime);
        }
    }

    private void setParticleRate()
    {
        if(particles != null)
        {
            ParticleSystem.MainModule main = particles.main;
            main.maxParticles = Befoulment;// (Befoulment / 2) + (Befoulment % 2);
            ParticleSystem.EmissionModule emissions = particles.emission;
            float rate = Mathf.Ceil((float)Befoulment / 2f);//5f);
            emissions.rateOverTime = new ParticleSystem.MinMaxCurve(rate, rate * 1.5f);// 3);
        }
    }

    private void cleanOverTime(float deltaTime)
    {
        cleanRate += deltaTime;
        if(cleanRate >= 0.2f)
        {
            cleanRate -= 0.2f;
            cleaning--;
            cleanAmmount(1);
            if(cleaning == 0)
            {
                cleanRate = 0f;
            }
        }
    }

    public void setBefoulment(int ammount)
    {
        Befoulment = ammount;
        if(Befoulment <= 0)
        {
            cleanUp();
        }
        else
        {
            setParticleRate();
        }
    }

    public void cleanAmmount(int ammount)
    {
        setBefoulment(Befoulment - ammount);
    }

    public void befoulAmmount(int ammount)
    {
        setBefoulment(Befoulment + ammount);
    }

    public void cleanUp()
    {
        if(polluter != null)
        {
            polluter.cleanedUp(this);
        }
        Destroy(gameObject);
    }

    private void clean(CleanerData cleaner)
    {
        cleaning += pollutionData.getCleanerEfectiveness(cleaner);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            //Debug.Log("Clean up!");
            CleanerSpray spray = other.GetComponent<CleanerSpray>();
            if(spray != null )
            {
                clean(spray.cleanerType);
            }
        }
    }
}
