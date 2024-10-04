using UnityEngine;

public class Pollutant : MonoBehaviour
{
    public PollutionData pollutionData;

    [SerializeField] private int Befoulment;
    public int befoulment
    {
        get
        {
            return Befoulment;
        }
    }


    private float cleanRate = 0f;
    private int cleaning = 0;

    private void FixedUpdate()
    {
        if(cleaning > 0)
        {
            cleanOverTime(Time.fixedDeltaTime);
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
