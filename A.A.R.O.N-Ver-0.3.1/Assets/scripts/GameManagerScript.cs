using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class SaveData
{
    public int playerLevel;
    public int playerChkpnt;
    public int highestLevel;
    
}
public class GameManagerScript : MonoBehaviour
{
    int playerLevel = 0;
    int highestLevel = 0;
    public int playerChkpnt = 0;
    playerController player = null;
    public Canvas continueCanvas;
    public List<Canvas> canvases = new List<Canvas>();
    bool isLoaded;
    public Button btn;
    [HideInInspector]public int selectedLevel = 0;
    void Start()
    {
        isLoaded = false;
        player = FindAnyObjectByType<playerController>();
        if (File.Exists(Application.persistentDataPath + "/saveFile.json") == false)
        {
            btn.interactable = false;
        }
    }
    public void saveGame(int saveTerminalIndex)
    {

        playerChkpnt = saveTerminalIndex;
        playerLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if(playerLevel> highestLevel)
        {
            highestLevel = playerLevel;
        }
        if (File.Exists(Application.persistentDataPath + "/saveFile.json"))
        {
            overWriteGame();
        }
        else
        {
            newGame();
            overWriteGame();
        }
    }
    public void restartLevel()
    {
        playerChkpnt = 0;
        overWriteGame();
        loadGame();
   }
    public void loadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/saveFile.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/saveFile.json");
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            playerLevel = data.playerLevel;
            playerChkpnt = data.playerChkpnt;
            SceneManager.LoadScene(playerLevel);

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
        playerLevel = 1;
        highestLevel = 1; 
        string path = Application.persistentDataPath + "/saveFile.json";
        File.WriteAllText(path, "{}"); // Create an empty JSON file
        overWriteGame();
    }
    public void overWriteGame()
    {
        // Overwrite the existing game save file
        SaveData data = new SaveData();
        data.playerLevel = playerLevel;
        data.playerChkpnt = playerChkpnt;
        data.highestLevel = highestLevel;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }
    public void NextLevel()
    {
        playerChkpnt = 0;
        playerLevel++;
        if (playerLevel > highestLevel)
        {
            highestLevel = playerLevel;
        }
        overWriteGame();
        SceneManager.LoadScene(playerLevel);
    }
 
     public void ContinueBtn1()
    {
        print("continue 1 Pressed");
        canvases[2].gameObject.SetActive(true);
    }
     public void ContinueBtn2()
     {
         loadGame();   
         canvases[0].gameObject.SetActive(false);
     }
    public void NewGameBtn1()
    {
        canvases[1].gameObject.SetActive(true);
        print("New Game Button 1 Pressed");
     }
     public void NewGameBtn2()
     {
         newGame();
         loadGame();
         canvases[0].gameObject.SetActive(false);
     }
    public void BackBtn()
    {
        canvases[0].gameObject.SetActive(true);
        for (int i = 1; i < canvases.Count; i++)
        {
            canvases[i].gameObject.SetActive(false);
        }
    }
    public void ExitBtn1()
    {
        canvases[3].gameObject.SetActive(true);
    }
    public void ExitBtn2()
    {
        Application.Quit();
    }
    public void LevelSelectBtn1()
    {
        canvases[4].gameObject.SetActive(true);
        Button[] levelButtons = canvases[4].GetComponentsInChildren<Button>();
        print(highestLevel);
        for (int i = 1; i < levelButtons.Length; i++)
        {
            if (i <= highestLevel)
            {
                levelButtons[i].interactable = true;
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Level " + i;
            }
            else
            {
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Locked";
            }
        }
    }
    public void LevelSelectBtn2(int level)
    {
        selectedLevel = level;
        canvases[5].gameObject.SetActive(true); 
        GameObject temp = GameObject.Find("LevelSelectCustomText");
        TextMeshProUGUI tempText = temp.GetComponent<TextMeshProUGUI>();
        tempText.text = "Are you sure you want to select level " +selectedLevel+"? By selecting a level you will lose any progress in level you were previously playing.";
    }
    public void lvlSelectbackBtn()
    {
        canvases[4].gameObject.SetActive(false);
        canvases[5].gameObject.SetActive(false);
    }
    public void LevelSelectBtn3()
    {
        playerChkpnt = 0;
        playerLevel = selectedLevel;
        overWriteGame();
        SceneManager.LoadScene(selectedLevel);

    }

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
                highestLevel = data.highestLevel;
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
                if (isLoaded == false && player != null)
                {
                    player.SetPlayerPos(new Vector3(0, 1, 0)); //default position if no terminal found
                    isLoaded = true;
                }
                else { isLoaded = true; }
            }

        }
        if (Input.GetKeyDown(KeyCode.P) && Application.isEditor == true)
        {
            File.Delete(Application.persistentDataPath + "/saveFile.json");
        }
         if (Input.GetKeyDown(KeyCode.L)&&Application.isEditor==true)
        {
            NextLevel();
        }
    }
}
