using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public AudioClip dogDeath;
    public void DeathCry()
    {
        GetComponentInParent<CompanionAI>().dogBark.GetComponent<AudioSource>().PlayOneShot(dogDeath);
    }
}
