using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Mgr_Input : MonoBehaviour
{
    private static Mgr_Input _instance;
    public static Mgr_Input Instance
    {
        get
        {
            if (_instance == null) _instance = GameManager.Instance.gameObject.AddComponent<Mgr_Input>();
            return _instance;
        }
    }

    public void Init()
    {

    }

    public bool ChangeKey(KeyBorad keyborad, KeyCode newKey)
    {
        if ((int)keyborad < AppConst.PlayerCustomKeyIndex) return false;
        Mgr_DataSave.Instance.m_GameData.InputData[keyborad] = newKey;
        return true;
    }

    // public void SaveKeyData()
    // {
    //     string str = JsonConvert.SerializeObject(m_InputData);
    //     FileHelper.WriteToFile(Application.persistentDataPath, AppConst.InputCustomData, str, FileMode.Create);
    // }
}
