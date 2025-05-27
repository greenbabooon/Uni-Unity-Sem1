using UnityEngine;

public class gateScript : MonoBehaviour
{
    public ButtonScript buttonScript=null;
    GameObject gate;
    SpriteRenderer gateSprite;
    Collider2D gateCol;
    public TerminalScript terminalScript=null;
   [Header("Gate mode")] 
   [Tooltip("0=Start open 1=Start closed")]
    public int gateState=0;//0=start open 1=start closed
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (buttonScript == null)
        {
            buttonScript=GetComponent<MultiButtonScript>().GetButtonScript();
     }
    gate =this.gameObject;   
    gateSprite=gate.GetComponent<SpriteRenderer>(); 
    gateCol=gate.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    if(gateState==0){   
    if (buttonScript.GetPressed()==true||terminalScript!=null&&terminalScript.GetInteracted()==true)
    {
    gateSprite.enabled=false; 
    gateCol.enabled=false;    
    }
    else if(buttonScript.GetPressed()==false||terminalScript!=null&&terminalScript.GetInteracted()==false)
    {
        gateSprite.enabled=true; 
        gateCol.enabled=true;
  }   
}

    if(gateState==1){   
    if (buttonScript!=null&&buttonScript.GetPressed()==true||terminalScript!=null&&terminalScript.GetInteracted()==true)
    {
    gateSprite.enabled=true; 
    gateCol.enabled=true;    
    }
    else if (buttonScript!=null&&buttonScript.GetPressed()==false||terminalScript!=null&&terminalScript.GetInteracted()==false)
    {
        gateSprite.enabled=false; 
        gateCol.enabled=false;
    }   
}
}}

