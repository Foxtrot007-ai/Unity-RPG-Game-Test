using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;
    CameraManager cameraManager;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponent<AnimatorManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        if(!inputManager.usingMoveStarted) playerLocomotion.HandleAllMovements();
    }
    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
    }
}
