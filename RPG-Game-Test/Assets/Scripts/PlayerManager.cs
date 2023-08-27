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
    public int experiencePoints = 0;
    public int experienceLimit = 100;
    public int level = 1;

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
    public void AddExperiencePoints(int enemyLvl)
    {
        experiencePoints += enemyLvl * 10;
        if(experiencePoints > experienceLimit)
        {
            experiencePoints = 0;
            experienceLimit += 50;
            level += 1;
            GetComponentInChildren<DummyScript>().maxHp = level * 80;
        }
    }
}
