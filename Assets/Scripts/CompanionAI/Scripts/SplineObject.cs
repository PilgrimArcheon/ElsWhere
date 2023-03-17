using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SplineObject : MonoBehaviour
{
    public float catchUpDistToAdd;
    public float normDistToAdd;
    public float distanceToAdd = 1.0f;
    public Spline[] spline;
    [HideInInspector] public int currentSpline;
    public float distancealongspline;
    Transform player;

    public bool canMove;
    public bool moveForward, catchUp, wait;

    void OnEnable()
    {
        distancealongspline = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (spline[currentSpline])
        {
            float off = spline[currentSpline].getLengthOffsetLen(distancealongspline);
            Vector3 p = spline[currentSpline].GetSplinePoint(off);
            Vector3 n = spline[currentSpline].GetSplineSlope(off);

            Vector3 followPos = new Vector3(p.x, transform.position.y, p.z);
            Vector3 followRot = Vector3.ProjectOnPlane(n, Vector3.up);

            transform.position = new Vector3(followPos.x, transform.position.y, followPos.z);

            if(!moveForward)
            {
                transform.rotation = Quaternion.LookRotation(-followRot);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(followRot);
            }
        }

        if(!canMove)
            return;
        
        if(distancealongspline < 0)
        {
            distancealongspline = 0;
            return;
        }
        
        distancealongspline += distanceToAdd * Time.deltaTime;
        if(wait)  
            distanceToAdd = 0;
        else
        {
            if(moveForward && !catchUp)
                distanceToAdd = normDistToAdd;
            else if(!moveForward && !catchUp)
                distanceToAdd = -normDistToAdd;  
            else if(catchUp && moveForward)
                distanceToAdd = catchUpDistToAdd;
            else if(catchUp && !moveForward)
                distanceToAdd = -catchUpDistToAdd;
        }
    }

    private void OnDrawGizmos()
    {
        if (spline == null)
        {
            //spline = transform.root.GetComponentsInChildren<Spline>();
        }
        if (spline[currentSpline])
        {
            float off = spline[currentSpline].getLengthOffsetLen(distancealongspline);
            Vector3 p = spline[currentSpline].GetSplinePoint(off);
            Vector3 n = spline[currentSpline].GetSplineSlope(off);

            Vector3 ortho = new Vector3(n.z, 0, -n.x).normalized;

            // transform.position = p;
            // transform.rotation = Quaternion.LookRotation(n);
        }
    }

    
}
