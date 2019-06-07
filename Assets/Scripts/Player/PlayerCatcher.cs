using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatcher : MonoBehaviour
{
    public Transform respawn;

    Cinemachine.CinemachineVirtualCamera cameraController;

    void Start()
    {
        cameraController = Camera.main.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (transform.position.y < -30)
        {
            transform.position = respawn.position;
            cameraController.Follow = respawn;
        }
        else
        {
            cameraController.Follow = gameObject.transform;
        }
    }
}
