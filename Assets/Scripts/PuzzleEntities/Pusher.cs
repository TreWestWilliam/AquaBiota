using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pusher : MonoBehaviour
{
    [SerializeField] private Vector3 Direction = Vector3.forward;
    [SerializeField]private List<Rigidbody> rigidbodies = new();
    private bool IsPushing = true;

    public void SetPushing(bool pushing) {  IsPushing = pushing; }


    public void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (rb != null) 
        { 
            rigidbodies.Add(rb);
        }
    }

    public void OnTriggerExit(Collider other) 
    { 
        var rb =other.GetComponent<Rigidbody>();
        if (rb != null) 
        {
            try 
            {
                rigidbodies.Remove(rb);
            }
            catch (System.Exception e) 
            {
                Debug.Log($"error removing rigidbody from pusher: {e.Message} ");
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPushing)
        {

            foreach (var rb in rigidbodies) 
            {
                rb.AddForce(Direction);
            }

        }
    }
}
