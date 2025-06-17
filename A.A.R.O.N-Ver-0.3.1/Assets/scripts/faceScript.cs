using UnityEngine;

public class faceScript : MonoBehaviour
{
    bool leftRight=false;
    //left =true right=false
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.D))
     {
         leftRight=false;
            this.transform.localPosition=new Vector3(0.3f,0.3f,-0.1f);       
         
     }
        if (Input.GetKeyDown(KeyCode.A))
        {
            leftRight=true;
            transform.localPosition=new Vector3(-0.3f,0.3f,-0.1f); 
        }   
        
    }
}
