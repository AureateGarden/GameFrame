using System;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffect
{
    None = 0,
    Bgm_01_soft = 10,
}

public class Mgr_Sound : MonoBehaviour
{
    private static Mgr_Sound _instance;
    public static Mgr_Sound Instance
    {
        get
        {
            if (_instance == null) _instance = GameManager.Instance.gameObject.AddComponent<Mgr_Sound>();
            return _instance;
        }
    }

    public List<SoundModel> m_SoundModelList { get; private set; }
    private GameObject m_SoundListObj;
    private Dictionary<SoundEffect, AudioSource> m_SoundList;

    public float m_GlobleBgmVolume { get; private set; }
    public float m_GlobleEmVolume { get; private set; }

    public void Init()
    {
        m_SoundList = new Dictionary<SoundEffect, AudioSource>();
        m_SoundModelList = new List<SoundModel>();
        m_SoundListObj = GameManager.Instance.m_MainGame.m_SoundList.gameObject;
        m_GlobleBgmVolume = Mgr_DataSave.Instance.m_GameData.BgmVolume;
        m_GlobleEmVolume = Mgr_DataSave.Instance.m_GameData.EmVolume;
        foreach (var item in (int[])Enum.GetValues(typeof(SoundEffect)))
        {
            if (item < 10) continue;
            AudioClip audio = Mgr_AssetBundle.Instance.LoadAsset<AudioClip>(ABTypes.sound, ((SoundEffect)item).ToString());
            AudioSource Source = m_SoundListObj.AddComponent<AudioSource>();
            Source.playOnAwake = false;
            Source.clip = audio;
            Source.enabled = false;
            m_SoundList.Add((SoundEffect)item, Source);
        }
    }

    public AudioSource GetAudio(SoundEffect sound)
    {
        AudioSource audio;
        if (m_SoundList.TryGetValue(sound, out audio)) return audio;
        return null;
    }

    public void PlaySound(SoundEffect sound, bool IsPlayOneShot = false)
    {
        AudioSource audio = GetAudio(sound);
        if (audio == null) return;
        if (IsPlayOneShot) audio.Play();
        else audio.PlayOneShot(audio.clip);
    }

    public void ToggleSoundPause(SoundEffect sound)
    {
        AudioSource audio = GetAudio(sound);
        if (audio == null) return;
        if (audio.isPlaying) audio.Pause();
        else
        {
            audio.UnPause();
            if (!audio.isPlaying) audio.Play();
        }
    }

    public void StopSound(SoundEffect sound)
    {
        AudioSource audio = GetAudio(sound);
        if (audio == null) return;
        audio.Stop();
    }

    public void StopAllSound()
    {
        foreach (var item in (int[])Enum.GetValues(typeof(SoundEffect)))
        {
            if (item < 10) continue;
            AudioSource audio = GetAudio((SoundEffect)item);
            if (audio == null) continue;
            audio.Stop();
        }
    }

    public void SetSoundVolume(SoundType type, float size)
    {
        size = Math.Min(1f, Math.Max(0f, size));
        switch (type)
        {
            case SoundType.Bgm:
                m_GlobleBgmVolume = size;
                break;
            case SoundType.Effect:
                m_GlobleEmVolume = size;
                break;
        }
        foreach (var item in m_SoundModelList)
        {
            if (item.m_Type.Equals(type))
            {
                foreach (var item2 in item.m_Sounds)
                {
                    m_SoundList[item2.m_SoundName].volume = item2.m_Volume * size;
                }
            }
        }
    }
}
