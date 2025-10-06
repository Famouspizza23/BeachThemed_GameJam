using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<PlayerData> players = new List<PlayerData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (players == null)
            players = new List<PlayerData>();
    }

    public void SetPlayers(List<PlayerData> newPlayers)
    {
        if (newPlayers == null || newPlayers.Count == 0)
        {
            Debug.LogWarning("Tried to set players with an empty list.");
            return;
        }

        players = new List<PlayerData>(newPlayers);
    }

    public List<PlayerData> GetPlayers()
    {
        if (players == null)
        {
            Debug.LogError("Player data list is null!");
            return new List<PlayerData>();
        }

        return players;
    }

    public void ClearPlayers()
    {
        if (players != null)
        {
            players.Clear();
        }
    }
}
