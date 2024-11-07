using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook freeLook;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        freeLook = GetComponent<CinemachineFreeLook>();
        StartCoroutine(CheckGamepadPresence());
    }

    IEnumerator CheckGamepadPresence()
    {
        while (true)
        {
            if (gameManager.IsGamepadPresent())
            {
                freeLook.m_YAxis.m_MaxSpeed = 2;
                freeLook.m_YAxis.m_AccelTime = 0.2f;
                freeLook.m_XAxis.m_MaxSpeed = 200;

                freeLook.m_XAxis.m_InputAxisName = "Mouse X Gamepad";
                freeLook.m_YAxis.m_InputAxisName = "Mouse Y Gamepad";
            }
            else
            {
                freeLook.m_YAxis.m_MaxSpeed = 4;
                freeLook.m_YAxis.m_AccelTime = 0.4f;
                freeLook.m_XAxis.m_MaxSpeed = 600;

                freeLook.m_XAxis.m_InputAxisName = "Mouse X";
                freeLook.m_YAxis.m_InputAxisName = "Mouse Y";
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}