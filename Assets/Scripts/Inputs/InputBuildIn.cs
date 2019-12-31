using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuildIn : InputBase
{
    public InputHandler m_EscapeEvent;

    public override void Init()
    {
        base.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_Keys[KeyBorad.Escape]))
        {
            m_EscapeEvent();
        }
    }
}