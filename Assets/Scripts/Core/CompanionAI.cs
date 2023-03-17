using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAI : MonoBehaviour
{
    public SplineObject splineObj;
    public LayerMask whatIsPlayer;
    public float sightRange;
    public float minFollowDistance;
    public bool playerInSightRange;
    public bool playerMinCatchUpRange;
    public Vector3 playerPosition;
    public GameObject dogBark, dogBreathing;
    Animator anim;
    bool leavePlayer;

    public bool canMove;

    private void Start() 
    {
        splineObj = GetComponent<SplineObject>();
        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if(splineObj == null || leavePlayer) return;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerMinCatchUpRange = Physics.CheckSphere(transform.position, minFollowDistance, whatIsPlayer);
        canMove = playerInSightRange;

        //companion Vector3 position
        Vector3 _companionPos = transform.position;
        //position of Player 
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        //Compare the Vector positions
        float dist = Vector3.Distance(_companionPos, playerPosition); 

        float forwardDistance = Random.Range(splineObj.distancealongspline, splineObj.distancealongspline + 10);
        float backwardDistance = Random.Range(splineObj.distancealongspline - 10, splineObj.distancealongspline);
        //canMove and isMovingForward
        Vector3 forwardVector = GetPosition(forwardDistance);
        Vector3 backwardVector = GetPosition(backwardDistance);

        float forDifference = Vector3.Distance(forwardVector, playerPosition);
        float backDifference = Vector3.Distance(backwardVector, playerPosition);

        if(playerInSightRange)
        {
            splineObj.moveForward = true;
            splineObj.canMove = true;
            splineObj.wait = false;
        }
        else
        {
            if(forDifference < backDifference)
            {
                if(playerMinCatchUpRange)
                {
                    splineObj.moveForward = true;//moveForward
                    splineObj.catchUp = false;
                    dogBark.SetActive(false); 
                    dogBreathing.SetActive(true);
                }
                else 
                {
                    splineObj.moveForward = true;//Catchup
                    splineObj.catchUp = true;
                    dogBark.SetActive(true); 
                    dogBreathing.SetActive(false);
                }
                splineObj.wait = false;
            } 
            else if(backDifference < forDifference)
            {
                if(!playerMinCatchUpRange && !playerInSightRange)
                {
                    splineObj.moveForward = false;//moveBackWard
                    splineObj.wait = false;
                    dogBark.SetActive(true); 
                    dogBreathing.SetActive(false);
                }
                else
                {
                    splineObj.wait = true;//wait
                    dogBark.SetActive(false); 
                    dogBreathing.SetActive(true);
                }
            } 
        }
    }

    void Update()
    {
        if(splineObj != null)
            anim.SetBool("wait", splineObj.wait);
    }

    Vector3 GetPosition(float distance)
    {
        float off = splineObj.spline[splineObj.currentSpline].getLengthOffsetLen(distance);
        Vector3 p = splineObj.spline[splineObj.currentSpline].GetSplinePoint(off);
        Vector3 pos = Vector3.zero;

        return pos = p;
    }

    void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.tag == "DogSit" && !canMove)
        {
            anim.SetBool("Sit", true);
            splineObj.wait = true;
            dogBark.SetActive(false); 
            dogBreathing.SetActive(true);
        }
        else if(other.gameObject.tag == "DogJump" && !canMove)
        {
            anim.SetBool("Jump", true);
            splineObj.distanceToAdd = 0;
            splineObj.wait = true;
            dogBark.SetActive(true); 
            dogBreathing.SetActive(false);
        }
        else if(other.gameObject.tag == "DogDie" && !canMove)
        {
            anim.SetBool("Die", true);
            if(splineObj != null)
            {
                splineObj.distanceToAdd = 0;
                splineObj.wait = true;
            }
            dogBark.SetActive(false); 
            dogBreathing.SetActive(false);
        }
        else if(other.gameObject.tag == "DogLeave")
        {
            splineObj.distanceToAdd = 10;
            splineObj.wait = false;
            splineObj.moveForward = true;
            leavePlayer = true;
            dogBark.SetActive(true); 
            dogBreathing.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        anim.SetBool("Sit", false);
        anim.SetBool("Jump", false);
        anim.SetBool("Die", false);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minFollowDistance);
    }
}
