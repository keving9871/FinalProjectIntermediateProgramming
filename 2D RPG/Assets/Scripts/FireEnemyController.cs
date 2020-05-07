using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyController : MonoBehaviour
{
    //Variables:
    public int health;
    public GameObject coin;

    Vector3 startpos;
    Transform trans;

    void Start()
    {
        //Set Health:
        health = 2;

        //Set Starting Position:
        trans = GetComponent<Transform>();
        startpos = trans.position;
    }
 
    void Update()
    {
        //Move back and forth
        trans.position = new Vector3(startpos.x + Mathf.PingPong(Time.time, 3), startpos.y, startpos.z);

        //Die when health is 0
        if(health <= 0)
        {
            Destroy(this.gameObject);

            //Drop Coin:
            Instantiate(coin, transform.position, transform.rotation);
            coin.tag = "Coin";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("friendly") || collision.CompareTag("Sword"))
        {
            health -= 1;

            if (collision.CompareTag("friendly"))
            {
                Destroy(collision);
            }
        }
    }
}
