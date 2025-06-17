using UnityEngine;

public class FeetScript : MonoBehaviour
{
    playerController playerController;
    RaycastHit2D stepCheck;
    Vector2 stepCheckoffset;
    float stepCheckDir = 1;
    bool grounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Ignore();
    }
    public void Ignore()
    {
        playerController = GetComponentInParent<playerController>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("grab");
        foreach (var obj in objs)
        {
            Physics2D.IgnoreCollision(obj.gameObject.GetComponent<Collider2D>(), gameObject.GetComponentInParent<CapsuleCollider2D>());
        }
        ButtonScript[] objs2 = FindObjectsByType<ButtonScript>(FindObjectsSortMode.InstanceID);
        foreach (var obj in objs2)
        {
            Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), gameObject.GetComponentInParent<CapsuleCollider2D>());
            Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
        GameObject[] objs3 = GameObject.FindGameObjectsWithTag("frame");
        foreach (var obj in objs3)
        {
            Physics2D.IgnoreCollision(obj.gameObject.GetComponent<Collider2D>(), gameObject.GetComponentInParent<CapsuleCollider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {/*//Step logic is not used anymore (changed box collider to capsule collider which was necessary when we started using tiles for the ground rather than placing the ground manually)

        stepCheckoffset.y = playerController.gameObject.GetComponentInParent<BoxCollider2D>().bounds.min.y;
        if (playerController.getLeftRight() == true)
        {
            stepCheckoffset.x = gameObject.GetComponent<BoxCollider2D>().bounds.min.x;
            stepCheckDir = -1;


        }
        else if (playerController.getLeftRight() == false)
        {
            stepCheckoffset.x = gameObject.GetComponent<BoxCollider2D>().bounds.max.x;
            stepCheckDir = 1;
            
        }
        stepCheck = Physics2D.Raycast(stepCheckoffset, new Vector2(stepCheckDir, 0),0.1f, LayerMask.GetMask("ground"));
        Debug.DrawRay(stepCheckoffset, new Vector2(stepCheckDir, 0), Color.red);
        if (stepCheck.collider != null&&grounded==true)
        {   
            print ("Step Check: " + stepCheck.collider.gameObject.name);
            RaycastHit2D upCheck = Physics2D.Raycast(new Vector2(stepCheck.transform.position.x,stepCheck.collider.bounds.max.y+0.05f), Vector2.up);
            Debug.DrawRay(new Vector2(stepCheck.point.x,stepCheck.collider.bounds.max.y+0.05f), Vector2.up, Color.green);
            if (upCheck.collider == null)
            {print ("Up Check: " + stepCheck.collider.gameObject.name);
                if (stepCheckoffset.y + 1.1 > stepCheck.collider.bounds.max.y&&grounded==true)
                {
                    float stepY = stepCheck.collider.bounds.max.y - stepCheckoffset.y;
                    playerController.gameObject.GetComponent<Transform>().position = new Vector3(playerController.GetComponent<Transform>().position.x + (0.05f * stepCheckDir), playerController.GetComponent<Transform>().position.y + stepY + 0.01f, playerController.GetComponent<Transform>().position.z);
                }
            }
           
        }*/
        
    }
    void OnTriggerEnter2D(Collider2D Collision)
    {
        GetComponent<ParticleSystem>().Play();   
    }
    void OnTriggerStay2D(Collider2D collision)
    {     if (collision.gameObject.layer == 6)
        {
            playerController.Grounded();
            grounded = true;
        }

        else
        {
            playerController.NotGrounded();
            grounded = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        playerController.NotGrounded();
        grounded = false;    
    }
    
    
}
 