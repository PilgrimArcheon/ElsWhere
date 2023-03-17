using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_Night : MonoBehaviour
{
    Vector3 rotation = Vector3.zero;
    public float degpersec = 5f;

    void Start()
    {
        rotation.x = 50;

    }


    // Update is called once per frame
    void Update()
    {
        rotation.x = degpersec * Time.deltaTime;
        transform.Rotate(rotation, Space.World);
    }
}
