using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum KeyBorad
{
    Escape,
    ArrowLeft = AppConst.PlayerCustomKeyIndex,
    ArrowRight,
    ArrowUp,
    ArrowDown,
    Attack,
    Jump,
    Skill,
}

public class InputBase : MonoBehaviour
{
    public delegate void InputHandler();
    protected Dictionary<KeyBorad, KeyCode> m_Keys;

    public virtual void Init()
    {
        m_Keys = Mgr_DataSave.Instance.m_GameData.InputData;
    }
}
