using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;

    public CinemachineVirtualCamera zoomin;
    public CinemachineVirtualCamera zoomout;

    public CinemachineVirtualCamera startCam;
    private CinemachineVirtualCamera currentCam;

    private void Start()
    {
        currentCam = startCam;
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] == currentCam)
            {
                cameras[i].Priority = 20;
            }
            else
            {
                cameras[i].Priority = 10;
            }
        }
    }

    public void SwitchCamera(CinemachineVirtualCamera newCam)
    {
        Debug.Log($"Switching to camera: {newCam.name}");
        currentCam = newCam;
        currentCam.Priority = 20;
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != currentCam)
            {
                cameras[i].Priority = 10;
            }
        }
    }
}
