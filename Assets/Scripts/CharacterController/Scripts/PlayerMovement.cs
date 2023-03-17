using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject camHolder;
    [SerializeField] private float moveSpeed;
    [SerializeField]float sensitivity;
    float mouseX;
    float mouseY;
    float horizontalMove;
    float verticalMove;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        
    }

    private void FixedUpdate()
    {
        Vector3 desiredForward = camHolder.transform.forward;

        Vector3 ForwardmoveDir = (desiredForward * verticalMove).normalized;
        Vector3 SidewaysMoveDir = new Vector3(desiredForward.z, 0, -desiredForward.x) * horizontalMove;
        Vector3 moveDir = (ForwardmoveDir + SidewaysMoveDir).normalized;

        if(Mathf.Abs(verticalMove) + Mathf.Abs(horizontalMove) != 0)
        {
            transform.rotation = Quaternion.LookRotation(moveDir,Vector3.up);
        }
        transform.position += moveDir * moveSpeed * Time.fixedDeltaTime;

        camHolder.transform.Rotate(Vector3.up * mouseX, Space.World);
        camHolder.transform.position = Vector3.Lerp(camHolder.transform.position, transform.position, 20 * Time.fixedDeltaTime);
    }

    
}
