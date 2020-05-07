using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossController : MonoBehaviour
{
    //Variables:
    private GameObject player;
    private GameObject boss;
    private float range, moveSpeed;
    public int health;
    public GameObject Key;
    public Transform keySpawnPoint;
    public ParticleSystem celebrateParticles;
    public TextMeshProUGUI bossHealth;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Enemy");

        //Set health:
        health = 25;

        //Set Move Speed:
        moveSpeed = 1.6f;

        bossHealth.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Get the distance from the boss to the player and store it in range variable
        range = Vector2.Distance(boss.transform.position, player.transform.position);

        //If the player is close enough execute block
        if(range < 10f)
        {
            //Move towards player
            Vector2 velocity = new Vector2((transform.position.x - player.transform.position.x) * moveSpeed, (transform.position.y - player.transform.position.y) * moveSpeed);
            GetComponent<Rigidbody2D>().velocity = -velocity;
        }

        //Killable:
        if (health <= 0)
        {
            //Delete Boss:
            Destroy(this.gameObject);

            //Drop Key:
            Instantiate(Key, keySpawnPoint.position, transform.rotation);
            Key.tag = "Key";

            //Create particles to celebrate:
            Instantiate(celebrateParticles, keySpawnPoint.position, transform.rotation);
        }

        //Display Current Health
        bossHealth.text = "Boss Health: " + health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("friendly") || collision.CompareTag("Sword"))
        {
            health -= 1;

            if (collision.CompareTag("friendly"))
            {
                Destroy(collision);
            }
        }
    }
}
