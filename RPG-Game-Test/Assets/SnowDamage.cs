using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowDamage : MonoBehaviour
{
    public GameObject snowEffect;
    public List<Collider> targets = new List<Collider>();
    public int DamageAmount;
    public bool alreadyDamaged = false;
    private void Awake()
    {
        alreadyDamaged = false;
    }

    private void Update()
    {
        if (!alreadyDamaged)
        {
            alreadyDamaged = true;
            MakeSnow();
            UseIt(2f, DamageAmount);
        }
    }

    public void UseIt(float time, int damage)
    {
        DamageAmount = damage;
        Invoke("DamageIt", time);
    }

    public void DamageIt()
    {
        foreach (Collider col in targets)
        {
            if (col != null)
            {
                if (col.tag == "Player")
                {
                    col.GetComponentInChildren<DummyScript>().DamageIt(DamageAmount);
                }
                else
                {
                    col.GetComponentInChildren<DummyScript>().RegenerateHealthPoints();
                }
                
            }
        }
        targets.RemoveAll(t => t == null);
        alreadyDamaged = false;
    }
    private void MakeSnow()
    {
        GameObject temp = Instantiate(snowEffect, transform.position - new Vector3(0,11,0), transform.rotation);
        temp.transform.localScale = new Vector3(3, 3, 3);
        Destroy(temp, 2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Dummy")
        {
            targets.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Dummy")
        {
            targets.Remove(other);
        }
    }

}
