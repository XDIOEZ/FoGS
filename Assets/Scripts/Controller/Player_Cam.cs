using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Cam : MonoBehaviour
{
    [Header("ÐéÄâÉãÏñ»ú")]
    public CinemachineVirtualCamera virtualCamera;

    
    public void StartWork()
    {
        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 300;
        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 300;
       
    }

    public void StopWork()
    {
        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
    }

    public void Update()
    {
       if(!PlayerMouseLock.IsLocked)
        {
            StopWork();
        }
        else
        {
            StartWork();
        }

    }
}
