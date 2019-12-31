using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public CameraMove m_CameraMove;
    public Transform m_Level;
    public Transform m_Loaded;
    public Transform m_SoundList;
    public SoundModel m_Bgm;

    public void Init()
    {
        m_Bgm.Init();
    }
}