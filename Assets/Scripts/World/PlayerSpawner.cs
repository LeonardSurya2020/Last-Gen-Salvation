using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
    [SerializeField] CinemachineVirtualCamera virtualCamera; // Referensi ke Cinemachine Virtual Camera
    private CameraFollow cameraFollow;
    // Start is called before the first frame update
    void Start()
    {
        // Spawn player di posisi spawner
        Instantiate(playerObject, transform.position, Quaternion.identity);

        // Atur Cinemachine Virtual Camera untuk mengikuti Player yang baru di-spawn
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerObject.transform;
            cameraFollow = virtualCamera.GetComponent<CameraFollow>();
            cameraFollow.setCamera();
        }
        else
        {
            Debug.LogWarning("Cinemachine Virtual Camera belum di-assign!");
        }
    }
}
