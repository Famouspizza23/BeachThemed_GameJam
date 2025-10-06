using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public bool pickupBalloon;
    public bool throwBalloon;

    public Quaternion rotationATeamRot;
    public Quaternion rotationBTeamRot;

    [Header("Health")]
    public int lives = 3;

    [Header("Balloon")]
    public bool hasBalloon;
    public GameObject balloon;
    public Transform balloonSpawnPoint;
    public float balloonSpeed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputAsset = playerInput.actions;
        player = inputAsset.FindActionMap("Player");

        if (TeamA)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationATeamRot, rotationSpeed * 10);
        }
        if (TeamB)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationBTeamRot, rotationSpeed * 10);
        }

        hasBalloon = false;
    }

    private void OnEnable()
    {
        player.FindAction("PickupThrow").started += DoPickupThrow;
        player.FindAction("Drop").started += DoDrop;

        var moveAction = player.FindAction("Movement");
        moveAction.performed += Movement;
        moveAction.canceled += Movement;

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
        Debug.Log("PickupThrow");

        if (context.started)
        {
            throwBalloon = true;
        }

        else if (context.canceled)
        {
            throwBalloon = false;
        }
    }

    private void DoDrop(InputAction.CallbackContext context)
    {
        Debug.Log("Drop");
    }

    void Update()
    {
        MovePlayer();

        if ()
        {
            Instantiate(balloon, balloonSpawnPoint.position, balloonSpawnPoint.rotation);
            balloon.GetComponent<Rigidbody>().velocity = balloonSpawnPoint.forward * balloonSpeed;
        }
    }

    public void Movement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void MovePlayer()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        transform.position += direction * playerSpeed * Time.deltaTime;

        if(direction != Vector3.zero)
        {
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
}
