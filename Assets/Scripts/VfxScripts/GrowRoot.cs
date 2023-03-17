using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowRoot : MonoBehaviour
{
    public Animator anim;
    public bool grown;    
    public float delay;
    public bool canGrow;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void UpdateRoots()
    {
        if(!canGrow)
        {
            canGrow = true;
            if(grown)
            {
                Invoke("Remove", delay);
            }
            else
            {
                Invoke("Grow", delay);
            }
        }

        anim.SetBool("canGrow", canGrow);
    }

    void Grow()
    {
        anim.SetTrigger("grow");
        grown = true;
        canGrow = false;
    }

    void Remove()
    {
        anim.SetTrigger("remove");
        grown = false;
    }
}
