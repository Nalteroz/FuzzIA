using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Projectil, Enemy;
    public float ProjVelocity = 1;
    public float ShootRate = 0.5f;

    float LastTimeShooted = 0;
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Aim(Enemy);
        Shoot();
	}

    void Aim(GameObject target)
    {
        Vector2 AimDirection = (target.transform.position - transform.position).normalized;
        transform.up = AimDirection;
    }
    void Shoot()
    {
        if ((Time.time - LastTimeShooted) >= ShootRate)
        {
            GameObject Bullet = Instantiate(Projectil, transform.position, Quaternion.identity);
            Bullet.GetComponent<Rigidbody2D>().velocity = transform.up * ProjVelocity;
            Destroy(Bullet, 10f);
            LastTimeShooted = Time.time;
        }
    }
}
