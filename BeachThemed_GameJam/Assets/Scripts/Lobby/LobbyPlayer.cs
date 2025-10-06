using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    public Text playerName;
    public Button teamAButton;
    public Button teamBButton;
    public Button readyButton;

    private PlayerController playerController;
    public bool IsReady { get; private set; }

    public void Setup(PlayerController pc)
    {
        playerController = pc;
        playerName.text = "Player " + (pc.GetInstanceID()); // replace with something better if needed

        teamAButton.onClick.AddListener(() => {
            pc.ChooseTeamA();
            Debug.Log("Player joined Team A");
        });

        teamBButton.onClick.AddListener(() => {
            pc.ChooseTeamB();
            Debug.Log("Player joined Team B");
        });

        readyButton.onClick.AddListener(() => {
            IsReady = true;
            readyButton.interactable = false;
            LobbyManager.Instance.CheckAllReady();
        });
    }
}
