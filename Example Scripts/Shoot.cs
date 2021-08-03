using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float speed = 40;
    public GameObject projectile;
    public Transform barrel;
    public float lifeTime = 2;
    public AudioSource audioSource = null;
    public AudioClip audioClip = null;

    public void Fire()
    {
        GameObject spawnProjectile = Instantiate(projectile, barrel.position, barrel.rotation);
        spawnProjectile.GetComponent<Rigidbody>().velocity = speed * barrel.up; // Capsules and cylinders have "up" as their length
        // Change to barrel.forward or whichever direction works as needed.
        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);
        Destroy(spawnProjectile, lifeTime);
    }
}
