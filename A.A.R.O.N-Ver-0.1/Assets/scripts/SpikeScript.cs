using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikeScript : MonoBehaviour
{      
    GameManagerScript gameManager=null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
    {
        gameManager=FindAnyObjectByType<GameManagerScript>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    { if (collision.gameObject.tag=="Player"){
        
      gameManager.loadGame();  
       
    }
    }
}
