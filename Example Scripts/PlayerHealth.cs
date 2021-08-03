using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 10;
    private float health;
    public XRRig rig;
    public Transform deathLocation;
    public Transform respawnLocation;
    public float respawnDelay;
    private bool invincible = false;
    public float iTimer = 1;

    public void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!invincible)
        {
            invincible = true;
            health -= damage;
            Debug.Log("Player taking damage, health: " + health);
            if (health <= 0)
            {
                Debug.Log("Player is dead");
                rig.transform.position = deathLocation.position;
                Invoke(nameof(Respawn), respawnDelay);
            }
            Invoke(nameof(Vulnerable), iTimer);
        }
    }

    private void Vulnerable()
    {
        invincible = false;
    }

    private void Respawn()
    {
        rig.transform.position = respawnLocation.position;
        health = maxHealth;
        Debug.Log("Restored health: " + health);
    }
}