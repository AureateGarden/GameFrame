using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIName
{
    UI_Start,
    UI_Option,
    UI_Game,
    UI_OnLoad,
    UI_Check,
    UI_GameMenu,
}

public enum UIType
{
    Window,
    PopUp,
    Icon,
}

public class Mgr_UI : MonoBehaviour
{
    private static Mgr_UI _instance;
    public static Mgr_UI Instance
    {
        get
        {
            if (_instance == null) _instance = GameManager.Instance.gameObject.AddComponent<Mgr_UI>();
            return _instance;
        }
    }

    private Dictionary<UIName, UIBase> m_OpenedUIs;

    public UIBase m_CurWindow { get; private set; }

    public void Init()
    {
        m_OpenedUIs = new Dictionary<UIName, UIBase>();
    }

    public T ToUI<T>(UIName uiName) where T : UIBase
    {
        UIBase TempUI;
        m_OpenedUIs.TryGetValue(uiName, out TempUI);
        T ui = TempUI as T;
        if (TempUI == null)
        {
            GameObject uiObj = Mgr_AssetBundle.Instance.LoadAsset<GameObject>(ABTypes.prefab, uiName.ToString());
            ui = Instantiate(uiObj).GetComponent<T>();
            ui.Init();
            switch (ui.m_Type)
            {
                case UIType.Window:
                    ui.transform.SetParent(GameManager.Instance.m_MainUI.m_Sorts[0], false);
                    break;
                case UIType.PopUp:
                    ui.transform.SetParent(GameManager.Instance.m_MainUI.m_Sorts[1], false);
                    break;
                case UIType.Icon:
                    ui.transform.SetParent(GameManager.Instance.m_MainUI.m_Sorts[2], false);
                    break;
            }
            m_OpenedUIs.Add(uiName, ui);
        }
        if (ui.m_Type.Equals(UIType.Window))
        {
            List<UIName> CloseList = new List<UIName>();
            foreach (var item in m_OpenedUIs.Values)
            {
                if (item.m_Type.Equals(UIType.PopUp)) CloseList.Add(item.m_UIName);
            }
            foreach (var item in CloseList)
            {
                UIBase desUi;
                if (!m_OpenedUIs.TryGetValue(item, out desUi)) continue;
                if (!desUi.m_IsCache) RemoveOpenedUI(uiName);
                desUi.OnClose();
            }
        }
        ui.OnOpen();
        if (ui.m_Type.Equals(UIType.Window))
        {
            if (m_CurWindow != null && !m_CurWindow.m_UIName.Equals(ui.m_UIName)) m_CurWindow.OnClose();
            m_CurWindow = ui;
        }
        return ui;
    }

    public PopBase GetPopPageTopUI()
    {
        Transform PopPages = GameManager.Instance.m_MainUI.m_Sorts[1];
        if (GameManager.Instance.m_MainUI.m_Sorts[1].childCount > 0)
        {
            return PopPages.GetChild(PopPages.childCount - 1).GetComponent<PopBase>();
        }

        return null;
    }

    public bool RemoveOpenedUI(UIName uiName)
    {
        return m_OpenedUIs.Remove(uiName);
    }
}
