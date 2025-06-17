using Unity.VisualScripting;
using UnityEngine;

public class OptionsScript : MonoBehaviour
{
    public pauseSystemScript pauseSystem=null;
    bool isPauseSys=false;
    void Start()
    {
        if (pauseSystem == null)
        {
            isPauseSys = true;   
        }else{
            isPauseSys = false;
        }
    }
    public void OpenOptions()
    {
        if (isPauseSys == false)
        {
            FindFirstObjectByType<GameManagerScript>().canvases[0].enabled = false;
            gameObject.GetComponentInChildren<Canvas>().enabled = true;
        }else
        {
            pauseSystem.gameObject.GetComponentInChildren<Canvas>().enabled = false;
            gameObject.GetComponentInChildren<Canvas>().enabled = true;
        }
        

    }
    public void CloseOptions()
    {
        if (isPauseSys == false)
        {
           FindFirstObjectByType<GameManagerScript>().canvases[0].enabled = false;
           gameObject.GetComponentInChildren<Canvas>().enabled = false;  
        }else
        {
            pauseSystem.gameObject.GetComponentInChildren<Canvas>().enabled = false;
            gameObject.GetComponentInChildren<Canvas>().enabled = false;
        }
       

    }
}
