using UnityEngine;

public class carryScript : MonoBehaviour
{   
    public GameObject playerObj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionStay2D(Collision2D collision)
    {   
        if(collision.gameObject.GetComponent<ArmScript>().GetIsGrabbed()==true){
            Physics2D.IgnoreCollision(collision.collider,playerObj.GetComponent<Collider2D>());
        }
        
    }
}
