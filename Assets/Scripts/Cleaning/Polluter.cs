using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Polluter : MonoBehaviour
{
    public SpawnPollutantArea[] spawnAreas;

    [SerializeField] private float spawnDelay;
    [SerializeField] private float randomDelayOffset;
    private float timeSinceSpawn;

    public int maxPollutants = 5;

    public List<Pollutant> pollutants;


    private void Awake()
    {
        pollutants = new List<Pollutant>();
    }

    private void Start()
    {
        timeSinceSpawn = Random.Range(0f, randomDelayOffset);
    }

    private void Update()
    {
        if(gameObject.activeSelf && pollutants.Count < maxPollutants)
        {
            timeSinceSpawn += Time.deltaTime;
            if(timeSinceSpawn > spawnDelay)
            {
                timeSinceSpawn = timeSinceSpawn % spawnDelay;
                timeSinceSpawn += Random.Range(0f, randomDelayOffset);
                spawnPollution();
            }
        }
    }

    private void spawnPollution()
    {
        float totalChance = 0f;
        foreach(SpawnPollutantArea area in spawnAreas)
        {
            totalChance += area.weight;
        }
        float result = Random.Range(0f, totalChance);
        for(int i = 0; i < spawnAreas.Length; i++)
        {
            if(result > spawnAreas[i].weight)
            {
                result -= spawnAreas[i].weight;
            }
            else
            {
                createPollutant(spawnAreas[i]);
                break;
            }
        }
    }

    private void createPollutant(SpawnPollutantArea area)
    {
        Pollutant pollutant = null;
        if(area.asChild)
        {
            pollutant = Instantiate<Pollutant>(area.pullutant, area.randomSpawnPosition(), area.spawnRotation(), transform);
        }
        else
        {
            pollutant = Instantiate<Pollutant>(area.pullutant, area.randomSpawnPosition(), area.spawnRotation());
        }
        pollutant.polluter = this;
        pollutants.Add(pollutant);
        pollutant.setBefoulment(Random.Range(area.minBefoulment, area.maxBefoulment + 1));
    }

    private void OnDisable()
    {
        for(int i = pollutants.Count - 1; i >= 0; i--)
        {
            if(pollutants[i] == null)
            {
                pollutants.RemoveAt(i);
            }
            else if(pollutants[i].transform.parent != transform)
            {
                pollutants[i].polluter = null;
                pollutants.RemoveAt(i);
            }
        }
    }

    public void cleanedUp(Pollutant pollutant)
    {
        pollutants.Remove(pollutant);
    }
}
