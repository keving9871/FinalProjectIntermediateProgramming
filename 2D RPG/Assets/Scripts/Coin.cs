using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //Variables:
    public PlayerController pc;
    public AudioClip coinSound;
    AudioSource audioSource;

    void Start()
    {
        //Make sure the gameobject has an Audio Source component in its reference variable:
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pc = FindObjectOfType<PlayerController>();
        if (collision.CompareTag("Player"))
        {
            //Add a coin to the player's coin total:
            pc.coins += 1;

            //Play satisfying sound
            audioSource.PlayOneShot(coinSound, 1f);

            //Delete this coin in the world:
            Destroy(this.gameObject);
        }
    }
}
