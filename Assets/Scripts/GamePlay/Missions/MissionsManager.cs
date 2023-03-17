using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsManager : MonoBehaviour
{
    public int _missionid;
    public static MissionsManager Instance;
    public List<Mission> missions = new List<Mission>();
    public AudioSource missionSound;
    public AudioClip newMissionSound, completedMissionSound;
    Transform player;

    void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetActiveMission(_missionid);
        if(_missionid == 0) Inventory.Instance.hasOrb = true;
        else Inventory.Instance.hasOrb = false;
        player.position = missions[_missionid].checkPoint.position;
    }

    public void GetActiveMission(int missionId)
    {
        foreach (Mission _mission in missions)
        {
            if(_mission.missionId == missionId)
                _mission.Active();
            else
                _mission.NotActive();
        }
    }

    public void GetMissionSound()
    {
        missionSound.PlayOneShot(newMissionSound);
    }

    public void CompleteMissionSound()
    {
        missionSound.PlayOneShot(completedMissionSound);
    }
}