using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Player")]
    public float moveSpeed;
    public float turnSpeed;
    public float jumpHeight = 0.5f;
    public float gravityMultiplier;
    float hMove, vMove;

    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;
    public float speedChangeRate = 10.0f;
    float sensitivity;
    [Space(10)]
    public float gravity = -15.0f;
    [Space(10)]
    public float jumpTimeout = 0.50f;
    public float fallTimeout = 0.15f;

    [Header("Player Grounded")]
    public bool grounded = true;
    public float groundedOffset = -0.14f;
    public float groundedRadius = 0.28f;
    public LayerMask groundLayers;

    public CharacterController characterController;

    [Header("Cinemachine")]
    public GameObject cinemachineCameraTarget;
    public float topClamp = 70.0f;
    public float bottomClamp = -30.0f;
    public float cameraAngleOverride = 0.0f;
    public bool lockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // animation IDs
    private int _animIDSpeedX, _animIDSpeedY, _animIDGrounded, _animIDJump;
    private int _animIDFreeFall, _animIDMotionSpeed;
    private int _animWeaponIdle, _animWeaponAim, _animWeaponReload;

    private Animator _animator;
    private CharacterController _controller;
    private GameObject _mainCamera;

    private const float _threshold = 0.01f;

    private bool _hasAnimator;
    public float _magnitudeVal;

    //Shooter
    [Header("Shooter")]
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private int numOfAnimStates = 1;
    [SerializeField] private Rig aimRig;

    Transform hitTransform;
    Vector3 mouseWorldPosition;
    float aimRigWeight;

    public bool canPick;
    public bool isAiming;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hMove = Input.GetAxis("Horizontal");
        vMove = Input.GetAxis("Vertical");
        gravity = 9.8f;
        Move();
        
        if(canPick == true && Input.GetKey(KeyCode.E))
        {
            PickObject();
        }
    }

    void Move()
    {
        Vector3 move = new Vector3(hMove, 0, vMove);
        if(move.magnitude > 0 && !isAiming)
        {
            Quaternion newDirectio = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, newDirectio, Time.deltaTime * turnSpeed);
        }

        characterController.SimpleMove(move * Time.deltaTime  * moveSpeed );
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Collectible")
        {
            //Show Pick Up On UI
            canPick = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Collectible")
        {
            canPick = false;
        }
    }

    public void PickObject()
    {
        GameObject collectible = GameObject.FindGameObjectWithTag("Collectible");
        Destroy(collectible);
        //Add To INVENTORY
    }
}
