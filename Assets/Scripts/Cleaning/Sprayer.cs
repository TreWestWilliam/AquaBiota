using UnityEngine;
using System.Collections;

public class Sprayer : MonoBehaviour
{
    public SprayerData sprayerData;

    public Cleaner[] cleaners;
    private int selectedCleaner;
    
    public Cleaner cleaner
    {
        get
        {
            if(cleaners == null || cleaners.Length < 1)
            {
                return null;
            }
            if(selectedCleaner >= 0 && selectedCleaner < cleaners.Length)
            {
                return cleaners[selectedCleaner];
            }
            return cleaners[0];
        }
    }

    [SerializeField] private Transform cleaningSprayOrigin;

    [SerializeField] private CleanerSpray[] cleanerSprays;
    private int nextSprayIndex = 0;

    private bool coolingDown = false;

    private void Start()
    {
        foreach(CleanerSpray spray in cleanerSprays)
        {
            spray.setSprayer(sprayerData);
            spray.setCleaner(cleaner.cleanerData);
        }
    }
    public float getWeight()
    {
        float weight = sprayerData.weight;
        foreach(Cleaner cleaner in cleaners)
        {
            weight += cleaner.ammount * cleaner.cleanerData.weightPerAmmount;
        }
        return weight;
    }

    private void setSelectedCleaner(int index)
    {
        selectedCleaner = index % cleaners.Length;
        foreach(CleanerSpray spray in cleanerSprays)
        {
            if(!spray.spraying)
            {
                spray.setCleaner(cleaner.cleanerData);
            }
            spray.setSprayer(sprayerData);
        }
    }

    public void nextCleaner()
    {
        setSelectedCleaner(selectedCleaner + 1);
    }

    public void priorCleaner()
    {
        setSelectedCleaner(selectedCleaner - 1);
    }


    public bool sprayCleaner()
    {
        if(!coolingDown && !cleanerSprays[nextSprayIndex].spraying && cleaner.ammount > 0)
        {
            expendCleaner(sprayerData.ammountPerSpray);

            //Debug.Log("Spray started at " + Time.time);
            cleanerSprays[nextSprayIndex].startSpray(cleaningSprayOrigin);
            coolingDown = true;
            StartCoroutine(sprayComplete(cleanerSprays[nextSprayIndex]));
            StartCoroutine(cooldownComplete());

            nextSprayIndex = (nextSprayIndex + 1) % cleanerSprays.Length;

            return true;
        }

        return false;
    }

    private IEnumerator sprayComplete(CleanerSpray spray)
    {
        yield return new WaitForSeconds(sprayerData.sprayDuration);
        //Debug.Log("Spray complete at " + Time.time);
        spray.stopSpray();
        if(spray.cleanerType != cleaner.cleanerData)
        {
            spray.setCleaner(cleaner.cleanerData);
        }
    }

    private IEnumerator cooldownComplete()
    {
        yield return new WaitForSeconds(sprayerData.consecutiveSprayDelay);
        coolingDown = false;
    }

    private void expendCleaner(int ammount)
    {
        cleaner.ammount -= sprayerData.ammountPerSpray;
        if(cleaner.ammount < 0)
        {
            cleaner.ammount = 0;
        }
    }
}
