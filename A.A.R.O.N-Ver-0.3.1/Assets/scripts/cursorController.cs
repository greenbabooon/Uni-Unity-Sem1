
using Unity.VisualScripting;
using UnityEngine;

public class cursorController : MonoBehaviour
{
    public GameObject cursor;
    public GameObject player;
    public GameObject playerArm;
    public ArmScript armScript;
    Vector3 mousePos;
    Vector3 cursorPos;
    float maxRad;
    public playerController playerController;
    float maxRadMultiplier = 1;

    Collider2D cursorCol;
    SpriteRenderer cursorSprite;
    Rigidbody2D cursorRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cursorSprite = cursor.GetComponent<SpriteRenderer>();
        cursorRb = cursor.GetComponent<Rigidbody2D>();
        cursorCol = cursor.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(cursorCol, player.GetComponent<CapsuleCollider2D>());
        Physics2D.IgnoreCollision(cursorCol, playerArm.GetComponent<Collider2D>());

    }

    // Update is called once per frame
    void Update()
    {
        if (armScript.GetIsThrowing() == true)
        {
            maxRadMultiplier = 3f;
        }else
        {
            maxRadMultiplier = 1f;
        }
        if (playerController.GetArmState() == "Charged")
        {

            cursorCol.enabled = false;
            cursorSprite.enabled = true;
            cursorSprite.color = Color.green;
        }
        else if (playerController.GetArmState() == "Carry" && armScript.GetIsThrowing() == false)
        {

            cursorCol.enabled = true;
            cursorSprite.enabled = true;
            cursorSprite.color = Color.red;
        }
        else if (playerController.GetArmState() == "Carry" && armScript.GetIsThrowing() == true)
        {
            cursorCol.enabled = false;
            cursorSprite.enabled = true;
            cursorSprite.color = Color.red;
        }

        else if (playerController.GetArmState() == "Standard")
        {
            mousePos = player.transform.position;
            cursorCol.enabled = false;
            cursorSprite.enabled = true;
            cursorSprite.enabled = false;
        }
        if (playerController.GetArmState() == "Carry" || playerController.GetArmState() == "Charged")
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.z = 0f;
            maxRad = playerController.GetMaxAimRadius();
            cursorPos.x = Mathf.Clamp(mousePos.x, player.transform.position.x - maxRad *maxRadMultiplier* 1.5f, player.transform.position.x + maxRad *maxRadMultiplier* 1.5f);
            cursorPos.y = Mathf.Clamp(mousePos.y, player.transform.position.y - maxRadMultiplier*maxRad, player.transform.position.y + maxRadMultiplier*maxRad);
        }
        else if (playerController.GetArmState() == "Standard")
        {
            cursor.transform.position = player.transform.position;
        }
        if (armScript.GetLastCollisionObj2() != null)
        {
            Physics2D.IgnoreCollision(cursorCol, armScript.GetLastCollisionObj2().GetComponent<Collider2D>());
            
        }

        if (Vector2.Distance(cursorPos, player.transform.position) > playerController.GetMaxAimRadius()*maxRadMultiplier + 5f)
        {
            cursor.transform.position = player.transform.position;
        }

    }
    void FixedUpdate()
    {
        cursorRb.MovePosition(Vector2.Lerp(cursor.transform.position, cursorPos, Time.fixedDeltaTime * 5f));
    }

    public Vector2 GetMousePos()
    {
        return mousePos;
    }

}