using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public TextMeshPro text;
    public int hp = 100;
    public Transform player;
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.text = hp + "/100";
        player = GameObject.Find("Character").transform;
    }
    private void Update()
    {
        transform.LookAt(player);
    }
    public void DamageIt()
    {
        hp -= 10;
        if (hp < 0)
        {
            hp = 100;
        }
        text.text = hp + "/100";
    }

}
