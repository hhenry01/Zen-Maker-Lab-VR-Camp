using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : MonoBehaviour
{
    public float speed = 40;
    public GameObject projectile;
    public Transform barrel;
    public int numberOfShots = 8;
    public float spreadDegree = 15;
    public float lifeTime = 2;
    public AudioSource audioSource = null;
    public AudioClip audioClip = null;

    private float spreadAngle;
    private void Start()
    {
        spreadAngle = spreadDegree * Mathf.Deg2Rad;
    }

    public void Fire()
    {
        GameObject spawnProjectile = Instantiate(projectile, barrel.position, barrel.rotation);
        spawnProjectile.GetComponent<Rigidbody>().velocity = speed * barrel.up; // Capsules and cylinders have "up" as their length
        Destroy(spawnProjectile, lifeTime);
        for (int i = 0; i < numberOfShots - 1; i++)
        {

            float angleX = Random.Range(-Mathf.Sin(spreadAngle), Mathf.Sin(spreadAngle));
            float angleY = Random.Range(-Mathf.Sin(spreadAngle), Mathf.Sin(spreadAngle));
            float angleZ = Random.Range(-Mathf.Sin(spreadAngle), Mathf.Sin(spreadAngle));

            Vector3 angleOffset = new Vector3(angleX, angleY, angleZ);
            Vector3 shotAngle = (barrel.up + angleOffset).normalized;
            spawnProjectile = Instantiate(projectile, barrel.position, Quaternion.Euler(angleOffset.normalized));
            spawnProjectile.GetComponent<Rigidbody>().velocity = speed * shotAngle;
            Destroy(spawnProjectile, lifeTime);
        }
        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);
    }

}
