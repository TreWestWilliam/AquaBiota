using System.Collections.Generic;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    public static OceanManager instance;
    public float SurfaceHeight;
    public float waveAmplitude;

    public float waveSpread;
    public float waveScale;
    public float waveSpeed;

    public int waveArea;
    public float waveSize;
    public int waveSubdivisions;

    public OceanSurface wavePrefab;

    public List<OceanSurface> surface;

    private Vector3 waveCenter;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        waveCenter = Vector3.up * SurfaceHeight;
        createWaves();
    }


    public void recenterWaves(Vector3 center)
    {
        center.y = SurfaceHeight;
        Vector3 offset = center - waveCenter;
        //if(offset.magnitude > waveSize / 2)
        {
            foreach(OceanSurface waves in surface)
            {
                waves.transform.position += offset;
            }
            waveCenter = center;
        }
    }

    public float waveOffset(float x, float z, float time)
    {
        float xCoord1 = x / waveSpread * waveScale + (time * waveSpeed);
        float zCoord1 = z / waveSpread * waveScale;

        float xCoord2 = x / waveSpread * waveScale;
        float zCoord2 = z / waveSpread * waveScale + (time * waveSpeed);

        float y = Mathf.PerlinNoise(xCoord1, zCoord1) + Mathf.PerlinNoise(xCoord2, zCoord2) - 1f;
        y = (y * waveAmplitude / 2);
        return y;
    }

    public float waveHeight(float x, float z, float time)
    {
        return SurfaceHeight + waveOffset(x, z, time);
    }

    private void createWaves()
    {
        for(int x = -waveArea; x <= waveArea; x++)
        {
            for(int z = -waveArea; z <= waveArea; z++)
            {
                Vector3 position = new Vector3((x - 0.5f) * waveSize, SurfaceHeight, (z - 0.5f) * waveSize);
                OceanSurface wave = Instantiate<OceanSurface>(wavePrefab, position, Quaternion.identity, transform);
                wave.name = "Wave (" + x + "," + z + ")";
                wave.xSize = waveSize;
                wave.zSize = waveSize;
                wave.xSubdivisions = waveSubdivisions;
                wave.zSubdivisions = waveSubdivisions;
                wave.GenerateNewMesh();
                surface.Add(wave);
            }
        }
    }
}
