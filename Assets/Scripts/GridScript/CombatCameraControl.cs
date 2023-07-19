using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CombatCameraControl : MonoBehaviour
{
    public GameObject selectedUnit;
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera.Follow = selectedUnit.transform;
        virtualCamera.LookAt = selectedUnit.transform;
    }

    public void SwitchCameraTarget(GameObject newTarget)
    {
        selectedUnit = newTarget;

        virtualCamera.Follow = selectedUnit.transform;
        virtualCamera.LookAt = selectedUnit.transform;
    }
}
