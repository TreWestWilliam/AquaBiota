using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Flock_Boids : MonoBehaviour
{
    [SerializeField] private GameObject flockUnitPrefab;
    [SerializeField] private int flockSize;
    [SerializeField] private Vector3 spawnBounds;

    public GameObject[] allUnits { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GenerateUnits();
    }

    private void GenerateUnits()
    {
        allUnits = new GameObject[flockSize];
        for (int i = 0; i < flockSize; i++)
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * spawnBounds.x, randomVector.y * spawnBounds.y, randomVector.z * spawnBounds.z);
            var spawnPosition = transform.position + randomVector;
            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            allUnits[i] = Instantiate(flockUnitPrefab, spawnPosition, rotation);
        }
    }

}
