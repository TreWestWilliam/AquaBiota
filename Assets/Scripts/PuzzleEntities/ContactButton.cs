using UnityEngine;
using UnityEngine.Events;

public class ContactButton : MonoBehaviour
{
    public GameObject ContactTarget;
    public UnityEvent OnContact;

    public bool OneShot = false;
    private bool FirstContact = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == ContactTarget) 
        {
            if (OneShot)
            {
                if (FirstContact) 
                {
                    OnContact.Invoke();
                }
            }
            else 
            {
                OnContact.Invoke();
            }
            FirstContact = false;
            
        }
    }
}
