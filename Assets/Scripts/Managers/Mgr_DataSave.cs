using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public abstract class GameData
{

}

[System.Serializable]
public class GlobleData : GameData
{
    public float BgmVolume;
    public float EmVolume;
    public Dictionary<KeyBorad, KeyCode> InputData;

    public GlobleData()
    {
        BgmVolume = 1;
        EmVolume = 1;
        InputData = new Dictionary<KeyBorad, KeyCode>();
        InputData.Add(KeyBorad.Escape, KeyCode.Escape);
        InputData.Add(KeyBorad.ArrowLeft, KeyCode.A);
        InputData.Add(KeyBorad.ArrowRight, KeyCode.D);
        InputData.Add(KeyBorad.ArrowUp, KeyCode.W);
        InputData.Add(KeyBorad.ArrowDown, KeyCode.S);
        InputData.Add(KeyBorad.Attack, KeyCode.J);
        InputData.Add(KeyBorad.Jump, KeyCode.K);
        InputData.Add(KeyBorad.Skill, KeyCode.L);
    }
}

[System.Serializable]
public class PlayerData : GameData
{
    public Dictionary<string, string> CustomData;

    public PlayerData()
    {
        CustomData = new Dictionary<string, string>();
    }
}

public enum DataType
{
    GlobleData,
    PlayerData,
}

public class Mgr_DataSave : MonoBehaviour
{
    private static Mgr_DataSave _instance;
    public static Mgr_DataSave Instance
    {
        get
        {
            if (_instance == null) _instance = GameManager.Instance.gameObject.AddComponent<Mgr_DataSave>();
            return _instance;
        }
    }

    public GlobleData m_GameData { get; set; }
    public PlayerData m_PlayerData { get; set; }

    public void Init()
    {
        if (!GetData())
        {
            m_GameData = new GlobleData();
            m_PlayerData = new PlayerData();
            SaveData();
        }
    }

    private bool GetData()
    {
        string str1 = FileHelper.ReadFromFile(Application.persistentDataPath, DataType.GlobleData.ToString());
        string str2 = FileHelper.ReadFromFile(Application.persistentDataPath, DataType.PlayerData.ToString());
        if (!str1.Equals(string.Empty) && !str2.Equals(string.Empty))
        {
            m_GameData = JsonConvert.DeserializeObject<GlobleData>(str1);
            m_PlayerData = JsonConvert.DeserializeObject<PlayerData>(str2);
            return true;
        }
        return false;
    }

    public bool SaveData()
    {
        return SaveData(DataType.GlobleData) && SaveData(DataType.PlayerData);
    }

    public bool SaveData(DataType type)
    {
        string str = string.Empty;
        switch (type)
        {
            case DataType.GlobleData:
                str = JsonConvert.SerializeObject(m_GameData);
                break;
            case DataType.PlayerData:
                str = JsonConvert.SerializeObject(m_PlayerData);
                break;
        }
        if (!str.Equals(string.Empty))
            return FileHelper.WriteToFile(Application.persistentDataPath, type.ToString(), str, FileMode.Create);
        return false;
    }
}