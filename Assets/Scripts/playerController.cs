using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public float moveSpeed;
    public Animator animator;

    public float angle;

    public float health = 100;
    public Weapon weaponScript;

    public Transform deadPlayer;

    public AudioSource src;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    public GameObject currentTask;

    Vector2 movement;
    public Vector2 mousePosition;
    public Vector2 direction;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        currentTask.transform.position = new Vector2(transform.position.x + 4, transform.position.y + 2);

        if (Input.GetKeyDown(KeyCode.M)) {
            if (currentTask.activeSelf)
            {
                currentTask.SetActive(false);
            }
            else {
                currentTask.SetActive(true);
            }
        }

        if (health > 0)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x != 0 || movement.y != 0)
            {
                animator.SetBool("toWalk", true);
            }
            else
            {
                animator.SetBool("toWalk", false);
            }
        }

        if (health <= 0) {
            gameObject.SetActive(false);
            Instantiate(deadPlayer, transform.position, Quaternion.Euler(0, 0, rb.rotation));
            //transform.position = new Vector2(-50, transform.position.y);
        }

        health = Mathf.Clamp(health, 0, 100);
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            src.Play();
            health += 40;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.layer == 9)
        {
            src.Play();
            weaponScript.currentWeapon = 1;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.layer == 11)
        {
            src.Play();
            weaponScript.currentWeapon = 2;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the boss attack particles
        if (collision.gameObject.layer == 4)
        {
            // Reduce the player's health
            health -= 10;

            // Check if the player is dead
            if (health <= 0)
            {
                // Handle player death
            }
        }
    }

    void FixedUpdate()
    {
        if (health > 0)
        {
            transform.position = new Vector2(transform.position.x + movement.x * moveSpeed * Time.fixedDeltaTime, transform.position.y + movement.y * moveSpeed * Time.fixedDeltaTime);
            direction = mousePosition - (Vector2)transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

}
