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
        playerController = GetComponentInParent<playerController>();
          TerminalScript[] terminals = FindObjectsByType<TerminalScript>(FindObjectsSortMode.None);
        foreach (var terminal in terminals)
        {
            Physics2D.IgnoreCollision(terminal.gameObject.GetComponent<Collider2D>(),this.gameObject.GetComponent<Collider2D>());  
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        stepCheck = Physics2D.Raycast(stepCheckoffset, new Vector2(stepCheckDir, 0),0.05f, LayerMask.GetMask("ground"));
        Debug.DrawRay(stepCheckoffset, new Vector2(stepCheckDir, 0), Color.red);
        if (stepCheck.collider != null&&stepCheck.collider.gameObject.tag!="grab")
        {
            if (stepCheckoffset.y + 0.5f > stepCheck.collider.bounds.max.y)
            {   float stepY= stepCheck.collider.bounds.max.y - stepCheckoffset.y;
                playerController.gameObject.GetComponent<Transform>().position = new Vector3(playerController.GetComponent<Transform>().position.x+(0.05f*stepCheckDir), playerController.GetComponent<Transform>().position.y+stepY+0.01f, playerController.GetComponent<Transform>().position.z);
            }
        }
        
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
 