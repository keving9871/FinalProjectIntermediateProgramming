using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{

    public PlayerController pc;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pc = FindObjectOfType<PlayerController>();
        if (collision.CompareTag("Player"))
        {
            pc.health += 2;
            Destroy(this.gameObject);
        }
    }
}
