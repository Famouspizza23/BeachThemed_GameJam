using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] teamASpawnPoints;
    public Transform[] teamBSpawnPoints;

    [Header("Player Prefab")]
    public GameObject playerPrefab;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        float waitTime = 0f;
        while (GameManager.Instance == null && waitTime < 5f)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        if (GameManager.Instance == null || GameManager.Instance.players.Count == 0)
        {
            Debug.LogError("No player data found in GameManager!");
            yield break;
        }

        yield return new WaitForSeconds(0.25f);
        SpawnAllPlayers();
    }

    private void SpawnAllPlayers()
    {
        var gm = GameManager.Instance;

        int aIndex = 0;
        int bIndex = 0;

        foreach (var p in gm.players)
        {
            Transform spawn = null;

            if (p.teamA && aIndex < teamASpawnPoints.Length)
            {
                spawn = teamASpawnPoints[aIndex++];
            }
            else if (p.teamB && bIndex < teamBSpawnPoints.Length)
            {
                spawn = teamBSpawnPoints[bIndex++];
            }
            else
            {
                continue;
            }

            SpawnPlayer(p, spawn.position);
        }
    }

    //THIS HAS CAUSED ME SO MANY PROBLEMS!!!
    private void SpawnPlayer(PlayerData data, Vector3 spawnPos)
    {
        InputDevice matchedDevice = InputSystem.devices.FirstOrDefault(d => d.deviceId == data.deviceId);

        if (matchedDevice == null)
        {
            Debug.LogWarning($"Could not find device for {data.playerName} (DeviceID {data.deviceId}) - defaulting to Keyboard.");
            matchedDevice = Keyboard.current;
        }

        PlayerInput playerInput = PlayerInput.Instantiate(
            playerPrefab,
            controlScheme: null,
            pairWithDevices: new InputDevice[] { matchedDevice }
        );

        GameObject obj = playerInput.gameObject;
        obj.transform.position = spawnPos;
        obj.name = data.playerName;

        PlayerController controller = obj.GetComponent<PlayerController>();
        if (controller != null)
        {
            if (data.teamA) controller.ChooseTeamA();
            else if (data.teamB) controller.ChooseTeamB();
        }

        Debug.Log($"Spawned {data.playerName} with {matchedDevice.displayName} at {spawnPos}");
    }
}