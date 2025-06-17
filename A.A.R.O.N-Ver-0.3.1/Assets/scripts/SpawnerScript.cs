using UnityEngine;

public class SpawnerScript : MonoBehaviour
{   
    public GameObject drop; // The object to spawn
    public ButtonScript button;
    bool isSpawned = false;
    void Update()
    {
        if (button.GetPressed() == true && isSpawned == false)
        {
            Spawn();
            isSpawned = true;
        }
        if(button.GetPressed()==false)
        {
            isSpawned = false;
        }
    }
    public void Spawn()
    {
        Instantiate(drop, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, gameObject.transform.position.z), Quaternion.identity);
        print("Spawned: " + drop.name);
        FindAnyObjectByType<FeetScript>().Ignore();
    }
}
