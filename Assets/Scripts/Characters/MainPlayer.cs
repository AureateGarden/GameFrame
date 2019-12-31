using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputMainPlayer))]
public class MainPlayer : CharacterBase
{
    public Vector2 m_GroundCheckPoint;
    public float m_CheckRadius;
    public Vector2 m_Direction { get; set; }
    public Rigidbody2D m_Rig { get; private set; }

    private InputMainPlayer m_Input;
    //private bool m_IsGround;
    private const float m_MoveSpeed = 200f;
    private const float m_MaxSpeed = 5f;
    private const float m_JumpForce = 200f;


    private void FixedUpdate()
    {
        LimitMaxSpeed();
    }

    public override void Init()
    {
        m_Rig = GetComponent<Rigidbody2D>();
        m_Input = GetComponent<InputMainPlayer>();
        //m_IsGround = false;
        OnSpown();
        m_Input.Init();
    }

    public override void OnSpown()
    {
        m_Input.m_MoveEvent += Move;
        m_Input.m_JumpEvent += Jump;
        m_Input.m_AttackEvent += Attack;
        m_Input.m_SkillEvent += Skill;
        base.OnSpown();
    }

    public override void OnDead()
    {
        m_Input.m_MoveEvent -= Move;
        m_Input.m_JumpEvent -= Jump;
        m_Input.m_AttackEvent -= Attack;
        m_Input.m_SkillEvent -= Skill;
        base.OnDead();
    }

    public void Move()
    {
        m_Rig.velocity = new Vector2(m_Direction.x * m_MoveSpeed * Time.deltaTime, m_Rig.velocity.y);
    }

    public void Jump()
    {
        m_Rig.AddForce(Vector2.up * m_JumpForce);
    }

    public void Attack()
    {

    }

    public void Skill()
    {

    }

    private void GroundCheck()
    {
        Collider2D collider = Physics2D.OverlapCircle(m_GroundCheckPoint, m_CheckRadius);
    }

    private void LimitMaxSpeed()
    {
        if (m_Rig.velocity.magnitude > m_MaxSpeed) m_Rig.velocity = m_Rig.velocity.normalized * m_MaxSpeed;
    }
}
