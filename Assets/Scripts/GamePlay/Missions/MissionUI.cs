using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    GameObject missionUI;
    GameObject completeMissionUI;
    public MissionState missionState;
    public string missionInfo;
    public bool isCompleted;
    public int completedMissionId;
    public bool gameComplete;
    public bool onInput;
    
    Transform missionUIHolder;

    void Start()
    {
        missionUIHolder = GameObject.Find("MissionUI").transform;
        missionUI = missionUIHolder.GetChild(1).gameObject;
        completeMissionUI = missionUIHolder.GetChild(0).gameObject;
    }

    public void ShowUI()
    {
        if(gameComplete)
        {
            GameComplete();
            return;
        }
            
        if(missionUI == null)
            return;
        missionUI.SetActive(true);
        missionUI.GetComponent<Text>().text = missionInfo;
        MissionsManager.Instance.GetMissionSound();
        gameObject.SetActive(false);     
    }

    public void MissionComplete()
    {
        isCompleted = true;
        MissionsManager.Instance.GetActiveMission(completedMissionId);
        MissionsManager.Instance.CompleteMissionSound();
        completeMissionUI.SetActive(true);
        missionUI.SetActive(false);
        Invoke("Done", 3f);
        if(gameComplete)
            GameComplete();    
    }

    public void Done()
    {
        completeMissionUI.SetActive(false);
    }

    void OnTriggerStay(Collider other) 
    {
        if(onInput)
            return;
        if(other.gameObject.tag == "Player")
        {
            if(missionState != MissionState.CompleteMission)
                ShowUI();
            else
                MissionComplete();
        }
    }

    //For Demo Build...//
    public void GameComplete()
    {
        Invoke("Completed", 3f);
    }

    void Completed()
    {
        MissionComplete();
        missionUI.SetActive(false);
        completeMissionUI.SetActive(false);
        //Game Completed
    }
}

public enum MissionState
{
    DoMission,
    CompleteMission
}