using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private PlayerInput playerInput;
    private InputAction playerAction;
    private InputActionAsset inputAsset;
    private InputActionMap player;

    [Header("Aspects")]
    public bool TeamA;
    public bool TeamB;
    public float playerSpeed = 5f;
    public float rotationSpeed = 720f;

    //Testing
    private CapsuleCollider capsuleCollider;
    public float playerDropTime = 3f;
    private bool playerDropped;

    public Quaternion rotationATeamRot;
    public Quaternion rotationBTeamRot;

    [Header("Health")]
    public int lives = 3;

    public Text testLivesCounter;

    [Header("Balloon")]
    public bool canPickupBalloon;
    public bool hasBalloon;
    public GameObject balloon;
    public Transform balloonSpawnPoint;

    public float inactivityThreshold = 1f;
    private float lastInputTime;
    private bool isInactive;

    private Vector3 lastMoveDirection = Vector3.forward;
    private Quaternion targetIdleRotation;
    private bool isMoving;
    private Coroutine smoothCoroutine;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

        playerInput = GetComponent<PlayerInput>();
        inputAsset = playerInput.actions;
        player = inputAsset.FindActionMap("Player");

        hasBalloon = false;

        lastInputTime = Time.time;
        isInactive = false;
    }

    private void OnEnable()
    {
        player.FindAction("PickupThrow").started += DoPickupThrow;
        player.FindAction("Drop").started += DoDrop;

        var moveAction = player.FindAction("Movement");
        moveAction.performed += Movement;
        moveAction.canceled += Movement;

        if (TeamA)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationATeamRot, rotationSpeed * 100);
        }
        if (TeamB)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationBTeamRot, rotationSpeed * 100);
        }

        player.Enable();
    }

    private void OnDisable()
    {
        player.FindAction("PickupThrow").started -= DoPickupThrow;
        player.FindAction("Drop").started -= DoDrop;

        var moveAction = player.FindAction("Movement");
        moveAction.performed -= Movement;
        moveAction.canceled -= Movement;

        player.Disable();
    }

    private void DoPickupThrow(InputAction.CallbackContext context)
    {
        if(!context.started)
        {
            return;
        }

        GameObject thrownBalloon = Instantiate(balloon, balloonSpawnPoint.position, balloonSpawnPoint.rotation);

        WaterBalloon wb = thrownBalloon.GetComponent<WaterBalloon>();

        Vector3 throwDirection = transform.forward;

        wb.Launch(throwDirection);
    }

    private void DoDrop(InputAction.CallbackContext context)
    {
        Debug.Log("Drop");

        if (!playerDropped)
        {
            playerDropped = true;
            StartCoroutine(Drop());
        }
    }

    void Update()
    {
        MovePlayer();

        if(lives <= 0)
        {
            //this.gameObject.SetActive(false);
            print("Player Death");
        }

        if(!isMoving && Time.time - lastInputTime >= inactivityThreshold)
        {
            if (!isInactive)
            {
                isInactive = true;
                SetIdleFacing();
            }
        }

        testLivesCounter.text = lives.ToString();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            if(smoothCoroutine != null)
            {
                StopCoroutine(smoothCoroutine);
                smoothCoroutine = null;
            }

            isMoving = true;
            isInactive = false;
            lastInputTime = Time.time;
            lastMoveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                lastInputTime = Time.time;
            }
        }
    }

    void MovePlayer()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        if (direction.magnitude > 0.01f)
        {
            isMoving = true;
            lastMoveDirection = direction;

            transform.position += direction * playerSpeed * Time.deltaTime;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void ChooseTeamA()
    {
        TeamA = true;
        TeamB = false;
    }

    public void ChooseTeamB()
    {
        TeamB = true;
        TeamA = false;
    }

    //Why :(
    public PlayerData CreatePlayerData()
    {
        return new PlayerData(gameObject.name, TeamA, TeamB, GetComponent<PlayerInput>().devices[0].deviceId);
    }

    public void DamagePlayers()
    {
        lives--;
    }

    IEnumerator Drop()
    {
        float playerSpeedTemp = playerSpeed;

        playerSpeed = 0f;
        capsuleCollider.height = .5f;
        capsuleCollider.radius = .25f;
        yield return new WaitForSeconds(playerDropTime);
        playerSpeed = playerSpeedTemp;
        capsuleCollider.height = 2f;
        capsuleCollider.radius = .5f;
        playerDropped = false;
    }

    void SetIdleFacing()
    {
        Vector3 facingDir;

        if (lastMoveDirection.x > 0)
        {
            facingDir = Vector3.right;
        }
        else if (lastMoveDirection.x < 0)
        {
            facingDir = Vector3.left;
        }
        else
        {
            facingDir = transform.forward;

            if (TeamA)
            {
                facingDir = Vector3.right;
            }
            if (TeamB)
            {
                facingDir = Vector3.left;
            }            
        }
        
        /*if (TeamB)
        {
            facingDir = -facingDir;
        }*/

        targetIdleRotation = Quaternion.LookRotation(facingDir, Vector3.up);

        smoothCoroutine = StartCoroutine(SmoothRotateToIdle());
    }

    IEnumerator SmoothRotateToIdle()
    {
        while (Quaternion.Angle(transform.rotation, targetIdleRotation) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetIdleRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetIdleRotation;
        smoothCoroutine = null;
    }
}
