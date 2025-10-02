using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyPlayerData : MonoBehaviour
{
    public PlayerInput input;
    public bool isReady;
    public bool teamA;

    public LobbyPlayerData(PlayerInput input)
    {
        this.input = input;
        isReady = false;
        teamA = true; // default to Team A
    }
}
