using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private void Awake()
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
        }

        if (type == Type.Wolf)
        {
            lvl = GetComponentInParent<EnemyAI>().level;
        }
        transform.LookAt(mainCamera);
    }
    public void DamageIt()
    {

        if (type == Type.Player)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>().animationState == 3)
            {
                hp -= 5;
            }
            else
            {
                hp -= 10;
            }
        }
        else
        {
            hp -= 20;
        }


        if (hp <= 0)
        {
            if(type == Type.Player)
            {
                hp = maxHp;
                GameObject temp = GameObject.FindGameObjectWithTag("Player");
                temp.GetComponent<Rigidbody>().MovePosition(respawnPoint.transform.position);
            }

            if (type == Type.Wolf)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().AddExperiencePoints(GetComponentInParent<EnemyAI>().level);
                Destroy(transform.parent.gameObject);
            }
        }

        text.text = "LVL " + lvl + " " + hp + "/" + maxHp;
    }

}
