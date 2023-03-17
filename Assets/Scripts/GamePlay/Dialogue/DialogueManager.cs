using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour 
{
	public static DialogueManager Instance;
	public GameObject dialogueBox;
	public GameObject popUpCloseButton;
	public Text nameText;
	public Text dialogueText;
	public DialogueTrigger currentDialogue;
	PlayerInput playerInput;
	AudioSource actorVoiceOver;

	private Queue<Sentence> sentences;
	private Dialogue activeDialogue;
	ThirdPersonController thirdPersonController;
	[HideInInspector] public bool canInteract;
	[HideInInspector] public bool hasStartedDialogue;

	void Awake()
	{
		Instance = this;//Instance this as the Audio Manager
	}
	
	void Start () 
	{
		sentences = new Queue<Sentence>();
		playerInput = FindObjectOfType<PlayerInput>();
		thirdPersonController = FindObjectOfType<ThirdPersonController>();
		actorVoiceOver = gameObject.AddComponent<AudioSource>();
		actorVoiceOver.volume = 0.75f;
	}

	public void StartDialogue (Dialogue dialogue)
	{
		if(!canInteract)
			return;

		dialogueBox.SetActive(true);
		popUpCloseButton.SetActive(false);
		MenuManager.Instance.InMenu();
		hasStartedDialogue = true;
		activeDialogue = dialogue;

		sentences.Clear();

		foreach (Sentence _sentence in dialogue.conversation.sentences)
		{
			sentences.Enqueue(_sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			if(currentDialogue == null) return; 
			if(currentDialogue.popUp)
				popUpCloseButton.SetActive(true);
			else
				EndDialogue();
			return;
		}

		Sentence _sentence = sentences.Dequeue();
		//Stop Active VoiceOver Playing
		actorVoiceOver.Stop();
		//Play Audio on the Actor
		if(_sentence.voiceOver != null)
		{
			actorVoiceOver.clip = _sentence.voiceOver;
			actorVoiceOver.Play();
		}

		if(_sentence.actor == Actor.NPC)
		{
			nameText.text = activeDialogue.name;
		}
		else
		{
			nameText.text = activeDialogue.responder;
		}
			
		StopAllCoroutines();
		StartCoroutine(TypeSentence(_sentence.sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		// dialogueText.text = "";
		// foreach (char letter in sentence.ToCharArray())
		// {
		// 	dialogueText.text += letter;
		// 	yield return null;
		// }
		dialogueText.text = sentence;
		yield return new WaitForSeconds(actorVoiceOver.clip.length + 0.25f);
		DisplayNextSentence();
	}

	public void EndDialogue()
	{
		Debug.Log("End HERE!!!");
		actorVoiceOver.Stop();
		dialogueBox.SetActive(false);
		hasStartedDialogue = false;
		thirdPersonController._isInteracting = false;
		MenuManager.Instance.OpenMenu("playerHud");
		MenuManager.Instance.NotInMenu();
		Inventory.Instance.indicator.SetActive(false);

		if(currentDialogue.dialogue.isMission)
		{
			
			if(!currentDialogue.dialogue.mission.isCompleted)
				currentDialogue.dialogue.mission.ShowUI();
			else
				currentDialogue.dialogue.mission.MissionComplete();
		}

		if(currentDialogue.dialogues.Count > 1)
		{
			Debug.Log("Here_1!!!");
			if(currentDialogue.dialogues[1].autoStart)
			{
				currentDialogue.dialogues.Remove(currentDialogue.dialogues[0]);
			}
		}
		canInteract = false;
		if(currentDialogue.transform.parent.gameObject != null)
			currentDialogue.transform.parent.gameObject.SetActive(false);
		currentDialogue = null;
	}
}
