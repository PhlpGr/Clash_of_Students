using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuerShoot : MonoBehaviour
{
    public GameObject firePrefab;  // Dein Feuer-Schuss Prefab
    public float timeBetweenShots = 2.5f; // Zeit in Sekunden zwischen den Schüssen
    public float flameSpeed = -5.0f; // Geschwindigkeit der Flamme
    private float nextFireTime = 0f; // Nächster Zeitpunkt, zu dem gefeuert wird

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            ShootFire();
            nextFireTime = Time.time + timeBetweenShots;
        }
    }

    void ShootFire()
    {
        // Erstelle eine neue Flamme
        GameObject fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
        // Setze die Geschwindigkeit der Flamme
        fire.GetComponent<Rigidbody2D>().velocity = new Vector2(flameSpeed, 0); 
        Destroy(fire, 3.0f);
    }
}

