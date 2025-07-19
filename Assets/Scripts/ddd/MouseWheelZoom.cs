using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWheelZoom : MonoBehaviour
{

    public CinemachineFreeLook cinemachineFreeLook;
    public float zoomSpeed;
    // Start is called before the first frame update
    void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        cinemachineFreeLook.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cinemachineFreeLook.enabled = cinemachineFreeLook.isActiveAndEnabled ? false : true;

        }

        if (cinemachineFreeLook.isActiveAndEnabled)
        {
            float scrollInpout = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInpout != 0)
            {
                cinemachineFreeLook.m_Lens.FieldOfView += scrollInpout * zoomSpeed * Time.deltaTime;
            }
        }

    }
}
