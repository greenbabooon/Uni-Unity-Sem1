using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TerminalScript : MonoBehaviour
{
    GameManagerScript gameManager = null;
    playerController playerController;
    bool playerInRange = false;
    int terminalOpen = 0;// 0=closed 1=open first menu 2=open secondary menu 3=open save menu 4=open interaction menu 5= save Complete
    string terminalTextString1 = "Terminal Opened successfully\n" +
       ">>C://Terminal_System_files<<\n";
    //welcome message for the terminal


    TextMeshProUGUI terminalText;
    string defaultText = "Press E to close the terminal\n";
    //default text for the terminal telling user to press e to close the terminal
    public List<string> terminalTextList = new List<string>() { "If you are sure you want to SAVE?\n press:\"0\" to save\n press \"1\" to go back" };
    // list of strings to be used in the terminal every string must have a unique name
    public List<string> terminalTextListnames = new List<string>() { "save" };// input names for the strings in the terminalTextList sting names must correspond to the string in the terminalTextList
    public bool otherInteraction = false;
    //checks if you want the terminal to talk with other interactable objects or not
    public GameObject InteractionObject = null;
    // the object that will be used to interact with the terminal

    public string interactionTextName = "Terminal Interaction";
    public string interactionName = "Interaction Object";
    public string interactionText;
    // the name of the interaction object that will be used in the terminal
    bool interacted = false;
    public bool isSaveTerminal = false;
    // decides if save functionality is used for this save terminal or not
    public int saveTerminalIndex;
    // keeps track of the order of the save terminal relative to other save terminals
    string delayedText = "";
    int timeElapsed = 0;//counts time every fixed update (if the method is called it resets)
    List<char> CharList=new List<char>(){' '};
    bool charListIsInitilized;

    void Start()
    {
        playerController = FindAnyObjectByType<playerController>();
        
        gameManager = FindAnyObjectByType<GameManagerScript>();
        interactionText = "This toggles the terminal interaction with the " + interactionName + " press \"2\" to interact with the " + InteractionObject.name + " or \"0\" to go back\n";
        if (otherInteraction == true && InteractionObject != null)
        {
            terminalTextListnames.Add(interactionTextName);
            terminalTextList.Add(interactionText);
        }
        Dictionary<String, string> terminalTextDictionary = new Dictionary<String, string>();
        // create a dictionary to store the terminal text strings and their names
        for (int i = 0; i < terminalTextList.Count; i++)
        {
            terminalTextDictionary.Add(terminalTextListnames[i], terminalTextList[i]);
            // add the strings to the dictionary with the names as keys
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange == true && Input.GetKeyDown(KeyCode.E) && terminalOpen == 0)
        {
            // Call the function to open the terminal
            OpenTerminal();
            GetComponent<AudioSource>().Play();
            terminalOpen = 1;
        }
        else if (terminalOpen == 1 && Input.GetKeyDown(KeyCode.E))
        {
            // Call the function to close the terminal
            CloseTerminal();
            terminalOpen = 0;
        }
        if (terminalOpen == 1)
        {
            for (int k = 0; k < terminalTextList.Count; k++)
            {
                KeyCode key = (KeyCode)(k + 48);
                if (Input.GetKeyDown(key) && isSaveTerminal == true)
                {
                    if (k == 0)
                    {
                        OpenSaveMenu();
                        terminalOpen = 3;
                    }
                    else if (k == terminalTextList.Count - 1 && InteractionObject != null && otherInteraction == true)
                    {
                        OpenInteractionMenu();
                        terminalOpen = 4;
                    }
                    else
                    {
                        OpenSecondaryMenu(k);
                        terminalOpen = 2;
                    }
                }else if (Input.GetKeyDown(key) && isSaveTerminal == false)
                {
                    
                }
            }
        }
        else if (terminalOpen == 2)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                OpenTerminal();
                terminalOpen = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CloseTerminal();
                terminalOpen = 0;
            }
        }
        else if (terminalOpen == 3)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Saving();
                terminalOpen = 5;
                gameManager.saveGame(saveTerminalIndex);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OpenTerminal();
                terminalOpen = 1;
            }
        }
        else if (terminalOpen == 4)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                OpenTerminal();
                terminalOpen = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CloseTerminal();
                terminalOpen = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // Call the function to interact with the object
                if (interacted == false)
                {
                    interacted = true;
                }
                else if (interacted == true)
                {
                    interacted = false;
                }
                terminalOpen = 1;
                OpenTerminal();
            }
        }
        else if (terminalOpen == 5)
        {
            Invoke("Saved", 3f);
        }


    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
}
    void OnTriggerExit2D(Collider2D other2)
        {
          if (other2.CompareTag("Player"))
        {
            playerInRange = false;
        } 
    }

    
    void OpenTerminal()
    {
        // Disable player movement
        playerController.SetPlayerScriptActive(false);
        this.GetComponentInChildren<Canvas>().enabled = true;
        TextMeshProUGUI terminalText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
        terminalText.enabled = true;
        TextOverTime(terminalTextString1 + GetTerminalTextNames() + defaultText);  

    }
    void CloseTerminal()
    {
        // Enable player movement
        playerController.SetPlayerScriptActive(true);
        GetComponent<AudioSource>().Play();
        this.GetComponentInChildren<Canvas>().enabled = false;

    }
    string GetTerminalTextNames()
    {
        string concat = "\n\n";
        for (int i = 0; i < terminalTextListnames.Count; i++)
        {
            concat += "press \"" + Convert.ToChar(i + 48) + "\" for:" + terminalTextListnames[i] + "\n";
        }// this method gets the names of the strings in the terminalTextList and returns them as a string
        // the names number in the list are converted to a char and added to the string with the corresponding ascii value for the input


        return concat;

    }

    void OpenSaveMenu()
    {
        TextMeshProUGUI terminalText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
        TextOverTime(terminalTextList[0]);
    }
    void OpenSecondaryMenu(int menuIndex)
    {
        TextMeshProUGUI terminalText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
        TextOverTime(terminalTextList[menuIndex] + "\n" + "press \"0\" to go back\n" +
        "press \"1\" close the terminal");
    }


    void OpenInteractionMenu()
    {
        TextMeshProUGUI terminalText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
        TextOverTime(interactionText + "\n" + "press \"0\" to go back\n" +
        "press \"1\" close the terminal." + " press \"2\" to interact with the " + interactionName + "\n") ;
    }

    public bool GetInteracted()
    {
        return interacted;
    }

    public Vector3 GetTerminalPosition()
    {
        return this.transform.position;
    }
    void Saving(){
        TextOverTime("saving\n......................................................................................................");
        Invoke("SaveComplete", 2f);
    }
    void SaveComplete()
    {
        TextOverTime("Game Saved! :)");   
    }
    void Saved()
    {
        terminalOpen = 0;
        CloseTerminal();
    }

    void TextOverTime(string text)
    {
        TextMeshProUGUI terminalText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
        terminalText.text = "";
        delayedText = "";
        timeElapsed = 0;
        CharList = new List<char>(text.ToCharArray());
        CharList.Insert(0,' ');//The time ticks before the element in pos 1 is read
    }

    void FixedUpdate()
    {
        timeElapsed++;
        if (timeElapsed < CharList.Count&&CharList.Capacity!=0 )
        {
            TextMeshProUGUI terminalText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
            delayedText += CharList[timeElapsed];
            terminalText.text = delayedText;
             } 
        
    }


}
