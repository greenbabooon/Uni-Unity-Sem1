using Unity.Mathematics;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    float startingY;
    protected bool pressed = false;
    BoxCollider2D buttonCol;
    GameObject buttonObj;
    float ypos;
    public int buttonMode = 0; //0=normal button, 1=Battery pad,

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonObj = this.gameObject;
        startingY = buttonObj.transform.position.y;
        buttonCol = buttonObj.GetComponent<BoxCollider2D>();
        if (buttonMode == 0)
        {
            Physics2D.IgnoreCollision(buttonObj.GetComponent<Collider2D>(), FindFirstObjectByType<playerController>().gameObject.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(buttonObj.GetComponentInParent<Collider2D>(), FindFirstObjectByType<playerController>().gameObject.GetComponent<Collider2D>());
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
         if (buttonMode == 0)
        {
        ypos = Mathf.Clamp(buttonObj.transform.position.y, startingY - 0.2f, startingY);
        buttonObj.GetComponent<Rigidbody2D>().MovePosition(new Vector3(buttonObj.transform.position.x, ypos, buttonObj.transform.position.z));
    }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (buttonMode == 0)
        {
            if (collision.gameObject.tag == "frame")
            {
                pressed = true;
                GetComponent<AudioSource>().Play();
            }
        }
        if (buttonMode == 1)
        {
            if (collision.gameObject.layer == 9)
            {
                pressed = true;
                GetComponent<AudioSource>().Play();
                GetComponent<ParticleSystem>().Play();

            }
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (buttonMode == 0)
        {
            if (collision.gameObject.tag == "frame")
            {
                pressed = true;
            }
    }
        if (buttonMode == 1)
        {
            if (collision.gameObject.layer == 9)
            {
                pressed = true;
                
            }
        }   

    }
    void OnCollisionExit2D(Collision2D collision)
    { 
         if (buttonMode == 0)
        {
        if (collision.gameObject.tag == "frame")
        {
            pressed = false;
            GetComponent<AudioSource>().Play();
        } 
    }  
        if (buttonMode == 1)
        {
            if (collision.gameObject.layer == 9)
            {
                pressed = false;
                GetComponent<AudioSource>().Play();
                
            }
        }    
    }
        void OnTriggerEnter2D(Collider2D collision)
    {
        if (buttonMode == 0)
        {
            if (collision.gameObject.tag == "frame")
            {
                pressed = true;
                GetComponent<AudioSource>().Play();
            }
        }
        if (buttonMode == 1)
        {
            if (collision.gameObject.layer == 9)
            {
                pressed = true;
                GetComponent<AudioSource>().Play();
                GetComponent<ParticleSystem>().Play();
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (buttonMode == 0)
        {
            if (collision.gameObject.tag == "frame")
            {
                pressed = true;
            }
    }
        if (buttonMode == 1)
        {
            if (collision.gameObject.layer == 9)
            {
                pressed = true;
                GetComponent<ParticleSystem>().Play();
            }
        }   

    }
    void OnTriggerExit2D(Collider2D collision)
    { 
         if (buttonMode == 0)
        {
        if (collision.gameObject.tag == "frame")
        {
            pressed = false;
            GetComponent<AudioSource>().Play();
        } 
    }  
        if (buttonMode == 1)
        {
            if (collision.gameObject.layer == 9)
            {
                pressed = false;
                GetComponent<AudioSource>().Play();
                GetComponent<ParticleSystem>().Stop();
            }
        }    
    }
    public bool GetPressed()
    {
        return pressed;
    }
    public void SetPressed(bool p)
    {
        pressed = p;   
    }
    
}
