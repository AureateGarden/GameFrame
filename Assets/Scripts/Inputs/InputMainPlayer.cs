using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MainPlayer))]
public class InputMainPlayer : InputBase
{
    public InputHandler m_MoveEvent;
    public InputHandler m_JumpEvent;
    public InputHandler m_AttackEvent;
    public InputHandler m_SkillEvent;
    private MainPlayer m_Player;

    public bool m_MoveKeyDown { get; private set; }

    public override void Init()
    {
        base.Init();
        m_Player = GetComponent<MainPlayer>();
        m_MoveKeyDown = false;
    }

    private void FixedUpdate()
    {
        if (m_MoveKeyDown) m_MoveEvent();
    }

    private void Update()
    {
        if (Input.GetKey(m_Keys[KeyBorad.ArrowLeft]) && !Input.GetKey(m_Keys[KeyBorad.ArrowRight]))
        {
            m_Player.m_Direction = new Vector2(-1, m_Player.m_Direction.y).normalized;
            m_MoveKeyDown = true;
        }
        else if (Input.GetKey(m_Keys[KeyBorad.ArrowRight]) && !Input.GetKey(m_Keys[KeyBorad.ArrowLeft]))
        {
            m_Player.m_Direction = new Vector2(1, m_Player.m_Direction.y).normalized;
            m_MoveKeyDown = true;
        }
        else
        {
            m_MoveKeyDown = false;
            m_Player.m_Direction = new Vector2(0, m_Player.m_Direction.y).normalized;
        }
        if (Input.GetKey(m_Keys[KeyBorad.ArrowUp]))
        {
            m_Player.m_Direction = new Vector2(m_Player.m_Direction.x, 1).normalized;
        }
        else if (Input.GetKey(m_Keys[KeyBorad.ArrowDown]))
        {
            m_Player.m_Direction = new Vector2(m_Player.m_Direction.x, -1).normalized;
        }
        else
        {
            m_Player.m_Direction = new Vector2(m_Player.m_Direction.x, 0).normalized;
        }
        if (Input.GetKeyDown(m_Keys[KeyBorad.Attack])) m_AttackEvent();
        if (Input.GetKeyDown(m_Keys[KeyBorad.Jump])) m_JumpEvent();
    }
}