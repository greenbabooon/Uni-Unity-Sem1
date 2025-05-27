

using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Player Controller Script for A.A.R.O.N-Ver-0.1
    //Defining global variables:
    public float moveSpeed = 5f;
    //moveSpeed multiplies the force that is applied to the PlayerRigidBody this increaces the player movement speed
    public float jumpForce = 10f;
    //jumpForce multiplies the force that is applied to the PlayerRigidBody this increaces the force of the jump
    public bool canMove = true;
    /*canMove is used to declare if the player can move or not.This is used in the case where the player shouldnt be able to
    interact with the controls eg cut scenes and pause menus */
    public GameObject PlayerObj;
    //creates a gameObject to store gameObject data
    Vector2 dirHor = Vector2.zero;
    Vector2 dirVert = Vector2.zero;
    public Rigidbody2D PlayerRb;
    bool grounded = false;
    bool canJump = true;
    //the collider of the bottom of the player character
    public float jumpTime = 1f;
    public GameObject cam;
    public string camMode = "player";
    Vector3 camDistance;
    string armState;
    public const string Standard = "Standard";
    public const string Charged = "Charged";
    public const string Carry = "Carry";
    bool canShoot = false;

    float maxAimRadius = 3f;
    const float MaxStandardRadius = 2f;
    const float MaxChargedRadius = 4.5f;
    const float MaxCarryRadius = 1.5f;
    bool playerScriptActive = true;
    bool leftOrRight = false;//left = true , right = false
    Animator playerAnim;
    AudioSource playerAudio;
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
        camDistance.z = -10f;
        armState = Standard;


    }

    void playerActive()
    {
        if (canJump == true)
        {
            if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
            {
                jump();
                playerAudio.Play();
                Invoke("jumpExit", jumpTime);
            }

        }
        if (grounded == false && playerAnim.GetBool("isJumping") == false)
        {
            playerAnim.SetBool("isFalling", true);
        }
        else
        {
            playerAnim.SetBool("isFalling", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            //moves player left on x-axis(-1 *multiplier)
            dirHor = Vector2.left;
            playerAnim.SetBool("isWalking", true);
            leftOrRight = true;
            this.GetComponent<SpriteRenderer>().flipX = true;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            //moves player right on x-axis(1 *multiplier)
            dirHor = Vector2.right;
            playerAnim.SetBool("isWalking", true);
            leftOrRight = false;
            this.GetComponent<SpriteRenderer>().flipX = false;

        }
        else
        {
            dirHor = Vector2.zero;
            playerAnim.SetBool("isWalking", false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (playerScriptActive == true)
        {//contrtols if anymovement can be made by the player at all.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            {

            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            }
            if (camMode == "player")
            {
                cam.transform.position = PlayerObj.transform.position + camDistance;

            }

            if (armState == Standard)
            {
                StandardState();
            }
            else if (armState == Charged)
            {
                ChargedState();
            }
            else if (armState == Carry)
            {
                CarryState();
            }


            //checking is the player canMove variable is set to true if so it calls the playerActive() method allowing for player movement
            if (canMove == true)
            {
                playerActive();


            }
        }
        else if (playerScriptActive == false)
        {

            dirHor = Vector2.zero;
            dirVert = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (PlayerRb.linearVelocity.y < 0f&&grounded==false)
        {
            dirHor = dirHor * 0.3f;
            
                 dirVert.y -= 0.04f;
                 Mathf.Clamp(dirVert.y, -3f, -0f);
        }
        else if(PlayerRb.linearVelocityY<0f&&grounded==true)
        {
            dirVert.y = 0f;
        }
        PlayerRb.linearVelocityX = dirHor.x * moveSpeed;
        PlayerRb.linearVelocityY = dirVert.y * jumpForce;
        if (Input.GetKey(KeyCode.LeftShift) && armState != Carry && grounded == true && dirHor.x == 0)
        {
            armState = Charged;
        }
        else if (Input.GetKey(KeyCode.Mouse1) && armState != Charged)
        {
            armState = Carry;
        }
        else
        {
            armState = Standard;
        }
    }
    public void Grounded()
    {
        grounded = true;
    }
    public void NotGrounded()
    {
        grounded = false;
    }

    public void jumpExit()
    {
        playerAnim.SetBool("isJumping", false);
        dirVert = Vector2.zero;
    }

    void jump()
    {
        playerAnim.SetBool("isJumping", true);
        dirVert = Vector2.up;
        dirHor = dirHor * 0.3f;

    }

    void StandardState()
    {
        canMove = true;
        canJump = true;
        canShoot = false;
        maxAimRadius = MaxStandardRadius;


    }
    void ChargedState()
    {
        canMove = false;
        canShoot = true;
        maxAimRadius = MaxChargedRadius;

    }
    void CarryState()
    {
        canMove = true;
        canJump = false;
        canShoot = false;
        maxAimRadius = MaxCarryRadius;

    }
    public float GetMaxAimRadius()
    {
        return maxAimRadius;
    }

    public string GetArmState()
    {
        return armState;
    }

    public void SetArmState(string state)
    {
        armState = state;
    }
    public bool GetCanShoot()
    {
        return canShoot;
    }

    public bool setCanMove(bool state)
    {
        canMove = state;
        return canMove;
    }
    public void SetPlayerScriptActive(bool state)
    {
        playerScriptActive = state;
    }

    public void SetPlayerPos(Vector3 pos)
    {
        this.transform.position = pos;
    }
    public bool getLeftRight()
    {
        return leftOrRight;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "grab")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), false);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "grab")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
        }
    }

}

