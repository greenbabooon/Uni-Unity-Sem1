
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class enemyBotScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject botObj;
    Rigidbody2D BotRb;
    Collider2D botCol;
    int dir = 1;
    public int speed = 2;
    float speedMultiplier = 1;
    public int visionDistance = 5;
    RaycastHit2D vision;
    float botOffsetX;
    RaycastHit2D dontFall;
    float dontFallOffsetX;
    GameManagerScript gameManager = null;
    RaycastHit2D sixthSense;
    SpriteRenderer EnemySprite;
    Transform ExlaimationSpriteTransform;
    SpriteRenderer ExlaimatioinSpriterenderer;
    float sizeMultiplier = 1;
    float rotationDegrees = 0;
    Vector3 startSize;
    bool f = true;
    AudioSource npcAudio;
    bool scanSfx = false;
    public GameObject drop;
    ParticleSystem particle;
    bool death = false;
    float deathTimer = 1f;
    bool spawnObj = false;
    bool isRanged = false;
    float shootingTimer = 0f;
    public GameObject bullet;
    float bulletLifeTime = 3f;
    float shootCooldown = 5f; 
    void Start()
    {
        if (bullet != null)
        {
            isRanged = true;
        }

        npcAudio = GetComponent<AudioSource>();
        EnemySprite = this.GetComponent<SpriteRenderer>();
        Transform child = transform.Find("alert");
        if (child != null)
        {
            GameObject childObj = child.gameObject;
        }
        ExlaimationSpriteTransform = child.GetComponent<Transform>();
        ExlaimatioinSpriterenderer = child.GetComponent<SpriteRenderer>();
        gameManager = FindAnyObjectByType<GameManagerScript>();
        botObj = this.gameObject;
        BotRb = botObj.GetComponent<Rigidbody2D>();
        botCol = botObj.GetComponent<Collider2D>();
        startSize = ExlaimationSpriteTransform.localScale;
        ExlaimatioinSpriterenderer.enabled = false;
        particle = GetComponent<ParticleSystem>();

    }

    void SfxDelayed()
    {
        npcAudio.Play();
        scanSfx = false;
    }
    void FixedUpdate()
    {
        if (death == true)
        {
            deathTimer -= 0.02f;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - 0.05f, gameObject.transform.localScale.y - 0.05f, gameObject.transform.localScale.z);
            if (gameObject.transform.localScale.x < 0f)
            {
                gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
            }
            particle.Play();
            if (deathTimer < 0)
            {
                DeathDelayed();

            }

        }
        if (death == false)
        {
            if (scanSfx == false)
            {
                Invoke("SfxDelayed", 5);
                scanSfx = true;
            }
            BotRb.linearVelocityX = dir * speed * speedMultiplier;
            if (dir == 1)
            {
                botOffsetX = botCol.bounds.max.x + 0.1f;
                dontFallOffsetX = botCol.bounds.max.x + 0.5f;
                EnemySprite.flipX = false;
            }
            else if (dir == -1)
            {
                botOffsetX = botCol.bounds.min.x - 0.1f;
                dontFallOffsetX = botCol.bounds.min.x - 0.25f;
                EnemySprite.flipX = true;
            }

            vision = Physics2D.Raycast(new Vector2(botOffsetX, botObj.transform.position.y), new Vector2(dir, 0), visionDistance, LayerMask.GetMask("Player", "ground"));
            Debug.DrawRay(new Vector2(botOffsetX, botObj.transform.position.y), new Vector2(dir, 0) * visionDistance, Color.red);
            shootingTimer += 0.02f;
            if (vision.collider != null)
            {

                if (vision.collider.gameObject.tag == "Player")
                {
                    ExlaimationSpriteTransform.localScale = startSize;
                    sizeMultiplier = 1;
                    speedMultiplier = 1.5f;
                    print("Player in sight");
                    ExlaimatioinSpriterenderer.enabled = true;
                      
                            if (shootingTimer >= shootCooldown)
                {
                    InstantiateBullet();
                    print("Bullet Instantiated method called");
                    shootingTimer = 0f;
                }      /*rotationDegrees += 0.25f;
                    ExlaimationSpriteTransform.rotation = new Quaternion(0, rotationDegrees, 0, 0);


                    if (f == true)
                    {   
                        sizeMultiplier += 0.001f;
                        ExlaimationSpriteTransform.localScale =new Vector3 (startSize.x*sizeMultiplier,startSize.y*sizeMultiplier,ExlaimationSpriteTransform.localScale.z);
                        if (sizeMultiplier > 1.5)
                        {
                            f = false;
                        }
                    }
                    else if (f == false)
                    {
                        sizeMultiplier -= 0.001f;
                        ExlaimationSpriteTransform.localScale =new Vector3 (startSize.x*sizeMultiplier,startSize.y*sizeMultiplier,ExlaimationSpriteTransform.localScale.z);
                         if (sizeMultiplier <1)
                        {
                            f = true;
                        }
                    }
                 */

                }
                else
                {
                    speedMultiplier = 1;
                    ExlaimatioinSpriterenderer.enabled = false;
                }

            }
            dontFall = Physics2D.Raycast(new Vector2(dontFallOffsetX, botObj.transform.position.y), Vector3.down, 1f, LayerMask.GetMask("ground"));
            Debug.DrawRay(new Vector2(dontFallOffsetX, botObj.transform.position.y), Vector2.down * 1f, Color.blue);
            if (dontFall.collider == null)
            {
                dir = dir * -1;
            }
            sixthSense = Physics2D.Raycast(new Vector2(botOffsetX, botObj.transform.position.y), new Vector2(dir * -1, 0), visionDistance / 2.5f, LayerMask.GetMask("Player", "ground"));
            if (sixthSense.collider != null)
            {
                if (sixthSense.collider.gameObject.tag == "Player")
                {
                    dir = dir * -1;

                }
            }

            RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(botOffsetX, botObj.transform.position.y - 0.4f), new Vector2(dir, 0), 0.3f);
            if (wallCheck.collider != null)
            {
                dir = dir * -1;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        /* if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
         {
             dir = dir * -1;
         }*/
        if (collision.gameObject.name == "Player" && death == false)
        {
            gameManager.loadGame();
        }

    }
    public void DeathDelayed()
    {
        FindAnyObjectByType<FeetScript>().Ignore();
        Destroy(this.gameObject);
    }
    public void setDeath(bool state)
    {
        spawnObj = true;
        death = true;
        Instantiate(drop, gameObject.transform.position, Quaternion.identity);
    }
    void InstantiateBullet()
    {
        if (bullet != null)
        {
            // Spawn position calculation
            Vector3 spawnPos = transform.position + new Vector3(dir * 1.5f, 0, 0);

            // Instantiate and setup bullet
            GameObject bulletInstance = Instantiate(bullet, spawnPos, Quaternion.identity);
            BulletScript bulletScript = bulletInstance.GetComponent<BulletScript>();

            if (bulletScript != null)
            {
                bulletScript.SetDirection(dir);
                // Ignore collision between bullet and bot
                Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), botCol);
                bulletScript = null;
            }
        }
    else
        {
            Debug.LogWarning("Bullet prefab is not assigned in the enemyBotScript.");
        }
    }
}
