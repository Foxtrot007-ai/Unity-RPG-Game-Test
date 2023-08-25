using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
    public InputManager inputManager;
    public Collider target;
    private void Awake()
    {
        target = null;
        inputManager = FindObjectOfType<InputManager>();
    }

    public void UseIt(float time)
    {
        Invoke("DamageIt", time);
    }
    public void DamageIt()
    {
        if(target != null)
        {
            target.GetComponentInChildren<DummyScript>().DamageIt();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dummy")
        {
            target = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Dummy")
        {
            target = null;
        }
    }
}
