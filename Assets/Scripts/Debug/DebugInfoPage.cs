using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfoPage : MonoBehaviour
{
    public Font m_TextFont;
    public GameObject m_DetailedPage;
    public Text m_DetailedTitle;
    public Text m_DetailedInfo;
    public ScrollRect m_ScrollList;
    private Dictionary<string, DebugInfo> m_DebugDic;

    public class DebugInfo
    {
        public Text m_Title;
        public string m_Name;
        public string m_Info;

        public DebugInfo(string name, string info, Text text)
        {
            m_Title = text;
            m_Name = name;
            m_Info = info;
        }
    }

    public void Init()
    {
        m_DebugDic = new Dictionary<string, DebugInfo>();
    }

    public void ShowDebugInfo(string name, string info)
    {
        GameManager.Instance.m_MainUI.m_BtnDebug.SetActive(true);
        if (!m_DebugDic.ContainsKey(name))
        {
            GameObject Obj = new GameObject();
            Obj.layer = 5;
            Text tex = Obj.AddComponent<Text>();
            Button btn = Obj.AddComponent<Button>();
            btn.transition = Button.Transition.None;
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.None;
            btn.navigation = nav;
            tex.gameObject.name = "DebugInfo";
            tex.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 40);
            tex.font = m_TextFont;
            tex.fontSize = 20;
            tex.alignment = TextAnchor.MiddleLeft;
            tex.color = Color.white;
            tex.text = name;
            tex.transform.SetParent(m_ScrollList.content, false);
            DebugInfo debugInfo = new DebugInfo(name, info, tex);
            m_DebugDic.Add(name, debugInfo);
            btn.onClick.AddListener(() =>
            {
                tex.text = name;
                tex.color = Color.white;
                DebugInfo strInfo;
                if (m_DebugDic.TryGetValue(name, out strInfo)) m_DetailedInfo.text = strInfo.m_Info;
                else m_DetailedInfo.text = "failed to update info.";
                m_DetailedTitle.text = name;
                m_DetailedPage.SetActive(true);
            });
        }
        else
        {
            DebugInfo strInfo;
            if (m_DebugDic.TryGetValue(name, out strInfo))
            {
                if (strInfo.m_Info != info)
                {
                    strInfo.m_Info = info;
                    strInfo.m_Title.text = name + " *new";
                    strInfo.m_Title.color = Color.red;
                }
            }
            else m_DetailedInfo.text = "failed to update info.";
        }
    }
}
