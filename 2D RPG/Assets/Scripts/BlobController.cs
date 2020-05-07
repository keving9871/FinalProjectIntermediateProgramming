using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour
{
    //Variables:
    public int health;
    public GameObject gun;
    public GameObject bullet;
    public Transform firepoint;
    Vector3 startPos;
    Transform trans;
    public AudioSource audioSource;
    public AudioClip bang;
    public float waitTime;
    public bool hasShot;
    private float timer, secondaryTimer;
    public Transform gunSpawnPoint;
    public ParticleSystem celebrateParticles;

    void Start()
    {
        //Set Health:
        health = 10;

        //Set Starting Position:
        trans = GetComponent<Transform>();
        startPos = trans.position;

        //Make sure that audiosource has the component
        audioSource = GetComponent<AudioSource>();

        hasShot = false;
        timer = 0f;
        secondaryTimer = 0f;
    }


    void Update()
    {
        //Move back and forth
        trans.position = new Vector3(startPos.x, startPos.y + Mathf.PingPong(Time.time, 5.5f), startPos.z);

        //Die when health is 0
        if (health <= 0)
        {
            Destroy(this.gameObject);

            //Drop Gun:
            Instantiate(gun, gunSpawnPoint.position, transform.rotation);
            gun.tag = "Gun";

            //Create particles to celebrate:
            Instantiate(celebrateParticles, gunSpawnPoint.position, transform.rotation);
        }

        timer += Time.deltaTime;
        secondaryTimer += Time.deltaTime;

        //Fire at player:
        if (secondaryTimer <= 5f)
        {
            if (timer >= .75f)
            {
                StartCoroutine(Shoot());
                hasShot = true;
                timer = 0f;
                secondaryTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("friendly") || collision.CompareTag("Sword"))
        {
            //Lower Health
            health -= 1;

            //Destroy the bullet its been hit by
            if (collision.CompareTag("friendly"))
            {
                Destroy(collision);
            }
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(.5f);
        hasShot = false;
        Instantiate(bullet, firepoint.position, Quaternion.Euler(0, 0, 90));
        audioSource.PlayOneShot(bang);
        
    }
}
