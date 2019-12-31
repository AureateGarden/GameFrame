using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public Transform[] m_Sorts;
    public GameObject m_BtnDebug;
    public DebugInfoPage m_DebugInfo { get; private set; }
    private InputBuildIn m_Input;

    public void Init()
    {
        m_Input = GetComponentInChildren<InputBuildIn>();
        m_Input.Init();
        InitDebugInfo();

        m_Input.m_EscapeEvent += EscFunc;
    }

    private void EscFunc()
    {
        PopBase pop = Mgr_UI.Instance.GetPopPageTopUI();
        if (pop != null && pop.gameObject.activeSelf)
        {
            pop.OnClose();
            return;
        }
        if (Mgr_UI.Instance.m_CurWindow.m_UIName.Equals(UIName.UI_Game))
        {
            Mgr_UI.Instance.ToUI<UI_GameMenu>(UIName.UI_GameMenu);
        }


    }

    private void InitDebugInfo()
    {
        GameObject Debuger = Mgr_AssetBundle.Instance.LoadAsset<GameObject>(ABTypes.prefab, "DebugPage");
        m_DebugInfo = Instantiate(Debuger).GetComponent<DebugInfoPage>();
        m_DebugInfo.gameObject.SetActive(false);
        m_DebugInfo.transform.SetParent(m_Sorts[2], false);
        m_DebugInfo.transform.SetSiblingIndex(m_Sorts[2].childCount - 2);
        m_DebugInfo.Init();
        RectTransform DebugTrans = m_DebugInfo.GetComponent<RectTransform>();
        DebugTrans.anchoredPosition = Vector2.zero;
    }

    public void OpenDebugInfo()
    {
        m_DebugInfo.gameObject.SetActive(true);
        m_BtnDebug.SetActive(false);
    }
}
