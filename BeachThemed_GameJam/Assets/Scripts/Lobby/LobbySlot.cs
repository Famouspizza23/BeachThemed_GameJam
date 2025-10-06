using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LobbySlot : MonoBehaviour
{
    [Header("UI States")]
    public GameObject emptyState;
    public GameObject activeState;

    [Header("UI Elements")]
    public Text playerName;
    public Button teamAButton;
    public Button teamBButton;
    public Button readyButton;

    [Header("State")]
    [HideInInspector] public PlayerController playerController;
    public bool isReady = false;
    public bool hasPlayerJoined = false;

    private void Start()
    {
        emptyState.SetActive(true);
        activeState.SetActive(false);

        teamAButton.onClick.AddListener(ChooseTeamA);
        teamBButton.onClick.AddListener(ChooseTeamB);
        readyButton.onClick.AddListener(ToggleReady);
    }

    public void AssignPlayer(PlayerController pc)
    {
        playerController = pc;
        hasPlayerJoined = true;

        emptyState.SetActive(false);
        activeState.SetActive(true);

        playerName.text = pc.name;
    }

    private void ChooseTeamA()
    {
        if (playerController == null) 
            return;
        playerController.ChooseTeamA();
        Debug.Log($"{playerController.name} chose Team A");
    }

    private void ChooseTeamB()
    {
        if (playerController == null) 
            return;
        playerController.ChooseTeamB();
        Debug.Log($"{playerController.name} chose Team B");
    }

    private void ToggleReady()
    {
        isReady = !isReady;
        readyButton.GetComponentInChildren<Text>().text = isReady ? "Ready" : "Ready Up";
        LobbyManager.Instance.CheckAllReady();
    }

    public bool HasPlayer()
    {
        return hasPlayerJoined && playerController != null && isReady;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }
}
