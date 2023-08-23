using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public TextMeshPro text;
    public int hp = 100;
    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        text.text = hp + "/100";
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
