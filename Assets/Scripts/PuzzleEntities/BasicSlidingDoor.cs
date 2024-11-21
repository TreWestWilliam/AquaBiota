using UnityEngine;

public class BasicSlidingDoor : MonoBehaviour
{
    private Vector3 StartPos;
    private Quaternion StartRot;
    public Transform Target;
    //private bool MoveToTarget =false;
    private bool IsMoving = false;
    public float MovementDuration;
    private float FinishTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPos = transform.position;
        StartRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving) 
        {
            float percent = 1- ((FinishTime-Time.time ) / MovementDuration );
            transform.position = Vector3.Lerp( StartPos, Target.position, percent);
            transform.rotation = Quaternion.Lerp( StartRot, Target.rotation, percent);
            Debug.Log(percent);
            if (percent >= 1) 
            {
                IsMoving = false;
                Target.position = StartPos;
                Target.rotation = StartRot;
            }
            
        }
        
    }

    public void StartMoving() 
    {
        if (!IsMoving) 
        {
            IsMoving = true;
            //MoveToTarget = !MoveToTarget;
            StartPos = transform.position;
            StartRot = transform.rotation;
            FinishTime = Time.time + MovementDuration;
        }
        
    }
}
