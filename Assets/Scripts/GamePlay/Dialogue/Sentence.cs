using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
	public Actor actor;
	public AnimActions actions;

	[TextArea(3, 10)]
	public string sentence;
	public AudioClip voiceOver;
}

public enum Actor
{
	Player,
	NPC
}

public enum AnimActions
{
	Idle,
	Talking
}
