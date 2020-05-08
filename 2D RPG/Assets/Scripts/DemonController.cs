using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    //Variables:
    public int health;
    public GameObject coin;
    public float moveSpeed;
    public float timeToChangeDirection;
    private float timeSinceLastDirectionChange;
    private Vector2 randomDirection;
    private Vector2 movementSpeed;

    void Start()
    {
        //Start at 0
        timeSinceLastDirectionChange = 0f;
    }

    void chooseRandomDirection()
    {
        //Make a random direction
        randomDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;

        //make a vector2 that uses movespeed and the current direction
        movementSpeed = randomDirection * moveSpeed;
    }

    void Update()
    {
        //Compare the current and how long to wait before changing and then change the direction to a random direction
        if(Time.time - timeToChangeDirection > timeToChangeDirection)
        {
            timeSinceLastDirectionChange = Time.time;
            chooseRandomDirection();
        }

        //Apply the random movements:
        transform.position = new Vector2(transform.position.x + (movementSpeed.x * Time.deltaTime), 
        transform.position.y + (movementSpeed.y * Time.deltaTime));

        //Die when health is 0
        if (health <= 0)
        {
            Destroy(this.gameObject);

            //Drop Coin:
            Instantiate(coin, transform.position, transform.rotation);
            coin.tag = "Coin";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("friendly") || collision.CompareTag("Sword"))
        {
            Debug.Log("Hit by sword");
            health -= 1;

            if (collision.CompareTag("friendly"))
            {
                Destroy(collision);
            }
        }
    }
}
