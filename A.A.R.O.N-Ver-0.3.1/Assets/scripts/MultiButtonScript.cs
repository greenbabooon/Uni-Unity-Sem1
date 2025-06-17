using System;
using Unity.VisualScripting;
using UnityEngine;

public class MultiButtonScript : MonoBehaviour
{
    public ButtonScript btn1;
    public ButtonScript btn2;
    ButtonScript button=new ButtonScript();
    void Update()
    {
        if (btn1.GetPressed() == true || btn2.GetPressed() == true)
        {
            button.SetPressed(true);
        }
        else if(btn1.GetPressed()==false&&btn2.GetPressed()==false)
        {
            button.SetPressed(false);
        }
    }
    public ButtonScript GetButtonScript()
    {
        return button;
    }
}
 


