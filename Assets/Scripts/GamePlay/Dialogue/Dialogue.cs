using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
	public string name;
	public string responder;
	public string description;
	public bool isMission;
	public bool autoStart;

	public Conversation conversation;
	
	[HideInInspector] public string[] sentences;

	public MissionUI mission;
	public MissionUI requiredMission;
}
