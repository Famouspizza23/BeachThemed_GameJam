using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();

    [SerializeField]
    private List<Transform> startingPointsTeamA;
    [SerializeField]
    private List<Transform> startingPointsTeamB;

    private PlayerInputManager playerInputManager;

    private int teamASpawnIndex = 0;
    private int teamBSpawnIndex = 0;

    private void Awake()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);

        PlayerController pController = player.GetComponent<PlayerController>();

        Transform playerParent = player.transform.parent;

        if (pController.TeamA)
        {
            if (teamASpawnIndex < startingPointsTeamA.Count)
            {
                playerParent.position = startingPointsTeamA[teamASpawnIndex].position;
                teamASpawnIndex++;
            }
            else
            {
                Debug.LogWarning("No more spawn points for Team A!");
            }
        }
        else if (pController.TeamB)
        {
            if (teamBSpawnIndex < startingPointsTeamB.Count)
            {
                playerParent.position = startingPointsTeamB[teamBSpawnIndex].position;
                teamBSpawnIndex++;
            }
            else
            {
                Debug.LogWarning("No more spawn points for Team B!");
            }
        }
    }
}
