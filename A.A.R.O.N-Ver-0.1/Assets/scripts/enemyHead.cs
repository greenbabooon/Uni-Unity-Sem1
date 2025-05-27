using Unity.VisualScripting;
using UnityEngine;

public class enemyHead : MonoBehaviour
{
    GameObject parentObj;
    void Start()
    {
        parentObj = GetComponentInParent<Transform>().gameObject;
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "grab")
        {
            GetComponentInParent<enemyBotScript>().death();
            
        }   
    }
}
