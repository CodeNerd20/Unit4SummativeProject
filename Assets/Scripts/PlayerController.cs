using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public float speed = 5.0f;
    public bool hasPowerUp;
    private float powerupStrength = 15.0f;
    private bool isOnGround = true;
    public float jumpForce;
    public GameObject powerupIndicator;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
        }

        powerupIndicator.transform.position = (Vector2)transform.position + new Vector2(0, 0.5f);
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(Vector2.right * speed * horizontalInput);

       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Power-Up"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody2D enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + "with powerup set to " + hasPowerUp);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode2D.Impulse);
        }
    }
}
