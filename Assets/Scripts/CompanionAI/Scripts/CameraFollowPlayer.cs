using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject targetPlayer;
    public Vector3 offset;
    public float damping;

    // Update is called once per frame
    void Update()
    {
         
        Vector3 newPosition = targetPlayer.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, new Vector3(newPosition.x, transform.position.y, newPosition.z), Time.deltaTime * damping);
    }
}
