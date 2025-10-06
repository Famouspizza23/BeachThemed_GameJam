using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    [Header("UI")]
    public LobbySlot[] slots; // assign only 4 slots in Inspector for now unless we want to add more players later
    public Button startButton;
    public GameObject startGameText;

    [Header("Lobby Slots")]
    public List<LobbySlot> lobbySlots = new List<LobbySlot>();


    [Header("Game Scene")]
    public string gameLevelName;

    public List<PlayerData> playersData = new List<PlayerData>();

    private List<PlayerController> unassignedPlayers = new List<PlayerController>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("No GameManager instance found!");
            return;
        }

        startGameText.SetActive(false);
        startButton.interactable = false;
        startButton.onClick.AddListener(StartGame);

    }

    private void OnEnable()
    {
        if (PlayerInputManager.instance != null)
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        if (PlayerInputManager.instance != null)
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }


    private void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerController pc = playerInput.GetComponent<PlayerController>();
        if (pc == null)
        {
            Debug.LogWarning("Joined player has no PlayerController!");
            return;
        }

        unassignedPlayers.Add(pc);

        LobbySlot slot = GetFirstEmptySlot();
        if (slot != null)
        {
            slot.AssignPlayer(pc);
            MarkPlayerAssigned(pc);
        }
        else
        {
            Debug.LogWarning("No empty slots available!");
        }

        RefreshPlayerDataList();
        CheckAllReady();
    }

    private LobbySlot GetFirstEmptySlot()
    {
        if (slots != null && slots.Length > 0)
        {
            foreach (var slot in slots)
            {
                if (slot != null && !slot.hasPlayerJoined)
                    return slot;
            }
        }

        foreach (var slot in lobbySlots)
        {
            if (slot != null && !slot.hasPlayerJoined)
                return slot;
        }

        return null;
    }

    public void MarkPlayerAssigned(PlayerController pc)
    {
        if (unassignedPlayers.Contains(pc))
            unassignedPlayers.Remove(pc);
    }

    public void CheckAllReady()
    {
        bool allReady = true;
        bool anyTaken = false;

        foreach (var slot in lobbySlots)
        {
            if (slot.hasPlayerJoined)
            {
                anyTaken = true;
                if (!slot.isReady)
                {
                    allReady = false;
                    break;
                }
            }
        }

        startButton.gameObject.SetActive(anyTaken && allReady);
        startButton.interactable = true;
        startGameText.SetActive(true);
    }

    private void RefreshPlayerDataList()
    {
        playersData.Clear();

        IEnumerable<LobbySlot> iterate = (slots != null && slots.Length > 0) ? slots : lobbySlots;

        foreach (var slot in iterate)
        {
            if (slot == null) continue;

            PlayerController pc = slot.GetPlayerController();
            if (pc != null)
            {
                PlayerData data = pc.CreatePlayerData();
                playersData.Add(data);
            }
        }
    }

    public void StartGame()
    {
        RefreshPlayerDataList();

        if (playersData.Count == 0)
        {
            Debug.LogWarning("No players to start!");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("No GameManager instance found before starting game!");
            return;
        }

        GameManager.Instance.SetPlayers(playersData);

        SceneManager.LoadScene(gameLevelName);
    }
}
