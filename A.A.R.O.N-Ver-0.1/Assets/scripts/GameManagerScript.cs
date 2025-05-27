using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class SaveData
{
    public int playerLevel;
    public int playerChkpnt;
    
}
public class GameManagerScript : MonoBehaviour
{
    int playerLevel = 0;
    public int playerChkpnt = 0;
    playerController player = null;
    public Canvas[] canvases;
    bool isLoaded;
    void Start()
    {
        isLoaded = false;
        player = FindAnyObjectByType<playerController>();
    }
    public void saveGame(int saveTerminalIndex)
    {

        playerChkpnt = saveTerminalIndex;
        playerLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (File.Exists(Application.persistentDataPath + "/saveFile.json"))
        {
            overWriteGame();
        }
        else
        {
            newGame();
        }

    }
    public void loadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/saveFile.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/saveFile.json");
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            playerLevel = data.playerLevel;
            playerChkpnt = data.playerChkpnt;
            if (playerLevel != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
            {
                //load the game scene
                SceneManager.LoadScene(playerLevel);
                TerminalScript[] terminals = FindObjectsByType<TerminalScript>(FindObjectsSortMode.None);
                foreach (TerminalScript terminal in terminals)
                {
                    if (terminal.saveTerminalIndex == playerChkpnt)
                    {
                        player.SetPlayerPos(terminal.GetTerminalPosition() + new Vector3(0, 1, 0));
                        //set the player position to the terminal position
                        break;
                    }
                }
            }
            else
            {
                SceneManager.LoadScene(playerLevel);
               

            }

        }
        else
        {
            //display error message
            Debug.Log("No save file found. Please create a new game.");
        }
    }
    public void newGame()
    {
        // Create a new game save file
        playerChkpnt = 0;
        playerLevel = 0;
        string path = Application.persistentDataPath + "/saveFile.json";
        File.WriteAllText(path, "{}"); // Create an empty JSON file
        overWriteGame();
    }
    public void exitGame()
    {

    }
    public void pauseGame()
    {

    }
    public void resumeGame()
    {

    }
    public void overWriteGame()
    {
        // Overwrite the existing game save file
        SaveData data = new SaveData();
        data.playerLevel = playerLevel;
        data.playerChkpnt = playerChkpnt;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }



    /*WIP public void ContinueBtn1()
     {
         canvases[2].enabled = true;
     }
     public void continueBtn2()
     {
         loadGame();
         canvases[0].enabled = false;
     }
     public void SaveBtn1()
     {
         canvases[1].enabled = true;
     }
     public void SaveBtn2()
     {
         newGame();
         canvases[0].enabled = false;
     }
     */
    void Update()
    {
        if (isLoaded == false)
        {
            if (File.Exists(Application.persistentDataPath + "/saveFile.json"))
            {
                string json = File.ReadAllText(Application.persistentDataPath + "/saveFile.json");
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                playerLevel = data.playerLevel;
                playerChkpnt = data.playerChkpnt;
                TerminalScript[] terminals = FindObjectsByType<TerminalScript>(FindObjectsSortMode.None);
                foreach (TerminalScript terminal in terminals)
                {
                    if (terminal.saveTerminalIndex == playerChkpnt)
                    {
                        player.SetPlayerPos(terminal.GetTerminalPosition() + new Vector3(0, 1, 0));
                        //set the player position to the terminal position
                        isLoaded = true;
                        break;
                    }
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            newGame();
            loadGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            loadGame();  
        }
    }
}
