using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : MonoBehaviour {
    public GameObject projectile;

    void Start()
    {
        Invoke("LaunchProjectile", 2.0f);
    }

    void LaunchProjectile()
    {
        float randomTime = Random.Range(1, 3);
        Instantiate(projectile, transform.position, Quaternion.identity);
        Invoke("LaunchProjectile", randomTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy Object")
        {
            Destroy(gameObject);
        }
    }
}
