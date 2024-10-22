using System;
using UnityEngine;

[Serializable]
public class SpawnPollutantArea : SpawnArea
{
    [SerializeField] private Pollutant PollutantPrefab;
    public Pollutant pullutant
    {
        get
        {
            return PollutantPrefab;
        }
    }

    public int minBefoulment = 5;
    public int maxBefoulment = 10;
}
