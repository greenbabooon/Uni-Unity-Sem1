
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Player entered door");
         FindFirstObjectByType<GameManagerScript>().selectedLevel = SceneManager.GetActiveScene().buildIndex + 1;
        }
    }
}
