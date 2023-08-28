using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DummyScript : MonoBehaviour
{
    public enum Type { Dummy, Wolf, Player}

    public Type type;
    public GameObject respawnPoint;
    public TextMeshPro text;
    public int hp = 100;
    public int maxHp = 100;
    public int lvl;
    public Transform mainCamera;
    public GameObject DeathEffect;
    public bool regenerateHP = false;
    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.text = "LVL " + lvl + " " + hp + "/" + maxHp;

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
        text.text = "LVL " + lvl + " " + hp + "/" + maxHp;
        if (type == Type.Player)
        {
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
                transform.parent.gameObject.GetComponent<EnemyAI>().enemySpawner.ResetMe(number);
                Destroy(transform.parent.gameObject);
            }
        }

        text.text = "LVL " + lvl + " " + hp + "/" + maxHp;
    }

    public void RegenerateHealthPoints()
    {
        hp += lvl;
        if (hp > maxHp) hp = maxHp;

        regenerateHP = false;
    }

}
