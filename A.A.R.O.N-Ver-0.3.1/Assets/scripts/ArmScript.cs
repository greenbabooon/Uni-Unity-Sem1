

using Unity.VisualScripting;
using UnityEngine;

public class ArmScript : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  cursorController cursorController;
  public Rigidbody2D armRb;
  public GameObject armGameObj;
  public Collider2D armCol;
  public CapsuleCollider2D playerCol;
  public playerController playerController;
  int shooting = 0;
  //0=not shooting 1=shooting 2=returning
  public float shootSpeed = 5f;
  public GameObject cursorObj;
  Vector2 mousePos;
  Vector2 curPos;
  float t = 0f;
  public Timer Timer;
  public GameObject playerObj;
  bool canFollow = true;
  bool isGrabbed = false;
  float curRelativePos = 0f;
  bool isCarrying = false;
  float prevMass;
  bool tooFar = false;
  GameObject lastCollisionObj;
  GameObject lastCollisionObj2;
  public Sprite[] armSprites;//0=HandGrabbing, 1=HandOpen , 2= HandCarrying
  SpriteRenderer armSprite;
  GameObject armTrail;
  Vector3 lastCollisionObj2OriginalScale;
  bool isThrowing = false;
  bool carryDelay = false;
  int angleMultiplier = 0;
  void Start()
  {
    armSprite = GetComponent<SpriteRenderer>();
    armTrail = GetComponentInChildren<TrailRenderer>().gameObject;
    Physics2D.IgnoreCollision(armCol, playerCol);
  }

  void FixedUpdate()
  {
    if (playerController.GetArmState() != "Standard")
    {
      armTrail.GetComponent<TrailRenderer>().enabled = true;
    }
    else
    {
      armTrail.GetComponent<TrailRenderer>().enabled = false;
    }
    if (shooting == 0)
    {
     
      if (playerController.GetArmState() != "Standard")
      {
        armRb.rotation = GetAngleBetweenObjects(playerObj.transform.position, cursorObj.transform.position) + angleMultiplier;
      }
      else
      {
        armRb.rotation = 0f; // Reset rotation to 0 when in Standard state
      }
      
      if (armRb.rotation >= 90f && armRb.rotation <= 270f)
      {
        armGameObj.transform.position = playerObj.transform.position + new Vector3(0f, 1f, -0.1f);
      }
      else
      {
        armGameObj.transform.position = playerObj.transform.position + new Vector3(0f, -1f, -0.1f);
      }

      if (playerController.getLeftRight() == true)
      {
        armGameObj.transform.position = new Vector2(playerObj.transform.position.x - 0.4f, playerObj.transform.position.y + 0.1f);
        //armTrail.transform.localPosition = new Vector3(-0.25f, 0);
        if (playerController.GetArmState() == "Carry" || playerController.GetArmState() == "Charged")
        {
          angleMultiplier = 180;

        }
        else
        {
          armSprite.flipX = true;
        }

      }
      else if (playerController.getLeftRight() == false)
      {
        armGameObj.transform.position = new Vector2(playerObj.transform.position.x + 0.4f, playerObj.transform.position.y + 0.1f);
        //armTrail.transform.localPosition = new Vector3(0.25f, 0);
        if (playerController.GetArmState() == "Carry" || playerController.GetArmState() == "Charged")
        {
          angleMultiplier = 0;

        }
        else
        {
          armSprite.flipX = false;
        }

      }

    }

    if (shooting == 1)
    {
      armSprite.sprite = armSprites[2];
      t += 0.04f;
      armGameObj.transform.position = Vector2.Lerp(curPos, mousePos, t);
      if (t >= 1f)
      {
        shooting = 2;

      }
    }
    if (shooting == 2)
    {
      armGameObj.transform.position = Vector2.Lerp(curPos + new Vector2(curRelativePos, 0), mousePos, t);
      armSprite.sprite = armSprites[0];
      t -= 0.04f;
      if (t <= -0f)
      {
        shooting = 0;
        canFollow = true;
        t = 0;

        if (lastCollisionObj != null && armRb != null)
        {
          lastCollisionObj.transform.SetParent(null);
          Rigidbody2D rb = lastCollisionObj.GetComponent<Rigidbody2D>();
          if (rb != null)
          {
            rb.bodyType = RigidbodyType2D.Dynamic;
          }
          lastCollisionObj = null;
        }

      }
    }
    if (playerController.GetArmState() == "Carry" && canFollow == true && tooFar == false && isThrowing == false && carryDelay == false)
    {

      armGameObj.transform.position = Vector2.Lerp(armRb.position, cursorObj.transform.position, Time.fixedDeltaTime * 10f);
      armSprite.sprite = armSprites[1];

      if (Vector2.Distance(armRb.position, playerObj.transform.position + new Vector3(0f, 1f, 0f)) > playerController.GetMaxAimRadius() + 2f)
      {
        Invoke("TooFar", 0.5f);
        Vector2.Lerp(armRb.position, playerObj.transform.position, Time.fixedDeltaTime);
        playerController.SetArmState("Standard");
        tooFar = true;
        armSprite.sprite = armSprites[0];
      }

    }
    else if (playerController.GetArmState() == "Carry" && isThrowing == true)
    {
      armRb.position = playerObj.transform.position + new Vector3(0f, 1f, 0f);
    }

  }
  void OnCollisionEnter2D(Collision2D collision)
  {

    if (shooting == 1)
    {
      shooting = 2;
    }
    if (collision.gameObject.tag == "grab" && shooting > 0)
    {

      lastCollisionObj = collision.gameObject;
      lastCollisionObj.gameObject.transform.SetParent(armGameObj.transform);
      lastCollisionObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
      armSprite.sprite = armSprites[0];
      print("grabbed");
    }

  }
  void OnCollisionStay2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "grab" && Input.GetKey(KeyCode.Mouse0) && playerController.GetArmState() == "Carry" && isCarrying == false&&lastCollisionObj2==null)
    {
      tooFar = false;
      lastCollisionObj2 = collision.gameObject;
      lastCollisionObj2OriginalScale = lastCollisionObj2.transform.localScale;
      lastCollisionObj2.gameObject.transform.SetParent(armGameObj.transform);
      lastCollisionObj2.transform.localScale = new Vector3(lastCollisionObj2OriginalScale.x * 0.8f, lastCollisionObj2OriginalScale.y * 0.8f, 1);
      lastCollisionObj2.GetComponent<Rigidbody2D>().simulated = false;
      armSprite.sprite = armSprites[0];
      armSprite.flipY = true;

    }

  }


  void Update()
  {
    if (playerController.GetArmState() == "Charged"&&shooting==0)
    {
      armSprite.sprite = armSprites[2];
    }
    if (playerController.GetArmState() == "Standard")
    {
      angleMultiplier = 0; // Reset angleMultiplier when not in Carry state
      armSprite.flipY = false;  // Reset the flipY when not in Carry state
    }
    if (playerController.GetArmState() != "Standard")
    {
      GetComponent<Animator>().enabled = false; // Disable the animator when any key is pressed
      
    }
    else
    {
      GetComponent<Animator>().enabled = true; // Enable the animator when no key is pressed
    }
    if (lastCollisionObj2 != null)
    {
      isCarrying = true;
      armSprite.sprite = armSprites[0];

    }

    if (Input.GetKeyDown(KeyCode.Mouse0) && shooting == 0 && playerController.GetArmState() == "Charged")
    {

      mousePos = cursorObj.transform.position;
      curPos = armGameObj.transform.position;
      canFollow = false;
      shooting = 1;
      GetComponentInChildren<ParticleSystem>().Play();
      GetComponent<AudioSource>().Play();
      if (mousePos.x > curPos.x)
      {
        // setting a negative offset for the armGameObj end position 
        curRelativePos = -1f;
      }
      else if (mousePos.x < curPos.x)
      {
        // setting a positive offset for the armGameObj end position 
        curRelativePos = 1f;

      }
    }
    if (Input.GetKeyUp(KeyCode.Mouse0) || playerController.GetArmState() != "Carry" || tooFar == true) // Check if the mouse button is released or the arm state is not "Carry" or too far
    {
      armSprite.flipY = false;
      if (lastCollisionObj2 != null) // Check if lastCollisionObj2 is not null
      {
        armSprite.sprite = armSprites[0];
        lastCollisionObj2.transform.SetParent(null);
        lastCollisionObj2.GetComponent<Rigidbody2D>().simulated = true;
        lastCollisionObj2.transform.localScale = lastCollisionObj2OriginalScale;
        lastCollisionObj2 = null; // Reset the reference
        isCarrying = false; // Reset the carrying state

      }

    }
    //new arm mechanic throwing items
    if (Input.GetKey(KeyCode.Space) && playerController.GetArmState() == "Carry" && lastCollisionObj2 != null)
    {
      isThrowing = true;
      playerController.SetPlayerScriptActive(false);
    }
    else if (Input.GetKeyUp(KeyCode.Space) && lastCollisionObj2 != null)
    {
      lastCollisionObj2.transform.SetParent(null);
      lastCollisionObj2.GetComponent<Rigidbody2D>().simulated = true;
      lastCollisionObj2.transform.localScale = lastCollisionObj2OriginalScale;
      isCarrying = false; // Reset the carrying state 
      isThrowing = false;
      lastCollisionObj2.GetComponent<Rigidbody2D>().AddForce(new Vector2((cursorObj.transform.position.x - armRb.position.x) * 5, (cursorObj.transform.position.y - armRb.position.y) * 5), ForceMode2D.Impulse);
      lastCollisionObj2 = null; // Reset the reference
      playerController.SetPlayerScriptActive(true);
      carryDelay = true;
      Invoke("delayedCarry", 0.2f);
    }
    else if (isThrowing && (lastCollisionObj2 == null || playerController.GetArmState() != "Carry"))
    {
      isThrowing = false;
      playerController.SetPlayerScriptActive(true);
    }
  }
  void delayedCarry()
  {
    carryDelay = false;
  }
  // Get the angle between two objects in degrees 
  float GetAngleBetweenObjects(Vector2 obj1, Vector2 obj2)
  {
    Vector2 direction = obj2 - obj1;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    return angle;
  }

  public bool GetIsGrabbed()
  {
    return isGrabbed;
  }
  public GameObject GetLastCollisionObj2()
  {
    return lastCollisionObj2;
  }
  void TooFar()
  {
    tooFar = false;
  }
  public bool GetIsThrowing()
  {
    return isThrowing;
  }
  public bool GetIsCarryDelayed()
  {
    return carryDelay;
  }
   
}
