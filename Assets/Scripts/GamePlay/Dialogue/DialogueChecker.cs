using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChecker : MonoBehaviour 
{
	public bool onInput;
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Player" && !onInput)
		{
			OnStartDialogue();
		}
	}

	public void OnStartDialogue()
	{
		if(transform.GetChild(0).gameObject != null)
		{
			DialogueManager.Instance.canInteract = true;
			DialogueManager.Instance.currentDialogue = GetComponentInChildren<DialogueTrigger>();
			DialogueManager.Instance.currentDialogue.PlayDialogue();
		}	
	}

	void OnTriggerExit(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			DialogueManager.Instance.canInteract = false;
		}
	}
}
