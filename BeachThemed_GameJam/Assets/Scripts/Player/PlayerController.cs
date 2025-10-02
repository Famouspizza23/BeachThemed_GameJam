using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    //private GameActions gameActions;
    private InputActionAsset inputActions;
    private InputActionMap player;
    private InputAction movement;

    [Header("Aspects")]
    public bool TeamA;
    public bool TeamB;
    public float playerSpeed = 5f;

    int randomTeamSet;

    private void Awake()
    {
        inputActions = this.GetComponent<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player");
        //gameActions = new GameActions();
    }

    private void OnEnable()
    {
        player.FindAction("Drop").started += DoDrop;
        player.FindAction("Pickup/Throw").started += DoPickupThrow;
        movement = player.FindAction("Movement");
        player.Enable();
    }

    private void DoPickupThrow(InputAction.CallbackContext context)
    {
        Debug.Log("Pickup/Throw");
    }

    private void DoDrop(InputAction.CallbackContext context)
    {
        Debug.Log("Drop");
    }

    private void OnDisable()
    {
        player.FindAction("Drop").started -= DoDrop;
        player.FindAction("Pickup/Throw").started -= DoPickupThrow;
        player.Disable();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 direction = movement.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * playerSpeed * Time.deltaTime;
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
}
