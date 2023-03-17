using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newConversation", menuName = "MidnightDreams/Dialogue/Conversation")]
[System.Serializable]
public class Conversation : ScriptableObject
{
	public Sentence[] sentences;
}