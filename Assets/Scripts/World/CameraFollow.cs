using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

    }

    public void setCamera()
    {
        // Cari Player di scene setelah scene diload
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            virtualCamera.Follow = player.transform;
        }
    }
}
