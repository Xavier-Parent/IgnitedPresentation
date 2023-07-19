using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform m_Player;
    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float m_SmoothFactor;
    [SerializeField]
    private float m_RotationSpeed;
    [SerializeField]
    private float m_MoveSpeed;
    [SerializeField]
    private float m_MinDistance;
    [SerializeField]
    private float m_MaxDistance;
    
    private Vector3 m_Offset;
    void Start()
    {
        m_Offset = transform.position - m_Player.position;
    }
    void Update()
    {
        Vector3 m_NewPosition = m_Player.position + m_Offset;
        transform.position = Vector3.Slerp(transform.position, m_NewPosition, m_SmoothFactor);

        transform.LookAt(m_Player);
        if (Input.GetMouseButton(1))
        {
            Quaternion m_CamTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * m_RotationSpeed, Vector3.up);
            m_Offset = m_CamTurnAngle * m_Offset;
        }
        float moveAmount = Input.GetAxis("Mouse ScrollWheel") * m_MoveSpeed;
        float distance = Vector3.Distance(transform.position, m_Player.position);
        float newDistance = Mathf.Clamp(distance - moveAmount, m_MinDistance, m_MaxDistance);
        m_Offset = m_Offset.normalized * newDistance;
    }
}
