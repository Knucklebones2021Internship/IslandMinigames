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

    // ======= TRIVIA SYSTEM ========
    // Timer for each of the question prompts. They last 15 seconds 
    private float questionTimer = 15f; 
    // +++ List of Questions +++
    public List<GameObject> questionList;
    // +++ List of Answers +++
    public List<GameObject> answerList; 

    // Boolean for whether the answer for the prompt is correct or 
    // incorrect, which determines whether holding the potato will 
    // increase/decrease player score time 
    public bool correctAnswer; 
    // Index for prompts
    private int promptIndex = 0; 

    // UI for the increase/decrease of player score time 
    public GameObject scoreTimerChange;  
    // Boolean for whether the score timer change is being shown 
    private bool sTimerShown = false; 
    // The amount of time the score timer UI is shown, which will be 1.5 seconds 
    private float sTimerCooldown = 1.5f;   
    // =============================== 

    // <summary> 
    // Randomly determine the first player to hold the potato 
    // </summary>
    void Start() {   
        RandomPotato();
        correctAnswer = true; 

        // Script for the hot potato 
        Scripts_Potato_HotPotato_BrianLin potScript = hotPotato.GetComponent<Scripts_Potato_HotPotato_BrianLin>();
        // Set the hot potato to type "correct" 
        potScript.potType = true;
    }

    // <summary> 
    // Checks the timers to determine whether its time for a game over
    // Changes the color of the timers when certain numbers have been reached   
    // </summary>
    void Update() {
        questionTimer -= Time.deltaTime;
        
        if (questionTimer <= 0) {
            questionList[promptIndex].SetActive(false);
            answerList[promptIndex].SetActive(false);

            promptIndex++; 
            if (promptIndex % 2 == 1) { 
                correctAnswer = false; 
                Scripts_Potato_HotPotato_BrianLin potScript = hotPotato.GetComponent<Scripts_Potato_HotPotato_BrianLin>();
                potScript.potType = false;
            }
            else if (promptIndex % 2 == 0) { 
                correctAnswer = true; 
                Scripts_Potato_HotPotato_BrianLin potScript = hotPotato.GetComponent<Scripts_Potato_HotPotato_BrianLin>();
                potScript.potType = true; 
            }

            questionList[promptIndex].SetActive(true);
            answerList[promptIndex].SetActive(true);

            StartCoroutine(ShowScoreTime());
            questionTimer = 15f;
        }
        
        if (sTimerShown) { sTimerCooldown -= Time.deltaTime; }
        if (sTimerCooldown <= 0) { 
            scoreTimerChange.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            sTimerCooldown = 1.5f; 
            sTimerShown = false;
        }

        int endGlobalTime = 0;
        int endPlayerTime = 0;
        int.TryParse(globalTimer.GetComponent<TMPro.TextMeshProUGUI>().text, out endGlobalTime);
        int.TryParse(playerTimer.GetComponent<TMPro.TextMeshProUGUI>().text, out endPlayerTime);
        
        if (endGlobalTime <= 10) {
            globalTimer.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
        } 

        if (endPlayerTime <= 3) {
            playerTimer.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
        } else {
            playerTimer.GetComponent<TMPro.TextMeshProUGUI>().color = pTimerColor;
        }

        // If a player with a potato holds it for more than 6 seconds, the potato is 
        // randomly tossed to another player 
        if (endPlayerTime <= 0) {
            //GameOver(); 
            //playerTimer.SetActive(false); 
            for (int i = 1; i < 5; i++) {
                Scripts_Player_HotPotato_Brian pScript = playerList[i-1].GetComponent<Scripts_Player_HotPotato_Brian>();

                if (pScript.isHoldingPotato) {
                    pScript.isHoldingPotato = false; 
                    pScript.justReceivedPotato = false;
                    pScript.timerCountdown = false;
                    endPlayerTime = 6;
                    RandomPotato(); 
                    break;
                }             
            }         
        } 
    } 

    // <summary> 
    // Randomly chooses a player to get a potato 
    // </summary>
    void RandomPotato() {
        int pid = Random.Range(1, playerList.Count + 1);
        Scripts_Player_HotPotato_Brian pScript = playerList[pid-1].GetComponent<Scripts_Player_HotPotato_Brian>();
        pScript.isHoldingPotato = true;
        pScript.justReceivedPotato = true;
        hotPotato.transform.position = pScript.guide.transform.position;
        hotPotato.transform.rotation = pScript.guide.transform.rotation;
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

    IEnumerator ShowScoreTime() {
        // Get the first player's score time 
        Scripts_Player_HotPotato_Brian pScript = playerList[0].GetComponent<Scripts_Player_HotPotato_Brian>();
        scoreTimerChange.GetComponent<TMPro.TextMeshProUGUI>().text = pScript.scoreTime.ToString("F2"); 
        if (pScript.scoreTime > 0) {
            scoreTimerChange.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
        } else {
            scoreTimerChange.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
        }  
        sTimerShown = true; 
        yield return new WaitForSeconds(1.0f);   
    }
}
