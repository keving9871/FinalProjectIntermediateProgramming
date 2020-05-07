using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Variables:
    private float moveSpeed;
    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        moveSpeed = -2500f;
        StartCoroutine(lifespan());
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.name == "LeftBullet(Clone)")
        {
            rb2D.velocity = new Vector2(moveSpeed * Time.deltaTime, 0);
        }

        if(gameObject.name == "Right Bullet(Clone)")
        {
            rb2D.velocity = new Vector2(-moveSpeed * Time.deltaTime, 0);
        }

        if(gameObject.name == "Enemy Bullet(Clone)")
        {
            rb2D.velocity = new Vector2(moveSpeed * Time.deltaTime, 0);
        }
    }

    IEnumerator lifespan()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
