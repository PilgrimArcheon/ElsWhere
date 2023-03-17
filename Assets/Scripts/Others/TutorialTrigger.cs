using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject objectToShow;
    public bool closeOff;
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            if(!closeOff)
                objectToShow.SetActive(true);
            else
                objectToShow.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            objectToShow.SetActive(false);
        }
    }
}
