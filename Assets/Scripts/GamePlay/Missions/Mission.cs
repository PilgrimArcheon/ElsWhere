using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    public GameObject missionComponents;
    public int missionId;
    public Transform checkPoint;

    public void NotActive()
    {
        if(missionComponents != null)
            missionComponents.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Active()
    {
        missionComponents.SetActive(true);
        FindObjectOfType<CompassBar>().UpdateItemMarkers();
    }
}