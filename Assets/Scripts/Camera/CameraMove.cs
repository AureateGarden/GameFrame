using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float m_MoveSpeedTime;
    public bool m_IsMoving { get; private set; }
    private int m_UnitTimes;
    private Vector2 m_InCrement;

    private void FixedUpdate()
    {
        if (m_UnitTimes > 0)
        {
            transform.position = new Vector3(transform.position.x + m_InCrement.x,
            transform.position.y + m_InCrement.y,
            transform.position.z);
            if (--m_UnitTimes == 0)
            {
                m_IsMoving = false;
                m_InCrement = Vector2.zero;
            }
        }
    }

    public void MoveToTarget(Transform target)
    {
        if (target == null || m_UnitTimes > 0) return;
        Vector2 temp = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
        Vector2 direction = temp.normalized;
        float distance = temp.magnitude;
        m_UnitTimes = (int)(m_MoveSpeedTime / Time.fixedDeltaTime);
        float disPerFrame = distance / m_UnitTimes;
        m_InCrement = direction * disPerFrame;
        m_IsMoving = true;
        Debug.Log(m_UnitTimes);
    }
}