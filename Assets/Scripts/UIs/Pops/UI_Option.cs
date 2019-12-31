using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option : PopBase
{
    [Header("Custom")]
    public GameObject m_ForWardWaitingPage;
    public ScrollRect m_SubPageKeyBorad;
    public Button[] m_SubListBtn;
    public GameObject[] m_SubPages;
    [Header("Sound Page")]
    public Slider m_BgmSlider;
    public Slider m_EmSlider;
    public InputField m_BgmSize;
    public InputField m_EmSize;
    private GameObject m_ShowSubPage;
    private UM_KeyCell m_CurrentCell;
    private List<UM_KeyCell> m_KeyCellList;
    private Dictionary<KeyBorad, KeyCode> m_TempChangeKeys;

    public override void Init()
    {
        base.Init();
        m_KeyCellList = new List<UM_KeyCell>();
        m_TempChangeKeys = new Dictionary<KeyBorad, KeyCode>();
        InitSubListBtn();
        InitKeyboradList();
        InitSoundPage();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        m_CurrentCell = null;
        m_ForWardWaitingPage.SetActive(false);
        m_ShowSubPage = m_SubPages[0];
        Refresh();
    }

    public override void OnClose()
    {
        SaveOptions();
        base.OnClose();
    }

    private void OnGUI()
    {
        if (m_ForWardWaitingPage.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                Event e = Event.current;
                if (!e.keyCode.Equals(KeyCode.Escape))
                {
                    AddChangedKey((KeyBorad)Enum.Parse(typeof(KeyBorad), m_CurrentCell.m_TitleKeyName.text.ToString()), e.keyCode);
                }
                m_CurrentCell.m_BtnKey.interactable = true;
                m_ForWardWaitingPage.SetActive(false);
            }
        }
    }

    public void BtnBack()
    {
        OnClose();
    }

    public void SaveOptions()
    {
        Mgr_DataSave.Instance.m_GameData.BgmVolume = Mgr_Sound.Instance.m_GlobleBgmVolume;
        Mgr_DataSave.Instance.m_GameData.EmVolume = Mgr_Sound.Instance.m_GlobleEmVolume;
        foreach (var item in m_TempChangeKeys.Keys)
        {
            Mgr_Input.Instance.ChangeKey(item, m_TempChangeKeys[item]);
        }
        Mgr_DataSave.Instance.SaveData(DataType.GlobleData);
    }

    private void InitSubListBtn()
    {
        if (m_SubListBtn.Length != m_SubPages.Length) return;
        for (int i = 0; i < m_SubListBtn.Length; ++i)
        {
            int temp = i;
            m_SubListBtn[temp].onClick.AddListener(() =>
            {
                if (m_SubPages[temp].Equals(m_ShowSubPage)) return;
                m_ShowSubPage.SetActive(false);
                m_SubPages[temp].SetActive(true);
                m_ShowSubPage = m_SubPages[temp];
            });
        }
    }

    private void InitKeyboradList()
    {
        GameObject typeLine = Mgr_AssetBundle.Instance.LoadAsset<GameObject>(ABTypes.prefab, AppConst.UIModelTypeLine);
        GameObject KeyCell = Mgr_AssetBundle.Instance.LoadAsset<GameObject>(ABTypes.prefab, AppConst.UIModelKeyCell);
        foreach (var item in (int[])Enum.GetValues(typeof(KeyBorad)))
        {
            if (item < AppConst.PlayerCustomKeyIndex) continue;
            if (item == AppConst.PlayerCustomKeyIndex) AddSubKeyboradPageTitle(typeLine, AppConst.StrPlayerKeyBoradTitle);
            UM_KeyCell umCell = Instantiate(KeyCell).GetComponent<UM_KeyCell>();
            umCell.transform.SetParent(m_SubPageKeyBorad.content, false);
            umCell.m_TitleKeyName.text = ((KeyBorad)item).ToString();
            umCell.m_BtnKey.onClick.AddListener(() =>
            {
                umCell.m_BtnKey.interactable = false;
                m_ForWardWaitingPage.SetActive(true);
                m_CurrentCell = umCell;
            });
            m_KeyCellList.Add(umCell);
        }
    }

    private void InitSoundPage()
    {
        m_BgmSlider.onValueChanged.AddListener((float value) =>
        {
            m_BgmSize.text = value.ToString();
            Mgr_Sound.Instance.SetSoundVolume(SoundType.Bgm, value / 100f);
        });
        m_EmSlider.onValueChanged.AddListener((float value) =>
        {
            m_EmSize.text = value.ToString();
            Mgr_Sound.Instance.SetSoundVolume(SoundType.Effect, value / 100f);
        });
        m_BgmSize.onEndEdit.AddListener((string str) =>
        {
            int temp = int.Parse(str);
            m_BgmSlider.value = temp;
            Mgr_Sound.Instance.SetSoundVolume(SoundType.Bgm, (float)temp / 100f);
        });
        m_EmSize.onEndEdit.AddListener((string str) =>
        {
            int temp = int.Parse(str);
            m_EmSlider.value = int.Parse(str);
            Mgr_Sound.Instance.SetSoundVolume(SoundType.Effect, (float)temp / 100f);
        });
    }

    private void Refresh()
    {
        foreach (var item in m_KeyCellList)
        {
            KeyBorad keyBorad = (KeyBorad)Enum.Parse(typeof(KeyBorad), item.m_TitleKeyName.text.ToString());
            KeyCode keyCode = Mgr_DataSave.Instance.m_GameData.InputData[keyBorad];
            if (keyCode.Equals(KeyCode.None)) item.m_BtnKeyName.text = string.Empty;
            else item.m_BtnKeyName.text = keyCode.ToString();
        }
        m_BgmSlider.value = Mgr_Sound.Instance.m_GlobleBgmVolume * 100f;
        m_EmSlider.value = Mgr_Sound.Instance.m_GlobleEmVolume * 100f;
        m_BgmSize.text = m_BgmSlider.value.ToString();
        m_EmSize.text = m_EmSlider.value.ToString();
    }

    private void AddSubKeyboradPageTitle(GameObject Obj, string info)
    {
        GameObject typeName = Instantiate(Obj);
        typeName.transform.SetParent(m_SubPageKeyBorad.content, false);
        Text titleName = typeName.GetComponentInChildren<Text>();
        titleName.text = info;
    }

    private void AddChangedKey(KeyBorad keyBorad, KeyCode keyCode)
    {
        foreach (var item in m_KeyCellList)
        {
            if (item.m_BtnKeyName.text.Equals(keyCode.ToString()))
            {
                KeyBorad keyName = (KeyBorad)Enum.Parse(typeof(KeyBorad), item.m_TitleKeyName.text);
                if (m_TempChangeKeys.ContainsKey(keyName)) m_TempChangeKeys[keyName] = KeyCode.None;
                else m_TempChangeKeys.Add(keyName, KeyCode.None);
                item.m_BtnKeyName.text = string.Empty;
            }
        }
        if (m_TempChangeKeys.ContainsKey(keyBorad)) m_TempChangeKeys[keyBorad] = keyCode;
        else m_TempChangeKeys.Add(keyBorad, keyCode);
        m_CurrentCell.m_BtnKeyName.text = keyCode.ToString();
    }
}
