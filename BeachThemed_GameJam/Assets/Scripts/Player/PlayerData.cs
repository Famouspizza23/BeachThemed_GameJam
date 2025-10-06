using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public bool teamA;
    public bool teamB;
    public int deviceId;

    public PlayerData(string name, bool teamA, bool teamB, int deviceId)
    {
        this.playerName = name;
        this.teamA = teamA;
        this.teamB = teamB;
        this.deviceId = deviceId;
    }

    public PlayerData() { }
}
