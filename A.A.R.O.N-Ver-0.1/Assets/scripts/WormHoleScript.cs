using UnityEngine;

public class WormHoleScript : MonoBehaviour
{
public GameObject neighbourWormhole;
 public GameObject player;
 bool isPlayerInWormhole=false;

    void Start()
    {
        
       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.gameObject==player)
        {
         isPlayerInWormhole=true;   
        }     
    }
    void OnTriggerExit2D(Collider2D collision){
    if (collision.gameObject==player)
        {
         isPlayerInWormhole=false;   
        }     
    }
    void Update()
    {
      if (isPlayerInWormhole==true&&Input.GetKeyDown(KeyCode.E))
      {
        player.transform.position=neighbourWormhole.transform.position+new Vector3(1,2,0);
        isPlayerInWormhole=false;
      }
   
    }
}
