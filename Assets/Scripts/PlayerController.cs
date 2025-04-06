using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 6f;
    private float horizontalDirection = 0f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    private Animator playerAnimation;

    private Vector3 respawnPoint;
    public GameObject fallDetector;

    public Text scoreText;
    public HealthBar healthBar;

    public float climbSpeed = 3f;
    private float verticalDirection;
    private bool isClimbing = false;

    public GameObject GameOverScreen; // Assign this in the Inspector for a Game Over screen
    public float spikeDamageRate = 0.002f; // Damage per frame while standing on a spike

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        respawnPoint = transform.position;
        scoreText.text = "Score: " + Scoring.totalScore;

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is alive
        if (healthBar.GetHealth() <= 0)
        {
            Die();
            return; // Exit Update to prevent further updates after death
        }

        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        horizontalDirection = Input.GetAxis("Horizontal");

        // Horizontal Movement
        if (horizontalDirection > 0f)
        {
            player.velocity = new Vector2(horizontalDirection * speed, player.velocity.y);
            transform.localScale = new Vector2(0.2354078f, 0.2354078f);
        }
        else if (horizontalDirection < 0f)
        {
            player.velocity = new Vector2(horizontalDirection * speed, player.velocity.y);
            transform.localScale = new Vector2(-0.2354078f, 0.2354078f);
        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
        }

        // Update animations
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);

        // Update fall detector
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);

        // Climbing Logic
        verticalDirection = Input.GetAxis("Vertical");

        if (isClimbing)
        {
            player.velocity = new Vector2(player.velocity.x, verticalDirection * climbSpeed);
            player.gravityScale = 0; // Disable gravity while climbing
            playerAnimation.SetBool("IsClimbing", true);
        }
        else
        {
            player.gravityScale = 1; // Re-enable gravity when not climbing
            playerAnimation.SetBool("IsClimbing", false);
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");

    // Play death animation
    playerAnimation.SetTrigger("Die");

    // Show the Game Over screen
    if (GameOverScreen != null)
    {
        GameOverScreen.SetActive(true);
        Debug.Log("Game Over screen activated.");
          
        }
        else
    {
        Debug.LogWarning("GameOverScreen is not assigned!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Disable player controls
    this.enabled = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        else if (collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            AudioManager.instance.PlaySound(AudioManager.instance.portalSound); // Play portal sound
            respawnPoint = transform.position;
        }
        else if (collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            AudioManager.instance.PlaySound(AudioManager.instance.portalSound); // Play portal sound
            respawnPoint = transform.position;
        }
        else if (collision.tag == "Crystal")
        {
            Debug.Log("Crystal collected!");
            Scoring.totalScore += 1;
            scoreText.text = "Score: " + Scoring.totalScore;

            // Play crystal sound
            AudioManager.instance.PlaySound(AudioManager.instance.crystalSound);

            collision.gameObject.SetActive(false); // Disable the crystal
        }

        else if (collision.tag == "Ladder")
        {
            isClimbing = true;
            Debug.Log("Started climbing ladder.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Spike")
        {
            // Play spike sound
            AudioManager.instance.PlaySound(AudioManager.instance.spikeSound);

            healthBar.Damage(spikeDamageRate); // Apply damage continuously while standing on spike
            Debug.Log("Player is taking damage from spike!");
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            player.gravityScale = 1; // Re-enable gravity when leaving the ladder
            Debug.Log("Exited ladder.");
        }
    }

}
