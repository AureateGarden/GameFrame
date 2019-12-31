using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [Header("Base Optional")]
    public UIName m_UIName;
    public bool m_IsCache = true;
    public UIType m_Type { get; protected set; }

    public virtual void Init()
    {
        m_Type = UIType.Window;
    }

    public virtual void OnOpen()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    public virtual void OnClose()
    {
        transform.SetAsFirstSibling();
        if (m_IsCache) gameObject.SetActive(false);
        else Destroy(gameObject);
    }
}
