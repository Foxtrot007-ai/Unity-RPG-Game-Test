using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
    public List<Collider> targets = new List<Collider>();
    public int DamageAmount;
 

    public void LookAtTarget(Transform Player)
    {
        foreach (Collider col in targets)
        {
            if (col != null)
            {
                Player.LookAt(col.transform);
                return;
            }
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
                col.GetComponentInChildren<DummyScript>().DamageIt(DamageAmount);
            }    
        }
        targets.RemoveAll(t => t == null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dummy")
        {
            targets.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Dummy")
        {
            targets.Remove(other);
        }
    }
}
