using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

[DefaultExecutionOrder(-50)]
public class Scripts_GameManager_HotPotato_Brian : Scripts_BaseManager_Wyatt
{
	protected override void Awake() {
		base.Awake();
	}

    // List of players in the game 
    public List<GameObject> playerList; 
    public GameObject hotPotato;

    // Game Timers
    public GameObject globalTimer;
    public GameObject playerTimer;
    // Game Over UI  
    public GameObject gameOverPanel;
    // Player Number text for the player that lost  
    public GameObject playerNum;  

    // Color of orange for the player timer 
    private Color pTimerColor = new Color(1.0f, 140f/255f, 61f/255f);

    // <summary> 
    // Randomly determine the first player to hold the potato 
    // </summary>
    void Start()
    {
        int pid = Random.Range(1, playerList.Count + 1);
        Scripts_Player_HotPotato_Brian pScript = playerList[pid-1].GetComponent<Scripts_Player_HotPotato_Brian>();
        pScript.isHoldingPotato = true;
        pScript.justReceivedPotato = true;
        hotPotato.transform.position = pScript.guide.transform.position;
        hotPotato.transform.rotation = pScript.guide.transform.rotation;
    }

    // <summary> 
    // Checks the timers to determine whether its time for a game over
    // Changes the color of the timers when certain numbers have been reached   
    // </summary>
    void Update() {
        int endGlobalTime = 0;
        int endPlayerTime = 0;
        int.TryParse(globalTimer.GetComponent<TMPro.TextMeshProUGUI>().text, out endGlobalTime);
        int.TryParse(playerTimer.GetComponent<TMPro.TextMeshProUGUI>().text, out endPlayerTime);
        
        if (endGlobalTime <= 80) {
            globalTimer.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
        } 

        if (endPlayerTime <= 3) {
            playerTimer.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
        } else {
            playerTimer.GetComponent<TMPro.TextMeshProUGUI>().color = pTimerColor;
        }

        if (endGlobalTime <= 75 || endPlayerTime <= 0) {
            GameOver(); 
        } 
    }
  
    // <summary> 
    // Sets the timers invisible and displays the game over panel
    // </summary>
    void GameOver() {
        playerTimer.SetActive(false); 
        globalTimer.SetActive(false);

        for (int i = 1; i < 5; i++) {
            Scripts_Player_HotPotato_Brian pScript = playerList[i-1].GetComponent<Scripts_Player_HotPotato_Brian>();
            pScript.gameOver = true;
            
            // Change the UI for the losing player number 
            if (pScript.isHoldingPotato) {
                playerNum.GetComponent<TMPro.TextMeshProUGUI>().text = i.ToString("0"); 
                gameOverPanel.SetActive(true);
            }             
        }
    }
}
