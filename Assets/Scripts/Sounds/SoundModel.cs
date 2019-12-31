using UnityEngine;

public enum SoundType
{
    Bgm,
    Effect,
}

[System.Serializable]
public class Sound
{
    public SoundEffect m_SoundName;
    public bool m_Mute;
    public bool m_PlayOnAwake;
    public bool m_Loop;
    [Range(0, 256)]
    public int m_Priority;
    [Range(0, 1)]
    public float m_Volume;
    [Range(-3, 3)]
    public float m_Pitch;
    [Range(-1, 1)]
    public float m_StereoPan;
    [Range(0, 1)]
    public float m_SpatialBlend;

    public bool m_IsSetAllOptions { get; private set; }

    public void SetAllOptions(AudioSource audio)
    {
        if (m_IsSetAllOptions || audio == null) return;
        audio.mute = m_Mute;
        audio.playOnAwake = m_PlayOnAwake;
        audio.loop = m_Loop;
        audio.priority = m_Priority;
        audio.volume = m_Volume;
        audio.pitch = m_Pitch;
        audio.panStereo = m_StereoPan;
        audio.spatialBlend = m_SpatialBlend;
        m_IsSetAllOptions = true;
    }
#if UNITY_EDITOR
    public void Default()
    {
        m_Mute = false;
        m_PlayOnAwake = false;
        m_Loop = false;
        m_Priority = 128;
        m_Volume = 1;
        m_Pitch = 1;
        m_StereoPan = 0;
        m_SpatialBlend = 0;
    }
#endif
}

public class SoundModel : MonoBehaviour
{
    public SoundType m_Type;
    public Sound[] m_Sounds;

    public void Init()
    {
        Mgr_Sound.Instance.m_SoundModelList.Add(this);
        for (int i = 0; i < m_Sounds.Length; ++i)
        {
            AudioSource audio = Mgr_Sound.Instance.GetAudio(m_Sounds[i].m_SoundName);
            m_Sounds[i].SetAllOptions(audio);
            switch (m_Type)
            {
                case SoundType.Bgm:
                    audio.volume = m_Sounds[i].m_Volume * Mgr_Sound.Instance.m_GlobleBgmVolume;
                    break;
                case SoundType.Effect:
                    audio.volume = m_Sounds[i].m_Volume * Mgr_Sound.Instance.m_GlobleEmVolume;
                    break;
            }
            audio.enabled = true;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("SetAllValueToDefault")]
    public void SetAllValueToDefault()
    {
        foreach (var item in m_Sounds)
        {
            item.Default();
        }
    }
#endif
}
