using UnityEngine;

public class BouyancyDoor : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private Transform Closed;
    [SerializeField] private Transform Open;

    [SerializeField] private Vector3 TouchingOffset;
    [SerializeField] private Vector3 CollisionSize;


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 DisplaySize = new Vector3(CollisionSize.x *transform.localScale.x, CollisionSize.y *transform.localScale.y, CollisionSize.z *transform.localScale.z);
        Gizmos.DrawWireCube(transform.position + TouchingOffset,DisplaySize);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target;

        //This should probably use physics.overlapbox to check if the object is pushing on it but oh well
      if ( Physics.CheckBox(transform.position+ TouchingOffset, CollisionSize) ) 
        {
            target = Closed.position;
        }  
      else 
        {
            target = Open.position;
        }

        Door.transform.position = Vector3.Lerp(Door.transform.position, target, 0.5f);
    }
}
