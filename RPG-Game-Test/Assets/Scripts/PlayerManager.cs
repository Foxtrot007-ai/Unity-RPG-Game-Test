using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;
    CameraManager cameraManager;
    public int experiencePoints = 0;
    public int experienceLimit = 100;
    public int level = 1;
    public bool inBossFight = false;

    private void Awake()
    {
        level = PlayerPrefs.GetInt("Level", 1);
        experiencePoints = PlayerPrefs.GetInt("experiencePoints", 0);
        experienceLimit = 50 + level * 50;
        
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
        experiencePoints += enemyLvl * 5;
        if(experiencePoints > experienceLimit)
        {
            experiencePoints = 0;
            experienceLimit += 100;
            level += 1;
            PlayerPrefs.SetInt("Level", level);
            GetComponentInChildren<DummyScript>().maxHp = level * 80;
        }
        PlayerPrefs.SetInt("experiencePoints", experiencePoints);
    }

    public void ReturnHome()
    {
        Invoke("ChangeScene", 10f);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

 
}
