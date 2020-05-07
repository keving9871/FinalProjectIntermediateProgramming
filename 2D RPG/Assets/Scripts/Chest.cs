using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    //Variables:
    public PlayerController player;
    public SpriteRenderer sr;
    public Sprite openedSprite;
    public GameObject sword;
    public bool isOpen;
    public ParticleSystem celebrateParticles;
    public AudioClip lootSound;
    AudioSource audioSource;
    public BoxCollider2D collider;
    private Vector3 swordSpawnPos = new Vector3(0.09f, -2.3f, 0.0f);
    
    void Start()
    {
        //Make sure the open boolean starts false:
        isOpen = false;

        //Make sure the gameobject has an Audio Source component in its reference variable:
        audioSource = GetComponent<AudioSource>();

        collider = GetComponent<BoxCollider2D>();
    }

    public void Open()
    {
        //Make sure the chest is not already opened:
        if (isOpen == false)
        {
            //Set the chest open boolean to true
            isOpen = true;

            //Change the chest sprite to be opened:
            sr.sprite = openedSprite;

            //Spawn the sword for the player:
            Instantiate(sword, swordSpawnPos, transform.rotation);
            sword.tag = "Sword";

            //Create particles to celebrate:
            Instantiate(celebrateParticles, swordSpawnPos, transform.rotation);

            //Play satisfying sound
            audioSource.PlayOneShot(lootSound, 1f);

            //Disable the collider:
            collider.enabled = false;
        }
    }
}
