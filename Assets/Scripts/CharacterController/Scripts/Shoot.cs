using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float range;
    public GameObject bulletImpact;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
        }
        
    }

    public void ShootBullet()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if(hit.transform.name == "Cube")
            {
                Destroy(hit.transform.gameObject);
            }

            Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
        }




    }
}
