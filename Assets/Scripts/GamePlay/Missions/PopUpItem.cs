using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpItem : MonoBehaviour
{
    public string popUpInfo;
    
    public void PopUp()
    {
        MenuManager.Instance.OpenMenu("popUp");
        MenuManager.Instance.InMenu();
        Cursor.lockState = CursorLockMode.None;

        if(gameObject.GetComponent<DialogueChecker>() != null)
        {
            gameObject.GetComponent<DialogueChecker>().OnStartDialogue();
        }
    }
}