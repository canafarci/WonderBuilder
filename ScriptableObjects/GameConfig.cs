using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/New Game Config", order = 0)]
public class GameConfig : ScriptableObject
{
    public int StartMoney;
    public SpeedUpgrades[] SpeedUpgrades;
    public CountUpgrades[] CountUpgrades;
    public MergeUpgrades[] MergeUpgrades;
}

[System.Serializable]
public class AudioConfig
{
    public AudioClip Audio;
    public float Volume;
}
[System.Serializable]
public struct SpeedUpgrades
{
    public float AgentBaseSpeed;
    public int Cost;
}
[System.Serializable]
public struct CountUpgrades
{
    public int Cost;
}
[System.Serializable]
public struct MergeUpgrades
{
    public int Cost;
}

