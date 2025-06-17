using UnityEngine;

public class BridgeScript : MonoBehaviour
{   public ButtonScript buttonScript;
    GameObject bridge;  
    SpriteRenderer bridgeSprite;
    Collider2D bridgeCol;        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     bridge=this.gameObject;
     bridgeSprite=bridge.GetComponent<SpriteRenderer>();
     bridgeCol=bridge.GetComponent<Collider2D>();   
    }

    // Update is called once per frame
    void Update()
    {
    if (buttonScript.GetPressed()==true)
    {
    bridgeSprite.enabled=true;
    bridgeCol.enabled=true;    
    }
    else
    {
        bridgeSprite.enabled=false; 
        bridgeCol.enabled=false;
        
    }}
}
