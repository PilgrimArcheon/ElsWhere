using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour 
{
	public List<Dialogue> dialogues;
	public bool popUp;
	[HideInInspector] public Animator anim;
	[HideInInspector] public Dialogue dialogue;


	public void Start() 
	{
		anim = GetComponentInParent<Animator>();
	}
	
	public void PlayDialogue()
	{
		if(dialogues.Count > 1)
		{
			if((dialogues[1].requiredMission != null && dialogues[1].requiredMission.isCompleted))
			{
				dialogues.Remove(dialogues[0]);
			}
		}	

		if(dialogues.Count != 0)
		{
			dialogue = dialogues[0];
			FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
		}
	}

	public void PlayAnim(AnimActions actions)
	{
		if(actions == AnimActions.Idle)
		{
			anim.SetBool("idle", true);
			anim.SetBool("talking", false);
		}
		else if(actions == AnimActions.Talking)
		{
			anim.SetBool("talking", true);
			anim.SetBool("idle", false);
		}
	}
}
