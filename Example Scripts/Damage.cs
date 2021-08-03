using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage = 1.0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mortal"))
            other.gameObject.SendMessage("TakeDamage", damage);
        // Calls a function TakeDamage(damage) belonging to the other gameobject
    }
}