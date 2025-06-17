using UnityEngine;

public class BulletScript : MonoBehaviour
{
    GameManagerScript gameManager = null;
    float lifeTime = 5f; // Time in seconds before the bullet is destroyed
    float speed = 12.5f; // Speed of the bullet
    Rigidbody2D rb;
    int direction = 1; // Direction of the bullet, can be set to -1 for left
    void OnEnable()
    {
        gameManager = FindAnyObjectByType<GameManagerScript>();
        Destroy(gameObject, lifeTime); // Destroy the bullet after its lifetime
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityX =direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.loadGame();
        }
        if (collision.gameObject.layer!=8)
        {
         Destroy(gameObject);   
        }
        print("Bullet hit: " + collision.gameObject.name);
        
    }
    public void SetDirection(int dir)
    {
        direction = dir;
    }
}
