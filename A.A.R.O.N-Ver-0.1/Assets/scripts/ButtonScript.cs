using Unity.Mathematics;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    float startingY;
    protected bool pressed = false;
    BoxCollider2D buttonCol;
    GameObject buttonObj;
    float ypos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonObj = this.gameObject;
        startingY = buttonObj.transform.position.y;
        buttonCol = buttonObj.GetComponent<BoxCollider2D>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ypos = Mathf.Clamp(buttonObj.transform.position.y, startingY - 0.2f, startingY);
        buttonObj.GetComponent<Rigidbody2D>().MovePosition(new Vector3(buttonObj.transform.position.x, ypos, buttonObj.transform.position.z));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "frame")
        {
            pressed = true;
            GetComponent<AudioSource>().Play();
        }




    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "frame")
        {
            pressed = true;
        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {   
        if(collision.gameObject.tag=="frame"){
            pressed = false;
             GetComponent<AudioSource>().Play();} 
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
