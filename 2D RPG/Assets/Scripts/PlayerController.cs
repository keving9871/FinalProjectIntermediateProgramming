using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Variables:
    public float moveSpeed, health;
    public Chest chest;
    public SpriteRenderer sr;
    public bool swordPowers, nextToInteractable, nearStore, gunPowers, hasShot;
    public GameObject meleeArea, leftMeleeArea, apple, canvas, audioController, hair, leftBullet, rightBullet;
    public int coins;
    private Scene scene;
    public TextMeshProUGUI interactText, healthText, coinText, storeText, getKeyText, gunText, swordText, dialogueText;
    private bool meleeIsRunning, leftMeleeisRunning, isFlipped, hasKey;
    public AudioSource audioSource;
    public AudioClip swoosh, ouch, eat, bang;
    private GameObject spawnPoint;
    public Transform firePoint, leftFirePoint;
    public Image keyImage;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(canvas);
        DontDestroyOnLoad(audioController);
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
        transform.position = spawnPoint.transform.position;
        nextToInteractable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        meleeArea.SetActive(false);
        leftMeleeArea.SetActive(false);
        health = 5;
        scene = SceneManager.GetActiveScene();
        interactText.gameObject.SetActive(false);
        getKeyText.gameObject.SetActive(false);
        gunText.gameObject.SetActive(false);
        leftMeleeisRunning = false;
        meleeIsRunning = false;
        healthText.gameObject.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        storeText.gameObject.SetActive(false);
        nextToInteractable = false;
        nearStore = false;
        //spawnPoint = null;
        gunPowers = false;
        swordPowers = false;
        isFlipped = false;
        hasShot = false;
        keyImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Controls:
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
        }

        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
        }

        //Sprinting:
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 5;
        }
        else
        {
            moveSpeed = 3;
        }

        //Flip the sprite depending on move direction
        if(Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            sr.flipX = true;
            hair.GetComponent<SpriteRenderer>().flipX = true;
            isFlipped = true;
        }
        
        //Return sprite to face the right if player is moving right
        if(Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            sr.flipX = false;
            hair.GetComponent<SpriteRenderer>().flipX = false;
            isFlipped = false;
        }

        //Sword Controls:
        if (Input.GetButtonDown("Fire1") && swordPowers == true)
        {
            Swing();
            swordText.gameObject.SetActive(false);
        }

        //Gun Controls:
        if(Input.GetButtonDown("Fire2") && gunPowers == true && hasShot == false)
        {
            Shoot();
            hasShot = true;
            gunText.gameObject.SetActive(false);
        }

        //Health System:
        if(health <= 0)
        {
            GameOver();
        }

        //Display Health Text
        healthText.text = health.ToString() + " x ";

        //Display Coins:
        coinText.text = coins.ToString() + " x ";

        //Dont display store text if the player can't afford anything:
        if(coins <= 0)
        {
            storeText.gameObject.SetActive(false);
        }

        //Make sure Interact Text is not always on:
        if(nextToInteractable == false)
        {
            interactText.gameObject.SetActive(false);
        }

        if(nextToInteractable == true)
        {
            interactText.gameObject.SetActive(true);
        }

        if(nearStore == true)
        {
            storeText.gameObject.SetActive(true);
        }
        else
        {
            storeText.gameObject.SetActive(false);
        }

        //Make sure the spawn point is set:
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn");

        //Make sure interact text is disabled on last level
       // if(currentSceneNumber == 5)
       // {
      //      interactText.gameObject.SetActive(false);
      //  }
    }

    public void Swing()
    {
        //Check if the player is facing left
        if (sr.flipX == false && leftMeleeisRunning == false)
        {
            StartCoroutine(melee());
        }

        //Check if the player is facing right
        if (sr.flipX == true && meleeIsRunning == false)
        {
            StartCoroutine(leftMelee());
        }

        //Play sound effect:
        audioSource.PlayOneShot(swoosh);
    }

    public void Shoot()
    {
        //Begin Coroutine
        StartCoroutine(Fire());
    }

    IEnumerator leftMelee()
    {
        //Enable the left attack:
        leftMeleeArea.SetActive(true);

        //Set running bool to on:
        leftMeleeisRunning = true;

        //Make the sword only stay out for about half a second
        yield return new WaitForSeconds(.40f);

        //Disable the sword
        leftMeleeArea.SetActive(false);

        //Disable running bool:
        leftMeleeisRunning = false;
    }

    IEnumerator melee()
    {
        //Enable the sword
        meleeArea.SetActive(true);

        //Set running bool to on:
        meleeIsRunning = true;

        //Make the sword only stay out for about half a second
        yield return new WaitForSeconds(.40f);

        //Disable the sword
        meleeArea.SetActive(false);

        //Disable running bool:
        meleeIsRunning = false;
    }

    IEnumerator hurtPlayer()
    {
        //Play hurt Sound:
        audioSource.PlayOneShot(ouch);

        //Indicate Damage by flashing red
        sr.color = Color.red;

        //Keep player red for .25 seconds:
        yield return new WaitForSeconds(.25f);

        //Lose 1 health
        health -= 1f;

        //Return player back to normal color:
        sr.color = new Color(255, 255, 255, 255);
    }

    IEnumerator Fire()
    {
        //Delay how fast player can fire:
        yield return new WaitForSeconds(.5f);

        //Play sound effect:
        audioSource.PlayOneShot(bang);

        //Allow shooting again:
        hasShot = false;

        //Spawn the bullet:
        if (isFlipped == false)
        {
            Instantiate(rightBullet, firePoint.position, Quaternion.Euler(0, 0, 270));
            rightBullet.tag = "friendly";
        }

        if(isFlipped == true)
        {
            Instantiate(leftBullet, leftFirePoint.position, Quaternion.Euler(0, 0, 90));
            leftBullet.tag = "friendly";
        }
        
    }

    public void GameOver()
    {
        //Back to Menu:
        SceneManager.LoadScene(0);
        Destroy(this.gameObject);
        Destroy(canvas);
        Destroy(audioController);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Damage the player when an enemy touches them:
        if (collision.CompareTag("Enemy") && leftMeleeisRunning == false && meleeIsRunning == false)
        {
            StartCoroutine(hurtPlayer());
        }

        //Let bullets damage player
        if (collision.CompareTag("Bullet"))
        {
            StartCoroutine(hurtPlayer());
        }

        //Check if player is close enough and pick up the sword
        if (collision.CompareTag("Sword"))
        {
            //Delete the sword in the world
            Destroy(collision.gameObject);

            //Set the sword powers boolean to true:
            swordPowers = true;

            //Enable Sword Text:
            swordText.gameObject.SetActive(true);
        }

        //Pick up gun:
        if (collision.CompareTag("Gun"))
        {
            //Delete the gun in the world
            Destroy(collision.gameObject);

            //Set the gun powers boolean to true:
            gunPowers = true;

            //Enable Gun Text:
            gunText.gameObject.SetActive(true);
        }

        //Display Interact Text:
        if (collision.CompareTag("Chest"))
        {
            nextToInteractable = true;
            //StartCoroutine(InteractTextTimer());
        }

        //Play Eat Sound
        if (collision.CompareTag("Apple"))
        {
            health += 2;
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(eat);
        }

        //Store
        if (collision.CompareTag("Store") && coins >= 1)
        {
            nearStore = true;
        }

        //Keep the player from getting out of the map:
        if (collision.CompareTag("OutofBounds") && spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }

        //Key Pickup:
        if (collision.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            hasKey = true;
            keyImage.enabled = true;
        }

        //Trigger NPC Dialogue
        if (collision.CompareTag("NPC"))
        {
            dialogueText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Check if player is close enough to interact with chest
        if(collision.CompareTag("Chest"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                chest.Open();
            }
        }

        if (collision.CompareTag("Door") && collision.gameObject.name != "LockedDoor")
        {
            nextToInteractable = true;
        }

        if(collision.CompareTag("Door") && collision.gameObject.name == "LockedDoor" && swordPowers == true && gunPowers == true && hasKey == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Destroy(canvas);
                Destroy(this.gameObject);
                SceneManager.LoadScene(5);
            }
        }

        if(collision.gameObject.name == "LockedDoor" && hasKey == false)
        {
            getKeyText.gameObject.SetActive(true);
        }

        //Keep the player from getting out of the map:
        if (collision.CompareTag("OutofBounds"))
        {
            transform.position = spawnPoint.transform.position;
        }

        //Let player open the door to progress:
        if (collision.CompareTag("Door") && Input.GetKey(KeyCode.E) && swordPowers == true)
        {
            LoadLevel();
        }

        if (collision.CompareTag("Store") && coins >= 1)
        {
            Vector3 offset = new Vector3(0, -3, 0);
            GameObject store = GameObject.FindGameObjectWithTag("Store");

            if (Input.GetKey(KeyCode.E))
            {
                coins -= 1;
                Instantiate(apple, store.transform.position + offset, transform.rotation);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Remove Interact Text
        if (collision.CompareTag("Chest"))
        {
            nextToInteractable = false;
        }

        //Remove Interact Text
        if (collision.CompareTag("Door"))
        {
            nextToInteractable = false;
        }

        //Remove Store text:
        if (collision.CompareTag("Store"))
        {
            nearStore = false;
        }

        if (collision.CompareTag("NPC"))
        {
            dialogueText.gameObject.SetActive(false);
        }
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
        transform.position = spawnPoint.transform.position;
        chest = null;
    }
}
