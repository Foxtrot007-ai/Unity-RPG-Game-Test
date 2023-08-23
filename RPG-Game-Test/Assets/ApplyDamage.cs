using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
    public InputManager inputManager;
    public bool canDamage = true;
    public bool alreadyDamaged = false;
    public Collider target;
    private void Awake()
    {
        target = null;
        inputManager = FindObjectOfType<InputManager>();
    }
    public void DamageIt()
    {
        if(target != null)
        {
            target.GetComponentInChildren<DummyScript>().DamageIt();
            target = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dummy")
        {
            target = other;
            canDamage = true;
        }
        else canDamage = false;
    }
    private void OnTriggerExit(Collider other)
    {
        canDamage = false;
    }
}
