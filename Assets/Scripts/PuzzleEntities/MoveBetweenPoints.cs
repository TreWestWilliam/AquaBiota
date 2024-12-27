using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    public Transform[] Points;
    public Transform Object;
    public int iterator = 1;
    private int prev = 0;
    [Range(0,1)]
    [SerializeField] private float percent;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        if (Points.Length == 0)
        {
            Debug.Log("MoveBetweenpoints cannot work with no points", gameObject);
            Destroy(this);
        }
        else if (Points.Length == 1) 
        {
            Object.transform.position = Points[0].position;
            Object.transform.rotation = Points[0].rotation;
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(Points[prev].position, Points[iterator].position) > .1)
        {
            percent += speed * (1 / Vector3.Distance(Points[prev].position, Points[iterator].position)) * Time.deltaTime;
        }
        else 
        {
            percent += speed * Time.deltaTime;
        }
        
        Object.position = Vector3.Lerp(Points[prev].position, Points[iterator].position, percent);
        Object.rotation = Quaternion.Lerp(Points[prev].rotation, Points[iterator].rotation, percent);

        if (percent >= 1) 
        {
            prev = iterator;
            iterator++;
            if ( iterator >= Points.Length) 
            {
                iterator = 0;
            }
            percent = 0;
        }
        
    }
}
