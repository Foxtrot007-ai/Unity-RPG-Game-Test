using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DummyScript : MonoBehaviour
{
    public enum Type { Dummy, Wolf, Player}

    public Type type;
    public bool IamBoss = false;
    public GameObject respawnPoint;
    public TextMeshPro text;
    public int hp = 100;
    public int maxHp = 100;
    public int lvl;
    public Transform mainCamera;
    public GameObject DeathEffect;
    public GameObject defeatedInfo;
    public bool regenerateHP = false;
    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        string temp = "LVL " + lvl + " " + hp + "/" + maxHp + " HP";

        if (type == Type.Player) {
            int exp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().experiencePoints;
            int limit = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().experienceLimit;
            temp += "\n" + exp + "/" + limit + " EXP";
        }

        text.text = temp;

        if (IamBoss)
        {
            defeatedInfo = GameObject.FindGameObjectWithTag("DefeatedInfo");
            defeatedInfo.SetActive(false);
        }

        if (type == Type.Player)
        {
            maxHp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().level * 80;
        }

        if (type == Type.Wolf)
        {
            maxHp = GetComponentInParent<EnemyAI>().level * 100;
        }
        hp = maxHp;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private void Update()
    {
        string temp = "LVL " + lvl + " " + hp + "/" + maxHp + " HP";

        if (type == Type.Player)
        {
            int exp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().experiencePoints;
            int limit = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().experienceLimit;
            temp += "\n" + exp + "/" + limit + " EXP";
            lvl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().level;
            if (!regenerateHP)
            {
                regenerateHP = true;
                Invoke("RegenerateHealthPoints", 1f);
            }
        }

        if (type == Type.Wolf)
        {
            lvl = GetComponentInParent<EnemyAI>().level;
            if (GetComponentInParent<EnemyAI>().restoreHp)
            {
                Debug.Log("restore");
                hp = maxHp;
                GetComponentInParent<EnemyAI>().restoreHp = false;
            }
        }
        text.text = temp;
        transform.LookAt(mainCamera);

    }
    public void DamageIt(int DamageAmount)
    {

        if (type == Type.Player)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>().animationState == 3)
            {
                hp -= DamageAmount / 2;
            }
            else
            {
                hp -= DamageAmount;
            }
        }
        else
        {
            hp -= DamageAmount;
        }


        if (hp <= 0)
        {
            if(type == Type.Player)
            {
                hp = maxHp;
                GameObject temp = GameObject.FindGameObjectWithTag("Player");
                if (temp.GetComponent<PlayerManager>().inBossFight)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                }
                else
                {
                    temp.GetComponent<Rigidbody>().MovePosition(respawnPoint.transform.position);
                }
            }

            if (type == Type.Wolf)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().AddExperiencePoints(GetComponentInParent<EnemyAI>().level);
               // GameObject temp = Instantiate(DeathEffect, transform.position, transform.rotation);
                //temp.transform.localScale = new Vector3(2, 2, 2);
               // Destroy(temp, 0.3f);
                int number = transform.parent.gameObject.GetComponent<EnemyAI>().myNumber;
                if (IamBoss)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>().bossDefeated = 1;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().ReturnHome();
                    defeatedInfo.SetActive(true);
                    PlayerPrefs.SetInt("BossDefeated", 1);
                }
                else
                {
                    transform.parent.gameObject.GetComponent<EnemyAI>().enemySpawner.ResetMe(number);
                }
                Destroy(transform.parent.gameObject);
            }
        }

        text.text = "LVL " + lvl + " " + hp + "/" + maxHp + " HP";
    }

    public void RegenerateHealthPoints()
    {
        hp += lvl;
        if (hp > maxHp) hp = maxHp;

        regenerateHP = false;
    }

}
