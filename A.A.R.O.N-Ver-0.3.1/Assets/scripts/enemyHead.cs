using Unity.VisualScripting;
using UnityEngine;

public class enemyHead : MonoBehaviour
{
    GameObject parentObj;
    bool dead = false;
    void Start()
    {
        parentObj = GetComponentInParent<Transform>().gameObject;
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "grab"&& dead==false)
        {
            GetComponentInParent<enemyBotScript>().setDeath(true);
            dead = true;
        }   
    }
}
