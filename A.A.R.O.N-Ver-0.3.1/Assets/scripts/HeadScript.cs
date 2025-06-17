using UnityEngine;

public class HeadScript : MonoBehaviour

{public playerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider){

        playerController.jumpExit();

    }
}
