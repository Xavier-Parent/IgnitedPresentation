using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private Ray m_LastRay;
    private NavMeshAgent m_Agent;
    [SerializeField]
    private Camera m_MainCamera;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        RayCast();
    }

    private void RayCast()
    {
        if(Input.GetMouseButtonDown(0))
        {
            m_LastRay = m_MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit m_RayCastInfo;
            if(Physics.Raycast(m_LastRay, out m_RayCastInfo))
            { 
                m_Agent.SetDestination(m_RayCastInfo.point);
            }

        }
    }
}
